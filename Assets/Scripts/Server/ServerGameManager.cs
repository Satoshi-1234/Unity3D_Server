#if UNITY_SERVER || UNITY_EDITOR
using Mirror;
using UnityEngine;

public class ServerGameManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // �v���C���[�����iCube�j
        if (playerPrefab == null) Debug.Log("Prefab���Ȃ��I");
        else Debug.Log($"{playerPrefab.gameObject.name}������I");
        GameObject player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    [Server]
    public void StartGame()
    {
        Debug.Log("Game Started!");
        // �Q�[���J�n�����i�V�[���J�ڂ⏉�����Ȃǁj
    }
}
#endif