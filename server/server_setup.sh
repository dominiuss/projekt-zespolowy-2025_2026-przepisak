#!/bin/bash
set -e

### KONFIGURACJA ###
GITLAB_URL="https://git.ita.wat.edu.pl"
GITLAB_TOKEN="glrt-vyAkqLw-bR-1XqC30MC5LW86MQpwOjU5CnQ6Mwp1OjJ1EQ.01.1a1n9cgle"
RUNNER_NAME="gitlab-runner-container"
DOCKER_IMAGE="docker:latest"
RUNNER_TAGS="docker,linux,student"

# === 1. Aktualizacja systemu ===
echo "[1/6] Aktualizacja systemu..."
apt update -y && apt upgrade -y && apt install nano -y

# === 2. Zabezpieczenie systemu ===
echo "[2/6] Zabezpieczanie SSH i podstawowe ustawienia..."
sed -i 's/^#PermitRootLogin.*/PermitRootLogin no/' /etc/ssh/sshd_config			# Logowanie na konto root po ssh jest zablowane
sed -i 's/^#PasswordAuthentication.*/PasswordAuthentication no/' /etc/ssh/sshd_config	# Uwierzytelnianie za pomoca hasla jest zablokowane
systemctl restart ssh

# Instalacja i konfiguracja zapory (UFW)
echo "[2.1] Instalacja i konfiguracja UFW..."
apt install -y ufw
export PATH=$PATH:/usr/sbin
ufw --force reset
ufw allow OpenSSH
ufw allow 2375/tcp			# Bedzie tu wymagana zmiana, gdy zostana ustalone porty, na ktorych bedzie operowac aplikacja
ufw --force enable

# === 3. Instalacja Dockera ===
echo "[3/6] Instalacja Dockera..."
if [ -f /etc/apt/keyrings/docker.gpg ]; then
    rm /etc/apt/keyrings/docker.gpg
fi

apt update -y
apt install -y ca-certificates curl gnupg lsb-release

# Naprawione repozytorium Dockera
mkdir -p /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/debian/gpg | gpg --dearmor -o /etc/apt/keyrings/docker.gpg

ARCH=$(dpkg --print-architecture)
CODENAME=$(lsb_release -cs)

echo "deb [arch=$ARCH signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/debian $CODENAME stable" \
  > /etc/apt/sources.list.d/docker.list

apt update -y
apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

systemctl enable docker
systemctl start docker

# Dodanie użytkownika 'student' do grupy docker
usermod -aG docker student
