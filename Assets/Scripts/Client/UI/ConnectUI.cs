#if !UNITY_SERVER || !UNITY_EDITOR
using Mirror;
using UnityEngine;

public class ConnectUI : MonoBehaviour
{
//#if UNITY_EDITOR || DEVELOPMENT_BUILD
    // デバッグ用UI（エディタやデバッグビルドのみ表示）
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 200));
        if (GUILayout.Button("Start as Host")) NetworkManager.singleton.StartHost();
        if (GUILayout.Button("Start as Client")) NetworkManager.singleton.StartClient();
        GUILayout.EndArea();
    }
//#endif
}
#endif
