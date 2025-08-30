using Mirror;
using UnityEngine;

public class ClientGameManager : MonoBehaviour
{
    public NetworkManager networkManager;

    void Start()
    {
        // �T�[�o�[�������Ď����ڑ�
        networkManager.StartClient();
    }

    public void CreateRoom()
    {
        if (!NetworkClient.isConnected)
        {
            networkManager.StartHost(); // �������z�X�g�ɂȂ�
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
