using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;  // �A�j���[�^�[�R���|�[�l���g�ւ̎Q��
    private Rigidbody rb;  // Rigidbody�R���|�[�l���g�ւ̎Q��

    public SerialReceive serialReceive;
    //public SerialSend serialSend;

    public float moveSpeed = 0.3f;  // �ړ����x
    public float rotateSpeed = 2;  // ��]���x
    public float jump = 1;  // �W�����v��
    //public float jumpThreshold = 400f;
    public float HValue { get; private set; }



    // Start�͍ŏ��̃t���[���̍X�V�O�ɌĂяo����܂�
    void Start()
    {
        // �A�j���[�^�[�R���|�[�l���g���擾
        animator = GetComponent<Animator>();
        // Rigidbody�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody>();
    }

    // Update�̓t���[�����ƂɈ�x�Ăяo����܂�
    void Update()
    {
        Vector3 accel = serialReceive.currentAccel;

        float ax = accel.x;
        float ay = accel.y;
        float az = accel.z;

        //// ���������̓��͂��擾
        //float h = Input.GetAxis("Horizontal");
        //// ���������̓��͂��擾
        //float v = Input.GetAxis("Vertical");
        //// �X�y�[�X�L�[�̓��͂��擾
        //bool jumpRequest = Input.GetKey(KeyCode.Space);

        //Debug.LogFormat("Input h: {0}, v: {1}, j: {2}", h, v, jumpRequest);

        float h = 0f;
        if (ay > 100f)
            h = -1f;
        else if (ay < -100f)
            h = 1f;
        HValue = h;

        // �������� (v) �̓��͂�����
        // y ���� +100 �𒴂����� v=1 (�O�i), -100 ����������� v=-1 (���), ����ȊO�� 0
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
        //        serialHandler.Write("1"); // Arduino���� angle=0�� �Ƃ���
        //    }
        //    else if (h == 1f) // h���ق�+1
        //    {
        //        serialHandler.Write("2"); // Arduino���� angle=180�� �Ƃ���
        //    }
        //}

        Debug.LogFormat("Current Accel x: {0}, y: {1}, z: {2}", accel.x, accel.y, accel.z);

        // ���͂Ɋ�Â��ăL�����N�^�[����]������
        transform.Rotate(0, h * (rotateSpeed / 10), 0);

        // ���͂Ɋ�Â��ăL�����N�^�[�̈ʒu���X�V����
        Vector3 newPos = rb.position + (transform.forward * v) * (moveSpeed / 100);
        rb.MovePosition(newPos);

        // ���͂Ɋ�Â��ăA�j���[�V�����̑��x��ݒ肷��
        if (Math.Abs(v) > 0)
        {
            animator.SetFloat("Velocity", 2);
        }
        else
        {
            animator.SetFloat("Velocity", 0);
        }

        // �W�����v�̓��͂�����ꍇ�A�A�j���[�V�����ƃW�����v�̗͂�ݒ肷��
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
