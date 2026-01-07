using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Bridge between Unity (C#) and the browser JS (PeerJS side).
/// 
/// Unity part is fully set up here. JS implementation can be added later.
/// </summary>
public class PeerManager : MonoBehaviour
{
    // --- JS → Unity entry point (called from JavaScript with SendMessage) ---

    /// <summary>
    /// Called from JS when a message arrives from a peer.
    /// Signature must be (string) to match Unity's SendMessage call.
    /// </summary>
    /// <param name="json">Payload from JS/PeerJS, usually JSON text.</param>
    public void OnPeerMessage(string json)
    {
        Debug.Log($"[PeerManager] Received from peer: {json}");

        // TODO: Parse and route this data into your game systems.
        // Example:
        // var data = JsonUtility.FromJson<YourMessageType>(json);
        // HandleMessage(data);
    }

    /// <summary>
    /// Called from JS when the PeerJS instance becomes ready.
    /// </summary>
    /// <param name="peerId">The PeerJS ID assigned to this host.</param>
    public void OnPeerReady(string peerId)
    {
        Debug.Log($"[PeerJS] Ready with ID: {peerId}");
    }


    // --- Unity → JS bridge (we only wire the C# side here) ---

#if !UNITY_EDITOR && UNITY_WEBGL
    // This expects a JS function named SendToPeerFromUnity(string) to exist
    // in the WebGL page or a .jslib plugin.
    [DllImport("__Internal")]
    private static extern void SendToPeerFromUnity(string message);
#else
    // In editor and non-WebGL builds, provide a safe stub so calls don't break.
    private static void SendToPeerFromUnity(string message)
    {
        Debug.Log($"[PeerManager] (Editor/Non-WebGL) Would send to peer: {message}");
    }
#endif

    /// <summary>
    /// Call this from your gameplay code to send a message to the browser/PeerJS.
    /// </summary>
    public void SendToPeer(string message)
    {
        SendToPeerFromUnity(message);
    }
}


