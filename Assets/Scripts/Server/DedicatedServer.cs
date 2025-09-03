using Mirror;
using UnityEngine;

public class DedicatedServer : MonoBehaviour
{
    void Start()
    {
#if UNITY_SERVER || !UNITY_EDITOR
        Debug.Log("[DedicatedServer] Headless server starting...");
        var nm = NetworkManager.singleton;

        if (nm != null)
        {
            // Authenticator Ç™ CustomNetworkManager ÇÃèÍçá
            if (nm.authenticator is CustomNetworkManager custom)
            {
                custom.StartServerWithDiscovery();
            }
            else
            {
                nm.StartServer();
            }
        }
        else
        {
            Debug.LogError("[DedicatedServer] NetworkManager not found!");
        }
#endif
    }
}