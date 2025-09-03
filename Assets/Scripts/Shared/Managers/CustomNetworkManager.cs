using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkAuthenticator
{
    [Header("識別情報")]
    public string gameId = "My3DGame";   // プロジェクト固有名に変更推奨
    public string version = "0.1.0";     // クライアント/サーバーで一致させる

    [Header("LAN 検出")]
    public LanDiscovery discovery;       // インスペクタでアタッチ

    // ========================================
    // Mirror 標準フック（サーバー側）
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
    // Mirror 標準フック（クライアント側）
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
    // 認証フック（サーバー）
    // ========================================
#if UNITY_SERVER || UNITY_EDITOR
    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {
        // 何もしない: HandshakeRequest を待つ
    }
#endif

    // ========================================
    // 認証フック（クライアント）
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
    // Handshake サーバー側
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
    // Handshake クライアント側
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
            Debug.Log("[LAN] サーバー広告開始");
        }
        else
            Debug.Log("discovery がありません");
    }

    public void StartClientWithDiscovery(string ip)
    {
        var nm = NetworkManager.singleton;
        if (nm == null) return;

        nm.networkAddress = ip;
        nm.StartClient();
        Debug.Log($"[LAN] {ip} に接続開始");
    }
}
//using Mirror;
//using UnityEngine;

//public class CustomNetworkManager : NetworkAuthenticator
//{
//    [Header("識別情報")]
//    public string gameId = "My3DGame";
//    public string version = "0.1.0";

//    [Header("LAN 検出")]
//    public LanDiscovery discovery; // Inspectorでアタッチしておく

//    // --------------------------
//    // 認証コード（既存部分）
//    // --------------------------
//    public override void OnStartServer()
//    {
//        Debug.Log("[Auth] サーバー認証開始");
//    }

//    public override void OnStartClient()
//    {
//        Debug.Log("[Auth] クライアント認証開始");
//    }

//    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
//    {
//        Debug.Log("[Auth] サーバー側でクライアント認証中…");
//        ServerAccept(conn); // 認証成功
//    }

//    public override void OnClientAuthenticate()
//    {
//        Debug.Log("[Auth] クライアント認証中…");
//        ClientAccept(); // 認証成功
//    }

//    // --------------------------
//    // LANサーバー起動/接続補助
//    // --------------------------
//    public void StartServerWithDiscovery()
//    {
//        var nm = NetworkManager.singleton;
//        if (nm == null)
//        {
//            Debug.LogError("[CustomNetworkManager] NetworkManager.singleton が見つかりません。シーンに NetworkManager を配置して下さい。");
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
//            Debug.Log("[LAN] サーバー広告開始");
//        }
//    }

//    public void StartClientWithDiscovery(string ip)
//    {
//        var nm = NetworkManager.singleton;
//        if (nm == null)
//        {
//            Debug.LogError("[CustomNetworkManager] NetworkManager.singleton が見つかりません。");
//            return;
//        }

//        nm.networkAddress = ip;
//        nm.StartClient();
//        Debug.Log($"[LAN] {ip} に接続開始");
//    }
//}
