using UnityEngine;
using UnityEngine.UI;
using Mirror.Discovery;
using Mirror;
using static LanDiscovery;

public class UI_ServerList : MonoBehaviour
{
    public LanDiscovery discovery;
    public Transform serverListParent;
    public Button serverButtonPrefab;

    void Awake()
    {
        if (discovery != null)
            discovery.OnServerFoundEvent.AddListener(OnServerFound);
    }
    public void RefreshServers()
    {
        // 既存リスト消去
        foreach (Transform child in serverListParent)
            Destroy(child.gameObject);

        // 検索開始
        discovery?.StartDiscovery();
        Debug.Log("[LAN] サーバー検索開始");
    }

    void OnServerFound(DiscoveryResponse info)
    {
        Debug.Log($"[UI] サーバー発見: {info._serverName} v{info._version} " +
                  $"({info._playerCount}/{info._maxPlayers}) at {info._address}");

        var btn = Instantiate(serverButtonPrefab, serverListParent);
        btn.GetComponentInChildren<Text>().text =
            $"{info._serverName} | v{info._version} | {info._playerCount}/{info._maxPlayers}";

        string ip = info._address;
        btn.onClick.AddListener(() =>
        {
            if (NetworkManager.singleton == null)
            {
                Debug.LogError("NetworkManager.singleton が null です。シーンに NetworkManager を配置して下さい。");
                return;
            }

            var transport = NetworkManager.singleton.transport as kcp2k.KcpTransport;
            if (transport != null)
            {
                transport.Port = (ushort)info._port; // サーバーの広告してきたポートに設定
                Debug.Log($"[Client] 接続ポートを {info._port} に変更しました");
            }

            NetworkManager.singleton.networkAddress = ip;
            NetworkManager.singleton.StartClient();
            Debug.Log($"[UI] {ip} に接続開始");
        });
    }
}
