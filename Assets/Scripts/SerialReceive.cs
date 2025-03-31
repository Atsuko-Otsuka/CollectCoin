using UnityEngine;

public class SerialReceive : MonoBehaviour
{
    // Hierarchy�œ���GameObject�������͕ʂ�GameObject��
    // SerialHandler���A�^�b�`���Ă����AInspector�ŕR�Â���
    public SerialHandler serialHandler;
    public Vector3 currentAccel = Vector3.zero;

    void Start()
    {
        // ��M�C�x���g�Ƀn���h����o�^
        if (serialHandler != null)
        {
            serialHandler.OnDataReceived += OnDataReceived;
        }
        else
        {
            Debug.LogError("[SerialReceive] SerialHandler not assigned in Inspector!");
        }
    }

    // �f�[�^��M���ɌĂ΂�郁�\�b�h
    void OnDataReceived(string message)
    {
        string[] values = message.Split(',');
        if (values.Length < 3) return;

        float ax, ay, az;
        if (float.TryParse(values[0], out ax) &&
            float.TryParse(values[1], out ay) &&
            float.TryParse(values[2], out az))
        {
            currentAccel = new Vector3(ax, ay, az);
        }

        // ���O�ɕ\�����邾��
        Debug.Log("[SerialReceive] Received: " + currentAccel);

        // �����ŕK�v�ɉ����ĕ�����p�[�X��AUnity�I�u�W�F�N�g���쓙���s��
    }
}
