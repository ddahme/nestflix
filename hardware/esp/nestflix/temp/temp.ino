
#include <WiFi.h>
#include <WebServer.h>
#include <DNSServer.h>
#include "SD_MMC.h"
#include "FS.h"
#include "cameraConfig.h"
#include "esp_camera.h"


const char *ssid = "Birdbox";
// const char *password = "12345678";
IPAddress apIP(192, 168, 4, 1);
const byte DNS_PORT = 53;
DNSServer dnsServer;

#define PART_BOUNDARY "123456789000000000000987654321"
static const char *_STREAM_CONTENT_TYPE = "multipart/x-mixed-replace;boundary=" PART_BOUNDARY;
static const char *_STREAM_BOUNDARY = "\r\n--" PART_BOUNDARY "\r\n";
static const char *_STREAM_PART = "Content-Type: image/jpeg\r\nContent-Length: %u\r\nX-Timestamp: %d.%06d\r\n\r\n";

// HTTP server on port 80
WebServer server(80);
String cameraError;

void setupAccessPoint();
void setupSdCard();
void setupCamera();
// void setupLed();

String getContentType(String filename);
void handleRequest();
void handleRoot();
void handleVideoStream();
void handleCapture();
// void handleLed();

void setup() {
  Serial.begin(115200);

  // Create Wi-Fi AP
  setupAccessPoint();
  // Setup sd-card
  // setupSdCard();
  // setupLed();
}

void loop() {
  server.handleClient();
}

void setupAccessPoint() {
  WiFi.softAP(ssid);
  WiFi.softAPConfig(apIP, apIP, IPAddress(255, 255, 255, 0));
  dnsServer.start(DNS_PORT, "*", apIP);
  server.onNotFound(handleRequest);
  server.on("/", handleRequest);
  server.on("/stream", HTTP_GET, handleVideoStream);
  server.on("/capture", HTTP_GET, handleCapture);
  // server.on("/live?", handleRequest);
  // server.on("/led", handleLed);
  // server.on("/accept", HTTP_POST, handleAccept);
  server.begin();
}

void setupSdCard() {
  if (!SD_MMC.begin()) {
    Serial.println("Card Mount Failed");
    return;
  }
}

void setupCamera() {
  camera_config_t config;
  config.ledc_channel = LEDC_CHANNEL_0;
  config.ledc_timer = LEDC_TIMER_0;
  config.pin_d0 = Y2_GPIO_NUM;
  config.pin_d1 = Y3_GPIO_NUM;
  config.pin_d2 = Y4_GPIO_NUM;
  config.pin_d3 = Y5_GPIO_NUM;
  config.pin_d4 = Y6_GPIO_NUM;
  config.pin_d5 = Y7_GPIO_NUM;
  config.pin_d6 = Y8_GPIO_NUM;
  config.pin_d7 = Y9_GPIO_NUM;
  config.pin_xclk = XCLK_GPIO_NUM;
  config.pin_pclk = PCLK_GPIO_NUM;
  config.pin_vsync = VSYNC_GPIO_NUM;
  config.pin_href = HREF_GPIO_NUM;
  config.pin_sccb_sda = SIOD_GPIO_NUM;
  config.pin_sccb_scl = SIOC_GPIO_NUM;
  config.pin_pwdn = PWDN_GPIO_NUM;
  config.pin_reset = RESET_GPIO_NUM;
  config.xclk_freq_hz = 20000000;
  config.frame_size = FRAMESIZE_UXGA;
  config.pixel_format = PIXFORMAT_JPEG;  // for streaming
  config.grab_mode = CAMERA_GRAB_WHEN_EMPTY;
  config.fb_location = CAMERA_FB_IN_PSRAM;
  config.jpeg_quality = 12;
  config.fb_count = 1;

  // if PSRAM IC present, init with UXGA resolution and higher JPEG quality
  // for larger pre-allocated frame buffer.
  if (config.pixel_format == PIXFORMAT_JPEG) {
    if (psramFound()) {
      config.jpeg_quality = 10;
      config.fb_count = 2;
      config.grab_mode = CAMERA_GRAB_LATEST;
    } else {
      // Limit the frame size when PSRAM is not available
      config.frame_size = FRAMESIZE_SVGA;
      config.fb_location = CAMERA_FB_IN_DRAM;
    }
  } else {
    // Best option for face detection/recognition
    config.frame_size = FRAMESIZE_240X240;
  }


  // camera init
  esp_err_t err = esp_camera_init(&config);
  if (err != ESP_OK) {
     cameraError = "Camera init failed";
    return;
  }

  sensor_t *s = esp_camera_sensor_get();
  // initial sensors are flipped vertically and colors are a bit saturated
  if (s->id.PID == OV3660_PID) {
    s->set_vflip(s, 1);        // flip it back
    s->set_brightness(s, 1);   // up the brightness just a bit
    s->set_saturation(s, -2);  // lower the saturation
  }
  // drop down frame size for higher initial frame rate
  if (config.pixel_format == PIXFORMAT_JPEG) {
    s->set_framesize(s, FRAMESIZE_QVGA);
  }

  // Setup LED FLash
  // setupLedFlash();
}

