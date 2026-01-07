README

## WebGL + PeerJS Setup

This project can build to WebGL and use PeerJS so that other users (viewers) can watch a host player’s game in a browser.

### Unity Side

- **Peer bridge script**
  - `Assets/Scripts/Game/PeerManager.cs`:
    - Receives data from JS via `OnPeerMessage(string json)` (called from JavaScript with `unityInstance.SendMessage("PeerManager", "OnPeerMessage", payload)`).
    - Sends data to JS via `SendToPeer(string message)`, which calls a JavaScript function `SendToPeerFromUnity` exposed to WebGL.
  - Add a GameObject named `PeerManager` to your scene and attach the `PeerManager` component.

- **WebGL plugin for C# → JS**
  - `Assets/Plugins/WebGL/PeerBridge.jslib`:
    - Exports `SendToPeerFromUnity` for WebGL builds.
    - Forwards messages from C# to a global `window.SendToPeerFromUnity` function that is defined in the WebGL template HTML.

- **WebGL template with PeerJS**
  - `Assets/WebGLTemplates/PeerTemplate/index.html`:
    - Standard Unity WebGL template that loads the build and creates the Unity instance.
    - Includes PeerJS from CDN.
    - Sets up a `Peer` instance, listens for incoming connections, and forwards received data into Unity via `unityInstance.SendMessage("PeerManager", "OnPeerMessage", JSON.stringify(data));`.
    - Defines a global JS function `SendToPeerFromUnity(message)` and assigns it to `window.SendToPeerFromUnity` so C# `[DllImport("__Internal")]` can call it.
  - In the Unity Editor:
    - `Edit` → `Project Settings…` → `Player` → select the **WebGL** tab.
    - Under **Resolution and Presentation**, set **WebGL Template** to `PeerTemplate`.

### JS / Server Side

The Unity project assumes a PeerJS environment but does not define your server setup. On the web / server side you should:

- **Run or configure a PeerJS signaling server**
  - For development you can use the default PeerJS cloud with `new Peer()`.
  - For more control, run your own PeerServer, for example:
    - `npm install peer -g`
    - `peerjs --port 9000 --path /myapp`
  - In your WebGL template and viewer app, configure:
    - `new Peer('some-id', { host: 'your-domain.com', port: 9000, path: '/myapp', secure: true });`

- **Host the WebGL build**
  - Serve the built WebGL files (including the PeerTemplate `index.html`) from a web server (nginx, Node/Express, static hosting, etc.).
  - Prefer HTTPS so WebRTC (used by PeerJS) works reliably in browsers.

### Watching a Host Player

The goal is: one user (host) runs the Unity WebGL game, and other users (viewers) connect via another web page to watch.

- **Decide what viewers receive**
  - **Option A – Video/screen stream**: capture the Unity canvas or screen in the host browser and send it as a WebRTC media stream to viewers.
  - **Option B – Game state**: send JSON snapshots of positions/HP/events; viewer renders its own 2D view in HTML/JS.

- **Host (Unity WebGL page)**
  - Acts as the “host” PeerJS peer, for example `new Peer('game-host', { host, port, path, secure });`.
  - Accepts multiple incoming PeerJS connections and treats most of them as viewers.
  - For **state streaming**: periodically call `PeerManager.SendToPeer(...)` from C# to broadcast game state to all connected peers.
  - For **video streaming** (JS side only):
    - Capture a media stream, e.g. `const stream = canvas.captureStream(30);`.
    - For each connecting peer, call `peer.call(viewerPeerId, stream);`.

- **Viewer web app**
  - Separate HTML/JS app that initializes its own Peer:
    - `const peer = new Peer(null, { host, port, path, secure });` (viewer gets a random ID).
  - To receive **state**:
    - `const conn = peer.connect('game-host');` and listen to `conn.on('data', state => { /* update UI */ });`.
  - To receive **video**:
    - Implement `peer.on('call', call => { call.answer(); call.on('stream', remoteStream => { /* show in <video> */ }); });`.
  - Provide simple UI to show/enter the host ID (e.g., `game-host`) and to display the received state or video.

- **Protocol and permissions**
  - Define a simple message schema (e.g., `{ type: 'state', players: [...], time: ... }`, `{ type: 'event', name: 'hit', value: 10 }`).
  - In `PeerManager.OnPeerMessage`, parse JSON and decide which messages are trusted (e.g., ignore inputs from viewers, or only accept input from a specific controller peer ID).

This README section is a high-level checklist so you remember what is configured in Unity and what still needs to be set up in your JS/server environment to support a “host + viewers” streaming/watch experience.