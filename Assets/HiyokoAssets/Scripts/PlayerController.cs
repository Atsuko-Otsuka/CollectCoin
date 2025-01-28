using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;  // アニメーターコンポーネントへの参照
    private Rigidbody rb;  // Rigidbodyコンポーネントへの参照

    public float moveSpeed = 0.1f;  // 移動速度
    public float rotateSpeed = 2;  // 回転速度
    public float jump = 1;  // ジャンプ力

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
        // 水平方向の入力を取得
        float h = Input.GetAxis("Horizontal");
        // 垂直方向の入力を取得
        float v = Input.GetAxis("Vertical");
        // スペースキーの入力を取得
        bool jumpRequest = Input.GetKey(KeyCode.Space);

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
