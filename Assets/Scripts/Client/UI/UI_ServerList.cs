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
        // �������X�g����
        foreach (Transform child in serverListParent)
            Destroy(child.gameObject);

        // �����J�n
        discovery?.StartDiscovery();
        Debug.Log("[LAN] �T�[�o�[�����J�n");
    }

    void OnServerFound(DiscoveryResponse info)
    {
        Debug.Log($"[UI] �T�[�o�[����: {info._serverName} v{info._version} " +
                  $"({info._playerCount}/{info._maxPlayers}) at {info._address}");

        var btn = Instantiate(serverButtonPrefab, serverListParent);
        btn.GetComponentInChildren<Text>().text =
            $"{info._serverName} | v{info._version} | {info._playerCount}/{info._maxPlayers}";

        string ip = info._address;
        btn.onClick.AddListener(() =>
        {
            if (NetworkManager.singleton == null)
            {
                Debug.LogError("NetworkManager.singleton �� null �ł��B�V�[���� NetworkManager ��z�u���ĉ������B");
                return;
            }

            var transport = NetworkManager.singleton.transport as kcp2k.KcpTransport;
            if (transport != null)
            {
                transport.Port = (ushort)info._port; // �T�[�o�[�̍L�����Ă����|�[�g�ɐݒ�
                Debug.Log($"[Client] �ڑ��|�[�g�� {info._port} �ɕύX���܂���");
            }

            NetworkManager.singleton.networkAddress = ip;
            NetworkManager.singleton.StartClient();
            Debug.Log($"[UI] {ip} �ɐڑ��J�n");
        });
    }
}
