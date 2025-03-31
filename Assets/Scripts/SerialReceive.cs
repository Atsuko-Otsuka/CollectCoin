using UnityEngine;

public class SerialReceive : MonoBehaviour
{
    // Hierarchyで同じGameObjectもしくは別のGameObjectに
    // SerialHandlerをアタッチしておき、Inspectorで紐づける
    public SerialHandler serialHandler;
    public Vector3 currentAccel = Vector3.zero;

    void Start()
    {
        // 受信イベントにハンドラを登録
        if (serialHandler != null)
        {
            serialHandler.OnDataReceived += OnDataReceived;
        }
        else
        {
            Debug.LogError("[SerialReceive] SerialHandler not assigned in Inspector!");
        }
    }

    // データ受信時に呼ばれるメソッド
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

        // ログに表示するだけ
        Debug.Log("[SerialReceive] Received: " + currentAccel);

        // ここで必要に応じて文字列パースや、Unityオブジェクト操作等を行う
    }
}
