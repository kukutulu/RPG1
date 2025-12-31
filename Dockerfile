# --- Stage 1: Build Unity Project ---
# LƯU Ý: Thay đổi phiên bản 2022.3.10f1 thành phiên bản chính xác project bạn đang dùng
# Xem phiên bản tại ProjectSettings/ProjectVersion.txt
FROM unityci/editor:6000.0.64f1-webgl-1 AS builder

# Thiết lập thư mục làm việc
WORKDIR /project

# Copy mã nguồn vào container
COPY . .

# Cần License để build. Có 2 cách:
# 1. Truyền file .ulf (Unity License File) vào
# 2. Truyền Username/Password/Serial qua biến môi trường (ENV)
# Ở đây ta sẽ chờ nhận biến môi trường UNITY_LICENSE từ lệnh docker build

# Chạy lệnh Build
# -nographics: Không bật giao diện
# -batchmode: Chạy ngầm
# -quit: Tự thoát sau khi xong
# -executeMethod: Gọi hàm C# chúng ta vừa viết ở Bước 1
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

# Copy kết quả build từ Stage 1 sang folder html của Nginx
COPY --from=builder /project/build/WebGL /usr/share/nginx/html

# Mở port 80
EXPOSE 80

# Chạy Nginx
CMD ["nginx", "-g", "daemon off;"]
