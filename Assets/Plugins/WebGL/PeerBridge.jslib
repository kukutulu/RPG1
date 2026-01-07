mergeInto(LibraryManager.library, {
    // Exposed to C# via [DllImport("__Internal")] SendToPeerFromUnity
    SendToPeerFromUnity: function (ptr) {
        var msg = UTF8ToString(ptr);

        // If the global JS handler exists (set in index.html), call it.
        if (typeof window !== 'undefined' && typeof window.SendToPeerFromUnity === 'function') {
            window.SendToPeerFromUnity(msg);
        } else {
            console.warn('[PeerBridge.jslib] window.SendToPeerFromUnity not found; message dropped:', msg);
        }
    },
});

