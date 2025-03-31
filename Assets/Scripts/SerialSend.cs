using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialSend : MonoBehaviour
{
    //SerialHandler.cのクラス
    public SerialHandler serialHandler;
    public PlayerController playerController;

    private float prevH = 0f;


    void Update()
    {
        float h = playerController.HValue;

        // ★ もし新しい h と前回の h が変わっていたら送信する
        if (Mathf.Abs(h - prevH) > 0.001f) // (ほぼ同じでなければ)
        {
            // h が -1f に変わったとき
            if (h == -1f)
            {
                serialHandler.Write("1"); // angle=0° にする命令
                Debug.Log("[SerialSend] Wrote: 1 (turn left)");
            }
            // h が +1f に変わったとき
            else if (h == 1f)
            {
                serialHandler.Write("2"); // angle=180° にする命令
                Debug.Log("[SerialSend] Wrote: 2 (turn right)");
            }
            else
            {
                // h=0 や その他の場合は送信しない
                Debug.Log("[SerialSend] H changed to " + h + " (no servo command)");
            }
        }

        // 今回の h を次回比較のために記憶
        prevH = h;
    }
}