// void setupLed() {
//   pinMode(LED_GPIO_NUM, OUTPUT);
//   digitalWrite(LED_GPIO_NUM, LOW);
  
//   isLedOn = false;
// }

String getContentType(String filename) {
  if (filename.endsWith(".htm") || filename.endsWith(".html")) return "text/html";
  else if (filename.endsWith(".css")) return "text/css";
  else if (filename.endsWith(".js")) return "application/javascript";
  else if (filename.endsWith(".png")) return "image/png";
  else if (filename.endsWith(".jpg")) return "image/jpeg";
  else if (filename.endsWith(".gif")) return "image/gif";
  else if (filename.endsWith(".ico")) return "image/x-icon";
  else if (filename.endsWith(".txt")) return "text/plain";
  return "application/octet-stream";
}

void handleRequest() {
  String path = server.uri();
  // default
  if (path.endsWith("live")){
    path.remove(path.length() - 5);
  }
  if (path.endsWith("/")) path += "index.html";

  File file = SD_MMC.open(path);
  if (!file) {
    server.send(404, "text/plain", path);
    return;
  }

  String contentType = getContentType(path);
  server.streamFile(file, contentType);
  file.close();
}

void handleVideoStream() {
  // String path = server.uri();
  // server.send(200, "text/plain", "videoStream");
  // return;
  WiFiClient client = server.client();
  String boundary = "frame";
  server.sendHeader("Content-Type", "multipart/x-mixed-replace; boundary=" + boundary);
  while(client.connected()) {
    camera_fb_t * fb = esp_camera_fb_get();
    if(!fb) break;

    client.printf("--%s\r\n", boundary.c_str());
    client.printf("Content-Type: image/jpeg\r\n");
    client.printf("Content-Length: %u\r\n\r\n", fb->len);
    client.write(fb->buf, fb->len);
    client.printf("\r\n");

    esp_camera_fb_return(fb);
    delay(100); // ~10 FPS
  }
}

void handleCapture() {
  camera_fb_t * fb = esp_camera_fb_get();
  if(!fb) {
    server.send(500, "text/plain", "Camera capture failed");
    return;
  }
  server.sendHeader("Content-Type", "image/jpeg");
  server.sendHeader("Content-Length", String(fb->len));
  server.send(200, "image/jpeg", "");
  WiFiClient client = server.client();
  client.write(fb->buf, fb->len);
  esp_camera_fb_return(fb);
}

// void handleLed() {
//   if (isLedOn) {
//     digitalWrite(LED_GPIO_NUM, LOW);
//     isLedOn = false;
//     server.send(200, "text/plain", "led is on");
//   } else {
//     digitalWrite(LED_GPIO_NUM, HIGH);
//     isLedOn = true;
//     server.send(200, "text/plain", "led is off");
//   }
// }

void handleLive(){

}
