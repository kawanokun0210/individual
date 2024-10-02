using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour
{
    //�V�[���̖��O
    public string nextSceneName;
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
    //�W�����v
    Rigidbody rb;
    float jumpPower = 8.2f;
    bool isJump = false;
    bool isHitBlock = true;
    //�ҋ@���[�V����
    int waitTimer = 0;
    //�̗͌n
    bool isHit = false;
    int coolTime = 0;
    int remainingHP = 3;
    public GameObject heartPrefab;
    private GameObject[] hearts;
    public Transform cameraTransform;
    //�U��
    public bool isAttack = false;
    //����
    public bool isFall = false;
    //�S�[��
    public bool isGoal = false;
    //��
    bool isHeel = false;

    //Audio�n�̐錾
    public AudioSource healSE;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody���֘A�t����
        rb = GetComponent<Rigidbody>();
        //�n�[�g�p
        hearts = new GameObject[remainingHP];
        CreateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        //���������̓��͂��擾���A���ꂼ��̈ړ����x��������B
        float Xvalue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float horizontalInput = Input.GetAxis("Horizontal");

        //���݂�X,Z�x�N�g���ɏ�̏����Ŏ擾�����l��n���B
        Vector3 movedir = new Vector3(Xvalue, 0, 0);

        // �J�����̈ʒu�Ɋ�Â��ăn�[�g���X�V
        Vector3 offset = new Vector3(-9, 4.5f, 10); // �J��������̃I�t�Z�b�g

        //�ړ��x�N�g����ݒ�
        move = new Vector3(horizontalInput, 0, 0);

        //�ړ�����
        Move(movedir, horizontalInput);

        //�W�����v�̏���
        Jump();

        //�U���֐�
        Attack();

        //�n�[�g�̃|�W�V�����֐�
        UpdateHeartPositions(offset);

        //�ҋ@�A�j���[�V�����֐�
        StartIdleAnimation();

        //�G�Ɠ����������̃_���[�W����
        ApplyDamageFromEnemy();

        //�񕜂̊֐�
        HeelAction();

        //�����̊֐�
        Fall();

    }

    private void OnCollisionEnter(Collision collision)
    {
        //�u���b�N�Ɠ������Ă���Ƃ�
        if (collision.gameObject.tag == "Block")
        {
            //�d�͖���
            rb.isKinematic = false;
            //�W�����v�̔��f(false�͂��Ă��Ȃ�)
            isJump = false;
            isHitBlock = true;
            //�A�j���[�V������ύX�B
            animator.SetBool("jump", false);
            //�A�j���[�V������ύX�B
            animator.SetBool("fall", false);
            //�A�j���[�V������ύX�B
            animator.SetBool("jumpping", false);
            //�A�j���[�V������ύX�B
            animator.SetBool("landing", false);
        }
        else
        {
            isHitBlock = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�v���C���[�Ɠ���������
        MashroomScript mashroom = other.gameObject.GetComponent<MashroomScript>();
        CactusScript cactus = other.GetComponentInParent<CactusScript>();

        //�L�m�R�ɓ���������
        if (!isAttack && mashroom != null && !mashroom.isDead)
        {
            if (other.gameObject.tag == "Mashroom")
            {
                //���������u�Ԃ�true�ɂ���
                isHit = true;
            }
        }

        //�T�{�e���ɓ���������
        if (!isAttack && cactus != null && !cactus.isDead)
        {
            if (other.gameObject.CompareTag("Cactus"))
            {
                //���������u�Ԃ�true�ɂ���
                isHit = true;
            }
        }

        //�������ɓ���������
        if (other.gameObject.tag == "Gost")
        {
            isHit = true;
        }

        //�S�[��������
        if (other.gameObject.tag == "Goal")
        {
            isGoal = true;
        }

        //�����񕜃A�C�e���ɓ���������
        if (other.gameObject.tag == "Heel")
        {
            isHeel = true;
        }

    }

    //�ŏ��̃n�[�g�����֐�
    void CreateHearts()
    {
        for (int i = 0; i < remainingHP; i++)
        {
            GameObject heart = Instantiate(heartPrefab);
            hearts[i] = heart;
        }
    }

    //�n�[�g�̃|�W�V�������Œ肷��֐�
    void UpdateHeartPositions(Vector3 offset)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            Vector3 position = cameraTransform.position + offset + new Vector3(i * 1.5f, 0, 0);
            hearts[i].transform.position = position;
        }
    }

    //�ړ��֐�
    void Move(Vector3 movedir, float horizontalInput)
    {
        //D����������E�ֈړ�
        if (Input.GetKey(KeyCode.D) && !isA || horizontalInput > 0)
        {
            //D�{�^����������Ă��邩
            isD = true;
            isAttack = false;
            //���ݒn�ɏ�Ŏ擾�������l�𑫂��Ĉړ�����B
            transform.position += movedir;
            //�i�ޕ����Ɋ��炩�Ɍ����B
            transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
            //�A�j���[�V������ύX�B
            animator.SetBool("mode", true);
            //�A�j���[�V������ύX�B
            animator.SetBool("attack", false);
        }
        else
        {
            //����������ĂȂ���΃A�j���[�V������߂�
            animator.SetBool("mode", false);
            isD = false;
        }

        //A���������獶�ֈړ�
        if (Input.GetKey(KeyCode.A) && !isD || horizontalInput < 0)
        {
            //D�{�^����������Ă��邩
            isA = true;
            isAttack = false;
            //���ݒn�ɏ�Ŏ擾�������l�𑫂��Ĉړ�����B
            transform.position += movedir;
            //�i�ޕ����Ɋ��炩�Ɍ����B
            transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
            //�A�j���[�V������ύX�B
            animator.SetBool("mode", true);
            //�A�j���[�V������ύX�B
            animator.SetBool("attack", false);
        }
        else
        {
            isA = false;
        }
    }

    //�W�����v�̊֐�
    void Jump()
    {
        //�X�y�[�X��������W�����v
        if (Input.GetKeyDown(KeyCode.Space) && !isJump || Input.GetButtonDown("Fire1") && !isJump)
        {
            //��ɃW�����v������
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            //�W�����v���Ă��邩�𔻒f(true�̓W�����v���Ă���Ɣ��f)
            isJump = true;
            isAttack = false;
            //�A�j���[�V������ύX�B
            animator.SetBool("jump", true);
            //�A�j���[�V������ύX�B
            animator.SetBool("attack", false);
        }

        if (isJump && rb.velocity.y > 0)
        {
            //�A�j���[�V������ύX�B
            animator.SetBool("jumpping", true);
        }
        else if (isJump && rb.velocity.y < 0)
        {
            Debug.Log("1");
            //�A�j���[�V������ύX�B
            animator.SetBool("fall", true);
        }

    }

    //�U���̊֐�
    void Attack()
    {
        //�G���^�[��������B�{�^������������U��
        if (Input.GetKeyDown(KeyCode.Return) && !isJump && !isAttack && !isD && !isA || Input.GetButtonDown("Fire2") && !isJump && !isAttack && !isD && !isA)
        {
            //�A�j���[�V������ύX�B
            animator.SetBool("attack", true);
            isAttack = true;
        }

        //�U���A�j���[�V�����̏�Ԃ��Ď�
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Attack���A�j���[�V�����̃X�e�[�g���ƈ�v����ꍇ
        if (isAttack && stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("attack", false);
            isAttack = false;
        }
    }

    //�ҋ@�A�j���[�V�����p�̏���
    private void StartIdleAnimation()
    {
        //�ҋ@���[�V�����Ɉڍs�����邩�ǂ���
        if (!isA && !isD && !isJump)
        {
            //�ǂ̂��炢���u���Ă��邩���`�F�b�N
            waitTimer++;
            //1�����u���Ă�����A�j���[�V����������
            if (waitTimer >= 3600)
            {
                //�A�j���[�V������ύX�B
                animator.SetBool("waitMotion", true);
            }
        }
        else
        {
            //�A�j���[�V������ύX�B
            animator.SetBool("waitMotion", false);
            //���u���Ԃ����Z�b�g
            waitTimer = 0;
        }
    }

    //�񕜂̊֐�
    private void HeelAction()
    {
        //����HP��3�ȉ����������
        if (isHeel && remainingHP <= 3)
        {
            remainingHP++;
            isHeel = false;
            healSE.Play();

            //�n�[�g�̕\�����𑝂₷
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < remainingHP)
                {
                    hearts[i].SetActive(true);
                    hearts[i].transform.rotation = hearts[0].transform.rotation;
                }
                else
                {
                    hearts[i].SetActive(false);
                }
            }

        }
        else if (isHeel && remainingHP > 3)
        {
            remainingHP = 3;
            isHeel = false;
        }
    }

    //�G�Ɠ����������ɑ̗͂����炷����
    private void ApplyDamageFromEnemy()
    {
        //�G�ɂ�����c��̗͂�0����Ȃ���
        if (isHit && remainingHP > 0 && coolTime == 0)
        {
            //�̗͂����炷
            remainingHP--;
            //�n�[�g�̕\���������炷
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < remainingHP)
                {
                    hearts[i].SetActive(true);
                }
                else
                {
                    hearts[i].SetActive(false);
                }
            }
        }
        else if (isHit && remainingHP == 0)
        {
            isFall = true;
        }

        //�̗͂�0����Ȃ����
        if (isHit && remainingHP > 0)
        {
            //�N�[���^�C�����Ԃ𑪂�
            coolTime++;
        }

        if (coolTime == 60)
        {
            //���G������
            isHit = false;
            //�N�[���^�C�������Z�b�g
            coolTime = 0;
        }
    }

    //�����̊֐�
    void Fall()
    {
        //���̈ʒu�������ɍs���Ύ��S
        if (transform.position.y < -15.0f)
        {
            isFall = true;
        }

        //�������Ă���Ƃ��ɗ����̃A�j���[�V�����ɐ؂�ւ���
        if (!isHitBlock && rb.velocity.y < 0)
        {
            //�A�j���[�V������ύX�B
            animator.SetBool("fall", true);
        }
        else
        {
            //�A�j���[�V������ύX�B
            animator.SetBool("fall", false);
        }


    }

}
