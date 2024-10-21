using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    private playerScript playerController;
    public float moveDuration = 1.0f; // �ړ��ɂ����鎞��
    public static bool isMoving = false; // �ړ������ǂ����̃t���O
    private Vector3 startPosition; // �ړ��J�n�ʒu
    private Vector3 targetPosition; // �ړ��ڕW�ʒu
    private float moveTime = 0; // �o�ߎ���

    private BoxCollider boxCollider; // BoxCollider�̎Q��

    private void Start()
    {
        // �v���C���[�̃I�u�W�F�N�g��T���A���̃X�N���v�g���擾
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<playerScript>();
        }

        //BoxCollider���擾
        boxCollider = GetComponent<BoxCollider>();

    }

    void Update()
    {
        // X�L�[�������ꂽ�Ƃ��A�ړ����J�n
        if (Input.GetKeyDown(KeyCode.X) && !isMoving && playerScript.isHitPipe)
        {
            startPosition = playerController.transform.position;
            targetPosition = transform.position + new Vector3(0, -1, 0);
            isMoving = true;
            moveTime = 0;
            boxCollider.enabled = !boxCollider.enabled;
        }

        // �ړ����̏ꍇ�ALerp�ňړ�
        if (isMoving)
        {
            moveTime += Time.deltaTime;
            float t = moveTime / moveDuration;
            playerController.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // �ړ�������������t���O�����Z�b�g
            if (t >= 1.0f)
            {
                isMoving = false;
            }
        }
    }
}
