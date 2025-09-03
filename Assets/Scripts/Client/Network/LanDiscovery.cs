using Mirror;
using Mirror.Discovery;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

// Mirror �� NetworkDiscoveryBase ���p��
public class LanDiscovery : NetworkDiscoveryBase<ServerRequest, ServerResponse>
{
    [System.Serializable]
    public class ServerFoundEvent : UnityEvent<ServerResponse> { }
    public ServerFoundEvent OnServerFoundEvent = new ServerFoundEvent();

    // �T�[�o�[���L���������Ԃ�
    protected override ServerResponse ProcessRequest(ServerRequest request, IPEndPoint endpoint)
    {
        return new ServerResponse
        {
            EndPoint = endpoint,
            uri = transport.ServerUri()
        };
    }

    // �N���C�A���g���T�[�o�[���������Ƃ��Ă΂��
    protected override void ProcessResponse(ServerResponse response, IPEndPoint endpoint)
    {
        response.EndPoint = endpoint; // IP��ێ�
        Debug.Log($"[LAN] �T�[�o�[���o: {endpoint.Address}:{endpoint.Port} / URI={response.uri}");
        OnServerFoundEvent.Invoke(response);
    }
}
