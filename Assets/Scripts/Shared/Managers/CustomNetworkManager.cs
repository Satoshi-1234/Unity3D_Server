using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkAuthenticator
{
    [Header("���ʏ��")]
    public string gameId = "My3DGame";   // �v���W�F�N�g�ŗL���ɕύX����
    public string version = "0.1.0";   // �N���C�A���g/�T�[�o�[�ň�v������

    // --- �T�[�o�[��: �N��/��~���Ƀn���h���o�^ ---
    public override void OnStartServer()
    {
        // HandshakeRequest ���͂����� OnServerHandshakeRequest �����s����R�[���o�b�N��o�^
        NetworkServer.RegisterHandler<HandshakeRequest>(OnServerHandshakeRequest, false);
    }

    public override void OnStopServer()
    {
        NetworkServer.UnregisterHandler<HandshakeRequest>();
    }

    // --- �N���C�A���g��: �N��/��~���Ƀn���h���o�^ ---
    public override void OnStartClient()
    {
        // �T�[�o�[����͂� HandshakeResponse ���󂯎��
        NetworkClient.RegisterHandler<HandshakeResponse>(OnClientHandshakeResponse, false);
    }

    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<HandshakeResponse>();
    }

    // --- Mirror�̔F�؃t�b�N ---
    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {
        // �������Ȃ��F�N���C�A���g����� HandshakeRequest ��҂�
        // �iOnServerHandshakeRequest �� Accept/Reject ����j
    }

    public override void OnClientAuthenticate()
    {
        // �ڑ������ HandshakeRequest �𑗂�
        var req = new HandshakeRequest { _gameId = gameId, _version = version };
        Debug.Log($"[AUTH/CLIENT] send handshake: {req._gameId} v{req._version}");
        NetworkClient.Send(req);
    }

    // --- �T�[�o�[�� HandshakeRequest ���󂯎�����Ƃ� ---
    void OnServerHandshakeRequest(NetworkConnectionToClient conn, HandshakeRequest req)
    {
        bool ok = (req._gameId == gameId && req._version == version);
        string msg = ok ? "OK" : $"Mismatch (srv:{gameId} {version} / cli:{req._gameId} {req._version})";

        // �N���C�A���g�։���
        conn.Send(new HandshakeResponse { _accepted = ok, _message = msg });

        if (ok)
        {
            Debug.Log($"[AUTH/SERVER] accept: {conn.connectionId} {msg}");
            ServerAccept(conn);   // <- Mirror�W��API
        }
        else
        {
            Debug.LogWarning($"[AUTH/SERVER] reject: {conn.connectionId} {msg}");
            ServerReject(conn);   // <- Mirror�W��API
        }
    }

    // --- �N���C�A���g�� HandshakeResponse ���󂯎�����Ƃ� ---
    void OnClientHandshakeResponse(HandshakeResponse res)
    {
        if (res._accepted)
        {
            Debug.Log($"[AUTH/CLIENT] accepted: {res._message}");
            ClientAccept();       // <- Mirror�W��API
        }
        else
        {
            Debug.LogWarning($"[AUTH/CLIENT] rejected: {res._message}");
            ClientReject();       // <- Mirror�W��API
        }
    }
}