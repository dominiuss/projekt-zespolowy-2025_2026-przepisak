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
