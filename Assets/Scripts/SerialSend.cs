using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialSend : MonoBehaviour
{
    //SerialHandler.c�̃N���X
    public SerialHandler serialHandler;
    public PlayerController playerController;

    private float prevH = 0f;


    void Update()
    {
        float h = playerController.HValue;

        // �� �����V���� h �ƑO��� h ���ς���Ă����瑗�M����
        if (Mathf.Abs(h - prevH) > 0.001f) // (�قړ����łȂ����)
        {
            // h �� -1f �ɕς�����Ƃ�
            if (h == -1f)
            {
                serialHandler.Write("1"); // angle=0�� �ɂ��閽��
                Debug.Log("[SerialSend] Wrote: 1 (turn left)");
            }
            // h �� +1f �ɕς�����Ƃ�
            else if (h == 1f)
            {
                serialHandler.Write("2"); // angle=180�� �ɂ��閽��
                Debug.Log("[SerialSend] Wrote: 2 (turn right)");
            }
            else
            {
                // h=0 �� ���̑��̏ꍇ�͑��M���Ȃ�
                Debug.Log("[SerialSend] H changed to " + h + " (no servo command)");
            }
        }

        // ����� h �������r�̂��߂ɋL��
        prevH = h;
    }
}
