using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;  // �A�j���[�^�[�R���|�[�l���g�ւ̎Q��
    private Rigidbody rb;  // Rigidbody�R���|�[�l���g�ւ̎Q��

    public float moveSpeed = 0.1f;  // �ړ����x
    public float rotateSpeed = 2;  // ��]���x
    public float jump = 1;  // �W�����v��

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
        // ���������̓��͂��擾
        float h = Input.GetAxis("Horizontal");
        // ���������̓��͂��擾
        float v = Input.GetAxis("Vertical");
        // �X�y�[�X�L�[�̓��͂��擾
        bool jumpRequest = Input.GetKey(KeyCode.Space);

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
