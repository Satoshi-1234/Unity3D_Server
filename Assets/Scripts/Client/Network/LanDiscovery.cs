using Mirror;
using Mirror.Discovery;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

// Mirror �� NetworkDiscoveryBase ���p��nse>
public class LanDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
{
    [System.Serializable]
    public class ServerFoundEvent : UnityEvent<DiscoveryResponse> { }
    public ServerFoundEvent OnServerFoundEvent = new ServerFoundEvent();

    protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint)
    {
        var nm = NetworkManager.singleton;
        var transport = nm.transport as kcp2k.KcpTransport;
        return new DiscoveryResponse
        {
            _uri = transport.ServerUri(),
            // endpoint�͒��ڕێ��ł��Ȃ��̂�string��
            _address = endpoint.Address.ToString(),
            _port = transport.Port,
            //_port = (nm.transport as kcp2k.KcpTransport)?.Port ?? 7777,
            // CustomNetworkManager �̎��ʏ��������ŗ��p
            _gameId = (nm.authenticator as CustomNetworkManager)?.gameId ?? "Unknown",
            _version = (nm.authenticator as CustomNetworkManager)?.version ?? "0.0.0",
            _playerCount = nm.numPlayers,
            _maxPlayers = nm.maxConnections,
            _serverName = System.Environment.MachineName // PC�����T�[�o�[���ɐݒ�
        };
    }

    protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint)
    {
        Debug.Log($"[LAN] �T�[�o�[���o: {response._serverName} ({response._gameId} v{response._version}) " +
                  $"Players {response._playerCount}/{response._maxPlayers}");
        OnServerFoundEvent.Invoke(response);
    }
}
