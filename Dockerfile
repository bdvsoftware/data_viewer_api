FROM ubuntu:22.04

ENV DEBIAN_FRONTEND=noninteractive

# 1. Instala dependencias del sistema
RUN apt-get update && apt-get install -y \
    libc6 \
    zlib1g \
    libgcc1 \
    libstdc++6 \
    libopencv-dev \
    libopencv-core-dev \
    libopencv-imgproc-dev \
    libopencv-imgcodecs-dev \
    libopencv-highgui-dev \
    libgl1-mesa-glx \
    libglib2.0-0 \
    ffmpeg \
    libsm6 \
    libxext6 \
    libxrender1 \
    libgtk2.0-dev \
    libtesseract-dev \
    tesseract-ocr \
    libavcodec58 \
    libavformat58 \
    libavutil56 \
    libopenexr-dev \
    && rm -rf /var/lib/apt/lists/*

# 2. Copia los binarios ya publicados de tu app
WORKDIR /App
COPY ./out ./

# 3. Configura el path de librerías
ENV LD_LIBRARY_PATH=/App:/lib:/usr/lib:/usr/local/lib:/usr/lib/x86_64-linux-gnu

# 4. Symlinks por compatibilidad con OpenCvSharp
RUN ln -s /App/libOpenCvSharpExtern.so /App/OpenCvSharpExtern.so || true && \
    ln -s /App/libOpenCvSharpExtern.so /App/OpenCvSharpExtern || true && \
    ln -s /App/libOpenCvSharpExtern.so /App/libOpenCvSharpExtern || true && \
    ln -s /usr/lib/x86_64-linux-gnu/libIlmImf-2_3.so.24 /usr/lib/x86_64-linux-gnu/libIlmImf-2_5.so.25 || true

# 5. Lanza la aplicación
ENTRYPOINT ["/App/DataViewerApi"]
