using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkAuthenticator
{
    [Header("識別情報")]
    public string gameId = "My3DGame";   // プロジェクト固有名に変更推奨
    public string version = "0.1.0";   // クライアント/サーバーで一致させる

    // --- サーバー側: 起動/停止時にハンドラ登録 ---
    public override void OnStartServer()
    {
        // HandshakeRequest が届いたら OnServerHandshakeRequest を実行するコールバックを登録
        NetworkServer.RegisterHandler<HandshakeRequest>(OnServerHandshakeRequest, false);
    }

    public override void OnStopServer()
    {
        NetworkServer.UnregisterHandler<HandshakeRequest>();
    }

    // --- クライアント側: 起動/停止時にハンドラ登録 ---
    public override void OnStartClient()
    {
        // サーバーから届く HandshakeResponse を受け取る
        NetworkClient.RegisterHandler<HandshakeResponse>(OnClientHandshakeResponse, false);
    }

    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<HandshakeResponse>();
    }

    // --- Mirrorの認証フック ---
    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {
        // 何もしない：クライアントからの HandshakeRequest を待つ
        // （OnServerHandshakeRequest で Accept/Reject する）
    }

    public override void OnClientAuthenticate()
    {
        // 接続直後に HandshakeRequest を送る
        var req = new HandshakeRequest { _gameId = gameId, _version = version };
        Debug.Log($"[AUTH/CLIENT] send handshake: {req._gameId} v{req._version}");
        NetworkClient.Send(req);
    }

    // --- サーバーが HandshakeRequest を受け取ったとき ---
    void OnServerHandshakeRequest(NetworkConnectionToClient conn, HandshakeRequest req)
    {
        bool ok = (req._gameId == gameId && req._version == version);
        string msg = ok ? "OK" : $"Mismatch (srv:{gameId} {version} / cli:{req._gameId} {req._version})";

        // クライアントへ応答
        conn.Send(new HandshakeResponse { _accepted = ok, _message = msg });

        if (ok)
        {
            Debug.Log($"[AUTH/SERVER] accept: {conn.connectionId} {msg}");
            ServerAccept(conn);   // <- Mirror標準API
        }
        else
        {
            Debug.LogWarning($"[AUTH/SERVER] reject: {conn.connectionId} {msg}");
            ServerReject(conn);   // <- Mirror標準API
        }
    }

    // --- クライアントが HandshakeResponse を受け取ったとき ---
    void OnClientHandshakeResponse(HandshakeResponse res)
    {
        if (res._accepted)
        {
            Debug.Log($"[AUTH/CLIENT] accepted: {res._message}");
            ClientAccept();       // <- Mirror標準API
        }
        else
        {
            Debug.LogWarning($"[AUTH/CLIENT] rejected: {res._message}");
            ClientReject();       // <- Mirror標準API
        }
    }
}