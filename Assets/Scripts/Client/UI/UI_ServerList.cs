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

    //void Start()
    //{
    //    if (discovery != null)
    //        discovery.OnServerFound.AddListener(OnServerFound);
    //}
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

    void OnServerFound(ServerResponse info)
    {
        var btn = Instantiate(serverButtonPrefab, serverListParent);
        btn.GetComponentInChildren<Text>().text = $"{info.EndPoint.Address}:{info.uri?.Port ?? 7777}";

        string ip = info.EndPoint.Address.ToString();
        btn.onClick.AddListener(() =>
        {
            // 直接 NetworkManager.singleton を使って接続
            if (NetworkManager.singleton == null)
            {
                Debug.LogError("NetworkManager.singleton が null です。シーンに NetworkManager を配置して下さい。");
                return;
            }
            NetworkManager.singleton.networkAddress = ip;
            NetworkManager.singleton.StartClient();
            Debug.Log($"[UI] {ip} に接続開始");
        });
    }
}
