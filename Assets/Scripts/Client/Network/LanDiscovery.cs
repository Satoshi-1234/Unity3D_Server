using Mirror;
using Mirror.Discovery;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

// Mirror の NetworkDiscoveryBase を継承
public class LanDiscovery : NetworkDiscoveryBase<ServerRequest, ServerResponse>
{
    [System.Serializable]
    public class ServerFoundEvent : UnityEvent<ServerResponse> { }
    public ServerFoundEvent OnServerFoundEvent = new ServerFoundEvent();

    // サーバーが広告する情報を返す
    protected override ServerResponse ProcessRequest(ServerRequest request, IPEndPoint endpoint)
    {
        return new ServerResponse
        {
            EndPoint = endpoint,
            uri = transport.ServerUri()
        };
    }

    // クライアントがサーバーを見つけたとき呼ばれる
    protected override void ProcessResponse(ServerResponse response, IPEndPoint endpoint)
    {
        response.EndPoint = endpoint; // IPを保持
        Debug.Log($"[LAN] サーバー検出: {endpoint.Address}:{endpoint.Port} / URI={response.uri}");
        OnServerFoundEvent.Invoke(response);
    }
}
