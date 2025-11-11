#!/bin/bash
set -e

# === 1. Aktualizacja systemu ===
echo "[1/6] Aktualizacja systemu..."
apt update -y && apt upgrade -y && apt install ssh -y && apt install nano -y

echo "=== Gotowe! ==="
echo "Instalacja i konfiguracja zakończona pomyślnie!"

