using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveBlockScript : MonoBehaviour
{
    public float moveDistance = 5;//�ړ�����
    public float moveSpeed = 1;//�ړ����x

    private Vector3 originalPosition;//���̈ʒu
    private Vector3 targetPosition;//�ڕW�ʒu
    private bool movingForward = true;//�ړ�����

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + Vector3.right * moveDistance;//�E�����Ɉړ�
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerScript.isPose)
        {
            //�ړ��֐�
            MoveObject();
        }
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
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
