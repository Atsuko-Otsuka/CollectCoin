using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;  // アニメーターコンポーネントへの参照
    private Rigidbody rb;  // Rigidbodyコンポーネントへの参照

    public SerialReceive serialReceive;
    //public SerialSend serialSend;

    public float moveSpeed = 0.3f;  // 移動速度
    public float rotateSpeed = 2;  // 回転速度
    public float jump = 1;  // ジャンプ力
    //public float jumpThreshold = 400f;
    public float HValue { get; private set; }



    // Startは最初のフレームの更新前に呼び出されます
    void Start()
    {
        // アニメーターコンポーネントを取得
        animator = GetComponent<Animator>();
        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();
    }

    // Updateはフレームごとに一度呼び出されます
    void Update()
    {
        Vector3 accel = serialReceive.currentAccel;

        float ax = accel.x;
        float ay = accel.y;
        float az = accel.z;

        //// 水平方向の入力を取得
        //float h = Input.GetAxis("Horizontal");
        //// 垂直方向の入力を取得
        //float v = Input.GetAxis("Vertical");
        //// スペースキーの入力を取得
        //bool jumpRequest = Input.GetKey(KeyCode.Space);

        //Debug.LogFormat("Input h: {0}, v: {1}, j: {2}", h, v, jumpRequest);

        float h = 0f;
        if (ay > 100f)
            h = -1f;
        else if (ay < -100f)
            h = 1f;
        HValue = h;

        // 垂直方向 (v) の入力を決定
        // y 軸が +100 を超えたら v=1 (前進), -100 を下回ったら v=-1 (後退), それ以外は 0
        float v = 0f;
        if (ax > 50f)
            v = 1f;
        else if (ax < -50f)
            v = -1f;

        bool jumpRequest = false;
        if (az > 500f)
            jumpRequest = true;

        //if (serialHandler != null)
        //{
        //    if (h == -1f) 
        //    {
        //        serialHandler.Write("1"); // Arduino側で angle=0° とする
        //    }
        //    else if (h == 1f) // hがほぼ+1
        //    {
        //        serialHandler.Write("2"); // Arduino側で angle=180° とする
        //    }
        //}

        Debug.LogFormat("Current Accel x: {0}, y: {1}, z: {2}", accel.x, accel.y, accel.z);

        // 入力に基づいてキャラクターを回転させる
        transform.Rotate(0, h * (rotateSpeed / 10), 0);

        // 入力に基づいてキャラクターの位置を更新する
        Vector3 newPos = rb.position + (transform.forward * v) * (moveSpeed / 100);
        rb.MovePosition(newPos);

        // 入力に基づいてアニメーションの速度を設定する
        if (Math.Abs(v) > 0)
        {
            animator.SetFloat("Velocity", 2);
        }
        else
        {
            animator.SetFloat("Velocity", 0);
        }

        // ジャンプの入力がある場合、アニメーションとジャンプの力を設定する
        if (jumpRequest)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (!info.IsName("Jump"))
            {
                animator.SetBool("Jump", true);
                rb.AddForce(Vector3.up * (jump / 10), ForceMode.VelocityChange);
            }
        }
        else
        {
            animator.SetBool("Jump", false);
        }
    }

}
