using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{
    public float fallDistance = 10.0f;//��������
    public float fallSpeed = 0.5f;
    public float riseSpeed = 0.5f;//�㏸���x
    private playerScript playerController;

    public float shakeMagnitude = 0.05f;//�k���镝
    public float shakeFrequency = 10.0f;//�k���鑬�x
    private Vector3 shakeOffset;

    private Vector3 initialPosition;
    private bool isFalling = false;
    private bool isRise = false;
    private int fallTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�̃I�u�W�F�N�g��T���A���̃X�N���v�g���擾
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<playerScript>();
        }

        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFalling && IsPlayerBelow() && !isRise)
        {
            ShakeBlock();
            fallTimer++;
        }

        if(fallTimer >= 120)
        {
            StartCoroutine(Fall());
        }

    }

    private bool IsPlayerBelow()
    {
        //�΂ƃv���C���[�̐����������v�Z
        float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(playerController.transform.position.x, 0, playerController.transform.position.z));
        return distance < 1.0f && playerController.transform.position.y < transform.position.y;
    }

    //�΂����ɗ��Ƃ��R���[�`��
    private IEnumerator Fall()
    {
        isFalling = true;
       
        while (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        isFalling = false;//�����I��
    }

    //�u���b�N��k�������鏈��
    private void ShakeBlock()
    {
        // �k���̃I�t�Z�b�g���v�Z
        shakeOffset = Random.insideUnitSphere * shakeMagnitude;
        shakeOffset.z = 0;//Z�����̗h���h���i2D�Q�[�������j

        //�����ڂ̗h��Ƃ��Ĉʒu�ύX��K�p
        Vector3 newPosition = initialPosition + shakeOffset;
        transform.position = newPosition;
    }

    //�΂��������㏸������R���[�`��
    private IEnumerator Rise()
    {
        isRise = true;

        while (transform.position.y < initialPosition.y)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;//�ŏI�ʒu���Z�b�g
        isRise = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�u���b�N�ƏՓ˂�����
        if (collision.gameObject.tag == "Block")
        {
            StopCoroutine(Fall());//�������~
            isFalling = false;
            fallTimer = 0;
            StartCoroutine(Rise());//�㏸���J�n
        }
    }

}
