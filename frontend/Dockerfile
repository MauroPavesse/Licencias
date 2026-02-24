FROM nginx:alpine
# Copiamos el contenido de la carpeta dist a la ruta de nginx
COPY ./dist /usr/share/nginx/html
# Copiamos nuestro archivo de configuración personalizado
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]