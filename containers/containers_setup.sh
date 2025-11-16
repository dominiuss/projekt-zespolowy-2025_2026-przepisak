#!/bin/bash
set -e

# === 1. Aktualizacja systemu ===
echo "[1/1] Aktualizacja systemu..."
apt update -y && apt upgrade -y && apt install ssh -y && apt install nano -y && apt install sudo -y
