using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // 自分のオブジェクトに権限があるときだけ入力処理
        if (!isLocalPlayer) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        transform.Translate(move * speed * Time.deltaTime);
    }
}