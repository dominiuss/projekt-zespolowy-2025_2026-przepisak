FROM node:20-alpine AS build

WORKDIR /my-app

COPY my-app/package*.json ./
RUN npm install

COPY my-app/ .
RUN npm run build

EXPOSE 5000

FROM node:20-alpine
RUN npm install -g serve
COPY --from=build /my-app/dist /my-app/dist
CMD ["serve", "-s", "/my-app/dist", "-l", "5000"]
