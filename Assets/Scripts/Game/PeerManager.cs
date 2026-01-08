using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Message types that can be received from viewers via PeerJS.
/// </summary>
[System.Serializable]
public class PeerMessage
{
    public string type;
    public string viewerId;
    public float? x;
    public float? y;
}

/// <summary>
/// Bridge between Unity (C#) and the browser JS (PeerJS side).
/// 
/// Unity part is fully set up here. JS implementation can be added later.
/// </summary>
public class PeerManager : MonoBehaviour
{
    private MonsterSpawn monsterSpawner;

    private void Start()
    {
        // Find the MonsterSpawn component in the scene
        monsterSpawner = FindObjectOfType<MonsterSpawn>();
        if (monsterSpawner == null)
        {
            Debug.LogWarning("[PeerManager] MonsterSpawn not found in scene. Spawn commands will be ignored.");
        }
    }

    // --- JS → Unity entry point (called from JavaScript with SendMessage) ---

    /// <summary>
    /// Called from JS when a message arrives from a peer.
    /// Signature must be (string) to match Unity's SendMessage call.
    /// </summary>
    /// <param name="json">Payload from JS/PeerJS, usually JSON text.</param>
    public void OnPeerMessage(string json)
    {
        Debug.Log($"[PeerManager] Received from peer: {json}");

        try
        {
            PeerMessage msg = JsonUtility.FromJson<PeerMessage>(json);
            
            if (msg.type == "spawn-monster")
            {
                HandleSpawnMonster(msg);
            }
            else if (msg.type == "viewer-hello")
            {
                Debug.Log($"[PeerManager] Viewer {msg.viewerId} connected");
            }
            else
            {
                Debug.LogWarning($"[PeerManager] Unknown message type: {msg.type}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[PeerManager] Failed to parse message: {e.Message}");
        }
    }

    private void HandleSpawnMonster(PeerMessage msg)
    {
        if (monsterSpawner == null)
        {
            Debug.LogWarning("[PeerManager] Cannot spawn: MonsterSpawn not found");
            return;
        }

        Debug.Log($"[PeerManager] Spawn command from viewer {msg.viewerId} at ({msg.x}, {msg.y})");
        monsterSpawner.SpawnMonsterManually(msg.x, msg.y);
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


