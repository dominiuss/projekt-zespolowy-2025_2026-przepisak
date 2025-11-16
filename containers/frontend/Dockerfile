FROM node:20-alpine

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

# Budowanie aplikacji
RUN npm run build

# Uruchamianie aplikacji dev lub prod
CMD ["npm", "start"]

