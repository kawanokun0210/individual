using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostScript : MonoBehaviour
{
    public float moveDistance = 5f;//�ړ�����
    public float moveSpeed = 2f;//�ړ����x

    private Vector3 originalPosition;//���̈ʒu
    private Vector3 targetPosition;//�ڕW�ʒu
    private bool movingForward = true;//�ړ�����

    //�A�j���[�^�[�R���g���[���[
    public Animator animator;

    private void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + Vector3.right * moveDistance; // �E�����Ɉړ�
    }

    private void Update()
    {
        //�ړ��֐�
        MoveObject();
    }

    private void MoveObject()
    {
        //���݂̃^�[�Q�b�g�ʒu�Ɋ�Â��ăI�u�W�F�N�g���ړ�
        if (movingForward)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                //���]���Ĉړ�������؂�ւ�
                movingForward = false;
                targetPosition = originalPosition;//�߂�ʒu
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);//X���Ŕ��]
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                //���]���Ĉړ�������؂�ւ�
                movingForward = true;
                targetPosition = originalPosition + Vector3.right * moveDistance;//�V�����ڕW�ʒu
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);//X���Ŕ��]
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }

}
