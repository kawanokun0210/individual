using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBlockScript : MonoBehaviour
{
    public float fallDelay = 3.0f;//�u���b�N���������J�n����܂ł̎���
    public float fallSpeed = 5.0f;//�u���b�N�̗������x

    private bool isPlayerOnBlock = false;
    private bool isFalling = false;
    private float timeOnBlock = 0f;

    //�ēx�o�������邽�߂̐錾
    private bool isResporn = false;
    private float respornTimer = 0;//���Z�b�g�܂ł̃J�E���g
    private float respornTime = 10.0f;//���Z�b�g�܂ł̎���
    private Vector3 initialPosition;
    private Vector3 initialScale;
    public float respawnScaleSpeed = 2.0f;//�g�呬�x


    // Start is called before the first frame update
    void Start()
    {
        //�ŏ��̃|�W�V������ۑ����Ă���
        initialPosition = transform.position;
        //�ŏ��̃X�P�[����ۑ����Ă���
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[���u���b�N�̏�ɂ���ꍇ�o�ߎ��Ԃ𑝉�������
        if (isPlayerOnBlock && !isFalling)
        {
            timeOnBlock += Time.deltaTime;

            //�w�莞�Ԃ��o�߂����痎�����J�n
            if (timeOnBlock >= fallDelay)
            {
                StartCoroutine(Fall());
            }
        }

        //�u���b�N���������Ă�����
        if (isFalling)
        {
            //���Ɉړ�������
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            //���X�|�[���܂ł̎��Ԃ�}��
            respornTimer += Time.deltaTime;
        }

        //�w��̎��ԂɂȂ�����
        if(respornTimer >= respornTime)
        {
            //���̈ʒu�ɖ߂�
            transform.position = initialPosition;
            //�g�傳���Ȃ���o��
            StartCoroutine(Respawn());
            //�^�C�}�[�����Z�b�g����
            respornTimer = 0;
            //�������ĂȂ����Ƃɂ���
            isFalling = false;
        }

    }

    //�v���C���[���u���b�N�̏�ɏ������Ă΂��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBlock = true;
            collision.transform.SetParent(transform);
        }
    }

    //�v���C���[���u���b�N���痣�ꂽ��Ă΂��
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBlock = false;
            timeOnBlock = 0f;//���Ԃ����Z�b�g
            collision.transform.SetParent(null);
        }
    }

    //���������̃R���[�`��
    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(1.0f);//�����J�n�܂ł̒Z���҂�����
    }

    //�u���b�N�����炩�Ɋg�債�Ȃ���ďo��������R���[�`��
    private IEnumerator Respawn()
    {
        transform.localScale = Vector3.zero;
        float elapsedTime = 0f;

        while (transform.localScale.x < initialScale.x)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, elapsedTime * respawnScaleSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale;//�ŏI�I�ɃX�P�[���𐳊m�ɖ߂�
    }
}
