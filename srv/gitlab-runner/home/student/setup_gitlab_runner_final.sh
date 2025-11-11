#!/bin/bash
set -e

### KONFIGURACJA ###
GITLAB_URL="https://git.ita.wat.edu.pl"
#GITLAB_TOKEN="glrt-EZEva2mVdRG3wPJrHom7Km86MQpwOjU5CnQ6Mwp1OjJ1EQ.01.1a0vgq0qu"
GITLAB_TOKEK="glrt-vyAkqLw-bR-1XqC30MC5LW86MQpwOjU5CnQ6Mwp1OjJ1EQ.01.1a1n9cgle"
RUNNER_NAME="dind-runner"
DOCKER_IMAGE="docker:latest"
RUNNER_TAGS="docker,linux,student"

# === 1. Aktualizacja systemu ===
echo "[1/6] Aktualizacja systemu..."
apt update -y && apt upgrade -y

# === 2. Zabezpieczenie systemu ===
echo "[2/6] Zabezpieczanie SSH i podstawowe ustawienia..."
sed -i 's/^#PermitRootLogin.*/PermitRootLogin no/' /etc/ssh/sshd_config
sed -i 's/^#PasswordAuthentication.*/PasswordAuthentication no/' /etc/ssh/sshd_config
systemctl restart ssh

# Instalacja i konfiguracja zapory (UFW)
echo "[2.1] Instalacja i konfiguracja UFW..."
apt install -y ufw
export PATH=$PATH:/usr/sbin
ufw --force reset
ufw allow OpenSSH
ufw allow 2375/tcp
ufw --force enable

# === 3. Instalacja Dockera ===
echo "[3/6] Instalacja Dockera..."
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

# === 4. Instalacja GitLab Runnera ===
echo "[4/6] Instalacja GitLab Runnera..."
curl -L https://packages.gitlab.com/install/repositories/runner/gitlab-runner/script.deb.sh | bash
apt install -y gitlab-runner

# === 5. Rejestracja GitLab Runnera ===
echo "[5/6] Rejestracja GitLab Runnera..."
#gitlab-runner register --non-interactive \
docker run --rm -v $(pwd)/config:/etc/gitlab-runner gitlab/gitlab-runner:latest register --non-interactive \
  --url "$GITLAB_URL" \
  --registration-token "$GITLAB_TOKEN" \
  --executor "docker" \
  --docker-privileged \
  --docker-volumes "/cache" \
  --docker-image "$DOCKER_IMAGE" \
  --description "$RUNNER_NAME" \
  --tls-ca-file="/home/gitlab-runner/ca.local.crt"

# === 6. Uruchomienie i autostart ===
echo "[6/6] Uruchamianie GitLab Runnera..."
#systemctl enable gitlab-runner
#systemctl start gitlab-runner

echo "=== Sprawdzenie statusu ==="
#gitlab-runner status

echo "=== Gotowe! ==="
echo "Runner został zainstalowany i zarejestrowany jako: $RUNNER_NAME"
echo "Instalacja i konfiguracja zakończona pomyślnie!"

