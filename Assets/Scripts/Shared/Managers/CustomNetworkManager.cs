using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkAuthenticator
{
    [Header("���ʏ��")]
    public string gameId = "My3DGame";   // �v���W�F�N�g�ŗL���ɕύX����
    public string version = "0.1.0";     // �N���C�A���g/�T�[�o�[�ň�v������

    [Header("LAN ���o")]
    public LanDiscovery discovery;       // �C���X�y�N�^�ŃA�^�b�`

    // ========================================
    // Mirror �W���t�b�N�i�T�[�o�[���j
    // ========================================
#if UNITY_SERVER || UNITY_EDITOR
    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler<HandshakeRequest>(OnServerHandshakeRequest, false);
    }

    public override void OnStopServer()
    {
        NetworkServer.UnregisterHandler<HandshakeRequest>();
    }
#endif

    // ========================================
    // Mirror �W���t�b�N�i�N���C�A���g���j
    // ========================================
#if !UNITY_SERVER || UNITY_EDITOR
    public override void OnStartClient()
    {
        NetworkClient.RegisterHandler<HandshakeResponse>(OnClientHandshakeResponse, false);
    }

    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<HandshakeResponse>();
    }
#endif

    // ========================================
    // �F�؃t�b�N�i�T�[�o�[�j
    // ========================================
#if UNITY_SERVER || UNITY_EDITOR
    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {
        // �������Ȃ�: HandshakeRequest ��҂�
    }
#endif

    // ========================================
    // �F�؃t�b�N�i�N���C�A���g�j
    // ========================================
#if !UNITY_SERVER || UNITY_EDITOR
    public override void OnClientAuthenticate()
    {
        var req = new HandshakeRequest { _gameId = gameId, _version = version };
        Debug.Log($"[AUTH/CLIENT] send handshake: {req._gameId} v{req._version}");
        NetworkClient.Send(req);
    }
#endif

    // ========================================
    // Handshake �T�[�o�[��
    // ========================================
#if UNITY_SERVER || UNITY_EDITOR
    void OnServerHandshakeRequest(NetworkConnectionToClient conn, HandshakeRequest req)
    {
        bool ok = (req._gameId == gameId && req._version == version);
        string msg = ok ? "OK" : $"Mismatch (srv:{gameId} {version} / cli:{req._gameId} {req._version})";

        conn.Send(new HandshakeResponse { _accepted = ok, _message = msg });

        if (ok)
        {
            Debug.Log($"[AUTH/SERVER] accept: {conn.connectionId} {msg}");
            ServerAccept(conn);   // Mirror API
        }
        else
        {
            Debug.LogWarning($"[AUTH/SERVER] reject: {conn.connectionId} {msg}");
            ServerReject(conn);   // Mirror API
        }
    }
#endif

    // ========================================
    // Handshake �N���C�A���g��
    // ========================================
#if !UNITY_SERVER || UNITY_EDITOR
    void OnClientHandshakeResponse(HandshakeResponse res)
    {
        if (res._accepted)
        {
            Debug.Log($"[AUTH/CLIENT] accepted: {res._message}");
            ClientAccept();       // Mirror API
        }
        else
        {
            Debug.LogWarning($"[AUTH/CLIENT] rejected: {res._message}");
            ClientReject();       // Mirror API
        }
    }
#endif

    public void StartServerWithDiscovery()
    {
        var nm = NetworkManager.singleton;
        if (nm == null) return;

#if UNITY_SERVER
        nm.StartServer();
#else
        nm.StartHost();
#endif

        if (discovery != null)
        {
            discovery.AdvertiseServer();
            Debug.Log("[LAN] �T�[�o�[�L���J�n");
        }
        else
            Debug.Log("discovery ������܂���");
    }

    public void StartClientWithDiscovery(string ip)
    {
        var nm = NetworkManager.singleton;
        if (nm == null) return;

        nm.networkAddress = ip;
        nm.StartClient();
        Debug.Log($"[LAN] {ip} �ɐڑ��J�n");
    }
}
//using Mirror;
//using UnityEngine;

//public class CustomNetworkManager : NetworkAuthenticator
//{
//    [Header("���ʏ��")]
//    public string gameId = "My3DGame";
//    public string version = "0.1.0";

//    [Header("LAN ���o")]
//    public LanDiscovery discovery; // Inspector�ŃA�^�b�`���Ă���

//    // --------------------------
//    // �F�؃R�[�h�i���������j
//    // --------------------------
//    public override void OnStartServer()
//    {
//        Debug.Log("[Auth] �T�[�o�[�F�؊J�n");
//    }

//    public override void OnStartClient()
//    {
//        Debug.Log("[Auth] �N���C�A���g�F�؊J�n");
//    }

//    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
//    {
//        Debug.Log("[Auth] �T�[�o�[���ŃN���C�A���g�F�ؒ��c");
//        ServerAccept(conn); // �F�ؐ���
//    }

//    public override void OnClientAuthenticate()
//    {
//        Debug.Log("[Auth] �N���C�A���g�F�ؒ��c");
//        ClientAccept(); // �F�ؐ���
//    }

//    // --------------------------
//    // LAN�T�[�o�[�N��/�ڑ��⏕
//    // --------------------------
//    public void StartServerWithDiscovery()
//    {
//        var nm = NetworkManager.singleton;
//        if (nm == null)
//        {
//            Debug.LogError("[CustomNetworkManager] NetworkManager.singleton ��������܂���B�V�[���� NetworkManager ��z�u���ĉ������B");
//            return;
//        }

//#if UNITY_SERVER
//        nm.StartServer();
//#else
//        nm.StartHost();
//#endif

//        if (discovery != null)
//        {
//            discovery.AdvertiseServer();
//            Debug.Log("[LAN] �T�[�o�[�L���J�n");
//        }
//    }

//    public void StartClientWithDiscovery(string ip)
//    {
//        var nm = NetworkManager.singleton;
//        if (nm == null)
//        {
//            Debug.LogError("[CustomNetworkManager] NetworkManager.singleton ��������܂���B");
//            return;
//        }

//        nm.networkAddress = ip;
//        nm.StartClient();
//        Debug.Log($"[LAN] {ip} �ɐڑ��J�n");
//    }
//}
