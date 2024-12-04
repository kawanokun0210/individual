using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{
    public float fallDistance = 10.0f;//��������
    public float fallSpeed = 2.0f;
    public float riseSpeed = 0.5f;//�㏸���x
    private playerScript playerController;

    public float shakeMagnitude = 0.05f;//�k���镝
    public float shakeFrequency = 10.0f;//�k���鑬�x
    private Vector3 shakeOffset;

    private Vector3 initialPosition;
    private bool isFalling = false;
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
        if(!isFalling && IsPlayerBelow())
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
        float elapsedTime = 0f;
        Vector3 targetPosition = initialPosition + Vector3.down * fallDistance;

        while (isFalling == true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fallSpeed;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;//�ŏI�ʒu���Z�b�g
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
        float elapsedTime = 0f;
        Vector3 currentPosition = transform.position;

        while (transform.position != initialPosition)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / riseSpeed;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(currentPosition, initialPosition, t);
            yield return null;
        }

        transform.position = initialPosition;//�ŏI�ʒu���Z�b�g
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
