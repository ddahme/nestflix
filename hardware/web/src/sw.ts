/// <reference lib="webworker" />

self.addEventListener("install", (event) => {
  console.log("[SW] Installed");
  self.skipWaiting();
});

self.addEventListener("activate", (event) => {
  console.log("[SW] Activated");
});

// sw-db.js
const DB_NAME = "request-queue";
const STORE_NAME = "requests";

function openDatabase() {
  return new Promise((resolve, reject) => {
    const request = indexedDB.open(DB_NAME, 1);
    request.onupgradeneeded = () => {
      const db = request.result;
      db.createObjectStore(STORE_NAME, { autoIncrement: true });
    };
    request.onsuccess = () => resolve(request.result);
    request.onerror = () => reject(request.error);
  });
}

async function saveRequest(data) {
  const db = await openDatabase();
  const tx = db.transaction(STORE_NAME, "readwrite");
  tx.objectStore(STORE_NAME).add(data);
  return tx.complete;
}

async function getAllRequests() {
  const db = await openDatabase();
  const tx = db.transaction(STORE_NAME, "readonly");
  const store = tx.objectStore(STORE_NAME);
  return new Promise((resolve, reject) => {
    const request = store.getAll();
    request.onsuccess = () => resolve(request.result);
    request.onerror = () => reject(request.error);
  });
}

async function clearRequests() {
  const db = await openDatabase();
  const tx = db.transaction(STORE_NAME, "readwrite");
  tx.objectStore(STORE_NAME).clear();
  return tx.complete;
}

self.addEventListener("fetch", (event) => {
  console.log("fetch event", event);
  const { request } = event;

  // Only handle POST requests with JSON
  if (
    request.method === "POST" &&
    request.headers.get("Content-Type")?.includes("application/json")
  ) {
    event.respondWith(
      (async () => {
        try {
          const clonedRequest = request.clone();
          const body = await clonedRequest.json();

          if (!navigator.onLine) {
            // Client is offline — queue the request
            await saveRequest({
              url: request.url,
              method: request.method,
              headers: [...request.headers],
              body,
            });

            // Return a generic response to the client
            return new Response(
              JSON.stringify({
                message: "Request saved. Will be sent when online.",
              }),
              { status: 202, headers: { "Content-Type": "application/json" } }
            );
          } else {
            // Online — send request immediately
            return fetch(request);
          }
        } catch (err) {
          return new Response("Error handling request", { status: 500 });
        }
      })()
    );
  }
});
