############################
# 1. BUILD UNITY WEBGL
############################
FROM unityci/editor:6000.0.64f1-webgl-3 AS builder

WORKDIR /project

# Copy toàn bộ source Unity
COPY . .

# Build WebGL (headless)
RUN unity-editor \
    -batchmode \
    -nographics \
    -quit \
    -projectPath /project \
    -executeMethod BuildWebGL.Build

############################
# 2. SERVE WITH NGINX
############################
FROM nginx:alpine

# Remove default config
RUN rm /etc/nginx/conf.d/default.conf

# Nginx config cho WebGL
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy WebGL build từ stage 1
COPY --from=builder /project/Build/WebGL /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
