#if UNITY_SERVER || UNITY_EDITOR
using Mirror;
using UnityEngine;

public class ServerGameManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // プレイヤー生成（Cube）
        if (playerPrefab == null) Debug.Log("Prefabがない！");
        else Debug.Log($"{playerPrefab.gameObject.name}がある！");
        GameObject player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    [Server]
    public void StartGame()
    {
        Debug.Log("Game Started!");
        // ゲーム開始処理（シーン遷移や初期化など）
    }
}
#endif