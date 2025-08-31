using Mirror;
using UnityEngine;

public class ClientGameManager : MonoBehaviour
{
    public NetworkManager networkManager;

    void Start()
    {
        // サーバーを見つけて自動接続
        networkManager.StartClient();
    }

    public void CreateRoom()
    {
        if (!NetworkClient.isConnected)
        {
            networkManager.StartHost(); // 自分がホストになる
        }
    }

    public void JoinRoom(string address)
    {
        if (!NetworkClient.isConnected)
        {
            networkManager.networkAddress = address;
            networkManager.StartClient();
        }
    }
}
