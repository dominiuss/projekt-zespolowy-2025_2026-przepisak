FROM node:20-alpine

WORKDIR /my-app

COPY my-app/package*.json ./
RUN npm install

COPY my-app/ .

EXPOSE 5000

# Uruchamianie aplikacji dev lub prod
CMD ["npm", "start"]

