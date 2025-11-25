#!/bin/bash
set -e

### KONFIGURACJA ###
GITLAB_URL="https://git.ita.wat.edu.pl"
GITLAB_TOKEN="glrt-vyAkqLw-bR-1XqC30MC5LW86MQpwOjU5CnQ6Mwp1OjJ1EQ.01.1a1n9cgle"
RUNNER_NAME="gitlab-runner-server"
DOCKER_IMAGE="docker:latest"
RUNNER_TAGS="docker,linux,student"

# === 1. Aktualizacja systemu ===
echo "[1/5] Aktualizacja systemu..."
apt update -y && apt upgrade -y && apt install ssh -y && apt install nano -y && apt install sudo -y

# === 2. Instalacja Dockera na Ubuntu 24 ===
echo "[2/5] Instalacja Dockera..."
if [ -f /etc/apt/keyrings/docker.gpg ]; then
    rm /etc/apt/keyrings/docker.gpg
fi
# Zainstaluj wymagane pakiety
apt update -y
apt install -y ca-certificates curl gnupg lsb-release

# Utwórz katalog na klucz GPG
mkdir -p /etc/apt/keyrings

# Pobierz klucz GPG Dockera
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | gpg --dearmor -o /etc/apt/keyrings/docker.gpg

ARCH=$(dpkg --print-architecture)
CODENAME=$(lsb_release -cs)

# Dodaj repozytorium Dockera dla Ubuntu
echo "deb [arch=$ARCH signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $CODENAME stable" \
  > /etc/apt/sources.list.d/docker.list

# Zaktualizuj listę pakietów
apt update -y

# Zainstaluj Dockera i wtyczki
apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

# Dodaj użytkownika 'root' do grupy docker
usermod -aG docker root

echo "[2/5] Instalacja Dockera zakończona."

# === 3. Instalacja GitLab Runnera ===
echo "[3/5] Instalacja GitLab Runnera..."
curl -L https://packages.gitlab.com/install/repositories/runner/gitlab-runner/script.deb.sh | bash
apt install -y gitlab-runner

# === 4. Rejestracja GitLab Runnera ===
echo "[4/5] Rejestracja GitLab Runnera..."
#docker run --rm -v $(pwd)/config:/etc/gitlab-runner gitlab/gitlab-runner:latest register --non-interactive \
gitlab-runner register --non-interactive \
  --url "$GITLAB_URL" \
  --registration-token "$GITLAB_TOKEN" \
  --executor "docker" \
  --docker-privileged \
  --docker-volumes "/cache" \
  --docker-image "$DOCKER_IMAGE" \
  --description "$RUNNER_NAME" \
  --tls-ca-file="/usr/local/share/ca-certificates/ca.local.crt"

# === 5. Uruchomienie GitLab Runnera ===
echo "[5/5] Uruchamianie GitLab Runnera..."

# Sprawdzenie, czy runner już działa
if pgrep -x "gitlab-runner" > /dev/null; then
    echo "GitLab Runner już działa."
else
    # Uruchomienie runnera w tle
    gitlab-runner run --working-directory /etc/gitlab-runner --config /etc/gitlab-runner/config.toml &
    echo "GitLab Runner uruchomiony w tle."
fi

echo "GitLab Runner działa."
echo "=== Gotowe! ==="
echo "Runner zostal zainstalowany i zarejestrowany jako: $RUNNER_NAME"
echo "Instalacja i konfiguracja zakończona pomyślnie!"

echo "Teraz, uruchomię kontenery, na których będzie działać aplikacja."

echo "Uruchamianie kontenerów z docker-compose..."
docker compose -f /git/containers/docker-compose.yml up -d

# Funkcja czekająca na pełną gotowość kontenera
wait_for_container() {
    local container_name="$1"
    echo "Czekam na uruchomienie kontenera $container_name..."

    # Czekamy aż kontener będzie w stanie "running" i opcjonalnie "healthy"
    while [ "$(docker inspect -f '{{.State.Running}}' "$container_name")" != "true" ]; do
        sleep 2
    done

    # Jeśli masz zdefiniowany healthcheck, odkomentuj poniższą linię:
    # while [ "$(docker inspect -f '{{.State.Health.Status}}' "$container_name")" != "healthy" ]; do sleep 2; done

    echo "Kontener $container_name jest gotowy."
}

# Pobranie listy kontenerów z docker-compose
containers=$(docker compose -f /git/containers/docker-compose.yml ps -q)

# Czekanie na wszystkie kontenery
for container in $containers; do
    wait_for_container "$container"
done

# Wykonanie wstępnej konfiguracji w każdym kontenerze
echo "Uruchamianie wstępnej konfiguracji w kontenerach..."
for container in $containers; do
    docker exec -i "$container" bash -c "/git/containers/container_setup.sh"
done

echo "Wstepna konfiguracja zakonczona."
