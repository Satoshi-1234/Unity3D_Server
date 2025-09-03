using Mirror;
using UnityEngine;

//public class DedicatedServer : MonoBehaviour
//{
//    void Start()
//    {
//#if UNITY_SERVER || !UNITY_EDITOR
//        Debug.Log("[DedicatedServer] Headless server starting...");
//        var nm = NetworkManager.singleton;

//        if (nm != null)
//        {
//            // Authenticator が CustomNetworkManager の場合
//            if (nm.authenticator is CustomNetworkManager custom)
//            {
//                custom.StartServerWithDiscovery();
//            }
//            else
//            {
//                nm.StartServer();
//            }
//        }
//        else
//        {
//            Debug.LogError("[DedicatedServer] NetworkManager not found!");
//        }
//#endif
//    }
//}
public class DedicatedServer : MonoBehaviour
{
    void Start()
    {
#if UNITY_SERVER || !UNITY_EDITOR
        Debug.Log("[DedicatedServer] Headless server starting...");
        var nm = NetworkManager.singleton;

        if (nm != null)
        {
            var transport = nm.transport as kcp2k.KcpTransport;
            if (transport != null)
            {
                // 例: 7000〜8000 の範囲からランダムにポートを決定
                System.Random rand = new System.Random();
                transport.Port = (ushort)rand.Next(7000, 8000);
                Debug.Log($"[Server] ポートを {transport.Port} に設定しました");
            }

            nm.StartServer();

            var discovery = nm.GetComponent<LanDiscovery>();
            if (discovery != null)
            {
                discovery.AdvertiseServer();
                Debug.Log("[LAN] サーバー広告開始");
            }
        }
        else
        {
            Debug.LogError("[DedicatedServer] NetworkManager not found!");
        }
#endif
    }
}