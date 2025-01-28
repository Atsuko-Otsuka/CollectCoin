using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;  // �v���C���[�̈ʒu����ێ�����
    private Vector3 offset;  // �v���C���[�ƃJ�����Ƃ̏����ʒu�֌W��ێ�����

    // �Q�[�����n�܂�Ƃ��Ɉ�x�����Ă΂�郁�\�b�h
    void Start()
    {
        // target�ɉ����ݒ肳��Ă��Ȃ����Player�^�O�����I�u�W�F�N�g���������Đݒ�
        if (!target)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go)
            {
                target = go.transform;
            }
        }
        // �J�����ƃv���C���[�̏����ʒu�֌W�i�I�t�Z�b�g�j���L�^����
        offset = transform.position - target.position;
    }

    // �e�t���[���̍Ō�ɌĂ΂�郁�\�b�h
    void LateUpdate()
    {
        // �v���C���[�̈ʒu�ɃI�t�Z�b�g�������邱�ƂŁA�J�������v���C���[��Ǐ]����
        transform.position = target.position + offset;
    }
}
