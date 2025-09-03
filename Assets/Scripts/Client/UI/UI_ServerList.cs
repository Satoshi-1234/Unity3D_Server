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
        // �������X�g����
        foreach (Transform child in serverListParent)
            Destroy(child.gameObject);

        // �����J�n
        discovery?.StartDiscovery();
        Debug.Log("[LAN] �T�[�o�[�����J�n");
    }

    void OnServerFound(ServerResponse info)
    {
        var btn = Instantiate(serverButtonPrefab, serverListParent);
        btn.GetComponentInChildren<Text>().text = $"{info.EndPoint.Address}:{info.uri?.Port ?? 7777}";

        string ip = info.EndPoint.Address.ToString();
        btn.onClick.AddListener(() =>
        {
            // ���� NetworkManager.singleton ���g���Đڑ�
            if (NetworkManager.singleton == null)
            {
                Debug.LogError("NetworkManager.singleton �� null �ł��B�V�[���� NetworkManager ��z�u���ĉ������B");
                return;
            }
            NetworkManager.singleton.networkAddress = ip;
            NetworkManager.singleton.StartClient();
            Debug.Log($"[UI] {ip} �ɐڑ��J�n");
        });
    }
}
