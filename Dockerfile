# --- Stage 1: Build Unity Project ---
# LƯU Ý: Thay đổi phiên bản 2022.3.10f1 thành phiên bản chính xác project bạn đang dùng
# Xem phiên bản tại ProjectSettings/ProjectVersion.txt
FROM unityci/editor:6000.0.64f1-webgl-3 AS builder

# 1. Thiết lập thư mục làm việc
WORKDIR /project

# 2. Tạo thư mục chứa License (Bắt buộc phải đúng đường dẫn này trên Linux)
RUN mkdir -p /root/.local/share/unity3d/Unity/

# 3. COPY file license từ máy bạn vào đúng chỗ trong Docker
COPY unity_license.ulf /root/.local/share/unity3d/Unity/Unity_lic.ulf

# 4. Copy mã nguồn game
COPY . .

# 5. Chạy lệnh Build
# Lưu ý: Đã xóa các tham số username/password vì đã có file license ở trên
RUN mkdir -p build/WebGL && \
    /opt/unity/Editor/Unity \
    -projectPath /project \
    -quit \
    -batchmode \
    -nographics \
    -buildTarget WebGL \
    -executeMethod BuildCommand.PerformBuild \
    -logFile /project/build.log || { cat /project/build.log; exit 1; }

# --- Stage 2: Serve with Nginx ---
FROM nginx:alpine

# Copy kết quả build sang Nginx
COPY --from=builder /project/build/WebGL /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
