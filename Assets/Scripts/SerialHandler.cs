using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialHandler : MonoBehaviour
{
    // 受信イベント用のデリゲート定義
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    // ここを自分の環境に合わせて変更
    // 例: "COM3", "COM5", "/dev/ttyUSB0", etc.
    public string portName = "COM3";
    public int baudRate = 115200;

    private SerialPort serialPort_;
    private Thread readThread_;
    private bool isRunning_ = false;

    // 受信データを一時保存するバッファ
    private string message_;
    private bool isNewMessageReceived_ = false;

    void Awake()
    {
        Open();
    }

    void Update()
    {
        // 新しいメッセージがあるなら、イベントを呼び出す
        if (isNewMessageReceived_)
        {
            OnDataReceived?.Invoke(message_);
            isNewMessageReceived_ = false;
        }
    }

    void OnDestroy()
    {
        Close();
    }

    /// <summary>
    /// シリアルポートを開く処理
    /// </summary>
    private void Open()
    {
        // 利用可能なポートをログ表示 (デバッグ用)
        foreach (string port in SerialPort.GetPortNames())
        {
            Debug.Log("[SerialHandler] Found port: " + port);
        }

        Debug.Log("[SerialHandler] Open() called for port: " + portName);

        // シリアルポートの設定
        serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);

        // Arduino Uno は "\r\n" が普通
        // Pico (MicroPython) は "\n" だけかもしれないので要確認
        serialPort_.NewLine = "\r\n";

        // タイムアウト設定 (ミリ秒)
        serialPort_.ReadTimeout = 1000;
        serialPort_.WriteTimeout = 500;

        // ※PicoなどCDCデバイスはDTRが必要な場合あり
        serialPort_.DtrEnable = true;

        try
        {
            // ポートオープン
            serialPort_.Open();
            Debug.Log("[SerialHandler] Port opened successfully: " + portName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("[SerialHandler] Port open error: " + e.Message);
            return;
        }

        // 受信スレッド開始
        isRunning_ = true;
        readThread_ = new Thread(Read);
        readThread_.Start();
    }

    /// <summary>
    /// シリアルポートを閉じる処理
    /// </summary>
    private void Close()
    {
        isRunning_ = false;

        if (readThread_ != null && readThread_.IsAlive)
        {
            readThread_.Join(); // スレッド終了待ち
        }

        if (serialPort_ != null && serialPort_.IsOpen)
        {
            serialPort_.Close();
            serialPort_.Dispose();
        }
    }

    /// <summary>
    /// 受信スレッドの実体
    /// </summary>
    private void Read()
    {
        Debug.Log("[SerialHandler] Read thread started");

        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
        {
            try
            {
                // 1行分受信 (改行コードが来るまで待機)
                string line = serialPort_.ReadLine();

                // 受信データを変数に格納→Update() で OnDataReceived を呼ぶ
                message_ = line;
                isNewMessageReceived_ = true;

                // デバッグ用ログ
                Debug.Log("[SerialHandler] Received line: " + line);
            }
            catch (System.TimeoutException)
            {
                // データ来ないままタイムアウトしたらスルー
                // （特に処理しなくてOK）
            }
            catch (System.Exception ex)
            {
                // その他例外発生時はログ表示
                Debug.LogWarning("[SerialHandler] Read error: " + ex.Message);
            }
        }

        Debug.Log("[SerialHandler] Read thread exited");
    }

    /// <summary>
    /// シリアルポートへの書き込み関数
    /// </summary>
    public void Write(string msg)
    {
        if (serialPort_ != null && serialPort_.IsOpen)
        {
            try
            {
                serialPort_.Write(msg);
                Debug.Log("[SerialHandler] Wrote: " + msg);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("[SerialHandler] Write error: " + ex.Message);
            }
        }
    }
}
