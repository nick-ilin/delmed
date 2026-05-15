# Stage 1: Build
FROM node:20-alpine AS build
WORKDIR /app

# Копируем package.json и устанавливаем зависимости
COPY package*.json ./
RUN npm ci --force

# Копируем исходники и собираем
COPY . .
RUN npm run build

# Stage 2: Serve with nginx
FROM nginx:alpine

# Копируем собранное приложение
COPY --from=build /app/dist/medd-frontend/browser /usr/share/nginx/html

# Копируем конфиг nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]