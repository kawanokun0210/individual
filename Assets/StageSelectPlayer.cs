using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StageSelectPlayer : MonoBehaviour
{

    //�A�j���[�^�[�R���g���[���[
    public Animator animator;
    //�ړ��X�s�[�h
    float moveSpeed = 6.0f;
    private Vector3 move;
    //������Ă���{�^��
    bool isA = false;
    bool isD = false;
    //��]���x
    float rotateSpeed = 10.0f;

    //�ǂ̃X�e�[�W�ɍs����
    public static bool firstStage = false;
    public static bool secondStage = false;

    //�R���g���[���[���͂��󂯕t���邩
    public static bool isInput = true;

    // Start is called before the first frame update
    void Start()
    {
        //�߂��Ă�������͂��󂯕t����
        isInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        //���������̓��͂��擾���A���ꂼ��̈ړ����x��������B
        float Xvalue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float horizontalInput = Input.GetAxis("Horizontal");

        //���݂�X,Z�x�N�g���ɏ�̏����Ŏ擾�����l��n���B
        Vector3 movedir = new Vector3(Xvalue, 0, 0);

        //�ړ��x�N�g����ݒ�
        move = new Vector3(horizontalInput, 0, 0);

        //�ړ��֐�
        Move(horizontalInput, movedir);

    }

    //�ړ��֐�
    void Move(float horizontalInput, Vector3 movedir)
    {
        //D����������E�ֈړ�
        if (Input.GetKey(KeyCode.D) && !isA && isInput || horizontalInput > 0 && isInput)
        {
            //D�{�^����������Ă��邩
            isD = true;
            //���ݒn�ɏ�Ŏ擾�������l�𑫂��Ĉړ�����B
            transform.position += movedir;
            //�i�ޕ����Ɋ��炩�Ɍ����B
            transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
            //�A�j���[�V������ύX�B
            animator.SetBool("mode", true);
        }
        else
        {
            //����������ĂȂ���΃A�j���[�V������߂�
            animator.SetBool("mode", false);
            isD = false;
        }

        //A���������獶�ֈړ�
        if (Input.GetKey(KeyCode.A) && !isD && isInput || horizontalInput < 0 && isInput)
        {
            //D�{�^����������Ă��邩
            isA = true;
            //���ݒn�ɏ�Ŏ擾�������l�𑫂��Ĉړ�����B
            transform.position += movedir;
            //�i�ޕ����Ɋ��炩�Ɍ����B
            transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
            //�A�j���[�V������ύX�B
            animator.SetBool("mode", true);
        }
        else
        {
            isA = false;
        }
    }

    //���̂ɓ������Ă��鎞�͂��̃X�e�[�W�ɍs����
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "FirstStage")
        {
            firstStage = true;
        }

        if (other.gameObject.tag == "SecondStage")
        {
            secondStage = true;
        }

    }

    //���̂ɓ������Ă��Ȃ��Ƃ��̓X�e�[�W�ɔ�΂Ȃ��悤�ɂ���
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FirstStage")
        {
            firstStage = false;
        }

        if (other.gameObject.tag == "SecondStage")
        {
            secondStage = false;
        }

    }

}
