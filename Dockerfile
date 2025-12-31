# --- Stage 1: Build Unity Project ---
# LƯU Ý: Thay đổi phiên bản 2022.3.10f1 thành phiên bản chính xác project bạn đang dùng
# Xem phiên bản tại ProjectSettings/ProjectVersion.txt
FROM unityci/editor:6000.0.64f1-webgl-3 AS builder

# 1. Khai báo biến để nhận email/pass từ dòng lệnh
ARG UNITY_EMAIL
ARG UNITY_PASSWORD

WORKDIR /project

# 2. Copy mã nguồn
COPY . .

# 3. Chạy lệnh Build (Thêm tham số đăng nhập trực tiếp)
# Lưu ý: Thêm -username và -password vào lệnh
RUN mkdir -p build/WebGL && \
    /opt/unity/Editor/Unity \
    -projectPath /project \
    -quit \
    -batchmode \
    -nographics \
    -username "$UNITY_EMAIL" \
    -password "$UNITY_PASSWORD" \
    -buildTarget WebGL \
    -executeMethod BuildCommand.PerformBuild \
    -logFile /project/build.log || { cat /project/build.log; exit 1; }

# --- Stage 2: Nginx ---
FROM nginx:alpine
COPY --from=builder /project/build/WebGL /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
