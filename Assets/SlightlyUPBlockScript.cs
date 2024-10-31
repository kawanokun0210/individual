using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlightlyUPBlockScript : MonoBehaviour
{
    public float fallDistance = 3f;//�u���b�N�������鋗��
    public float fallSpeed = 2f;//�u���b�N�������鑬�x
    public float returnSpeed = 1f;//�u���b�N���߂鑬�x
    private Vector3 initialPosition;
    private bool playerOnBlock = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (playerOnBlock)
        {
            //�v���C���[���u���b�N�̏�ɂ���ꍇ�A�u���b�N�𗎂Ƃ�
            Vector3 targetPosition = initialPosition + new Vector3(0, fallDistance, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, fallSpeed * Time.deltaTime);
        }
        else
        {
            //�v���C���[���u���b�N�̏�ɂ��Ȃ��ꍇ�A�u���b�N�����̈ʒu�ɖ߂�
            transform.position = Vector3.Lerp(transform.position, initialPosition, returnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //���ׂĂ̏ՓːڐG�_���擾
        foreach (ContactPoint contact in collision.contacts)
        {
            //�ڐG�_�̖@���x�N�g�����I�u�W�F�N�g�̏�����ɋ߂��ꍇ�A�ォ��̏Փ˂ƌ��Ȃ�
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                if (collision.gameObject.tag == "Player")
                {
                    playerOnBlock = true;
                    collision.transform.SetParent(transform);
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnBlock = true;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnBlock = false;
            collision.transform.SetParent(null);
        }
    }
}

