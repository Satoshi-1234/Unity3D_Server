#if !UNITY_SERVER || !UNITY_EDITOR
using Mirror;
using UnityEngine;

public class ConnectUI : MonoBehaviour
{
//#if UNITY_EDITOR || DEVELOPMENT_BUILD
    // �f�o�b�O�pUI�i�G�f�B�^��f�o�b�O�r���h�̂ݕ\���j
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
