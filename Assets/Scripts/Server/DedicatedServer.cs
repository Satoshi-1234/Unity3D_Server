using Mirror;
using UnityEngine;


public class DedicatedServer : MonoBehaviour
{
    [System.NonSerialized] public int port = 7777;

    void Start()
    {
#if UNITY_SERVER && !UNITY_EDITOR
        Transport activeTransport = Transport.active;
        if (activeTransport is kcp2k.KcpTransport kcp)
        {
            kcp.Port = (ushort)port;
        }

        Debug.Log($"[DedicatedServer] Starting server on port {port}...");
        NetworkManager.singleton.StartServer();
#endif
    }
}