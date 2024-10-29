using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

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

    //���C�p�̐錾
    float rayDistance = 0.5f;
    private bool isBlock = true;

    //�L�[���͂̐錾
    public static bool isInput = true;

    //Audio�n�̐錾
    public AudioSource healSE;
    public AudioSource swingSwordSE;
    public AudioSource jumpSE;

    //�|�[�Y��ʗp
    public static bool isPose = false;
    int backCoolTime = 0;

    //�y�ǂƂ̔���
    public static bool isHitPipe = false;
    public static bool isPushPipe = false;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody���֘A�t����
        rb = GetComponent<Rigidbody>();
        //�n�[�g�p
        hearts = new GameObject[remainingHP];
        CreateHearts();

        //��x�y�ǂɓ�������y�ǂ̃|�W�V�����ɕύX����
        if(PipeScript.isTableScene && PipeScript.isSceneChange)
        {
            transform.position = new Vector3(11, -3.5f, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // �J�����̈ʒu�Ɋ�Â��ăn�[�g���X�V
        Vector3 offset = new Vector3(-9, 4.5f, 10); // �J��������̃I�t�Z�b�g

        if (!isPose)
        {
            //�v���C��ʂɖ߂��悤�ɂȂ�܂ł̃N�[���^�C��
            if (backCoolTime != 60)
            {
                backCoolTime++;
            }

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

            //�|�[�Y��ʂ��J������
            OpenPose();

        }

    }

    private void FixedUpdate()
    {
        //���������̓��͂��擾���A���ꂼ��̈ړ����x��������B
        float Xvalue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float horizontalInput = Input.GetAxis("Horizontal");

        //�ړ��x�N�g����ݒ�
        move = new Vector3(horizontalInput, 0, 0);

        //���݂�X,Z�x�N�g���ɏ�̏����Ŏ擾�����l��n���B
        Vector3 movedir = new Vector3(Xvalue, 0, 0);

        if (!isPose)
        {
            //�ړ�����
            Move(movedir, horizontalInput);

        }
        //���C�ɂ�铖���蔻��
        RayHit();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            //�W�����v���ł����Ԃɂ���
            isJump = false;

            animator.SetBool("fall", false);
            animator.SetBool("jumpping", false);
            animator.SetBool("jump", false);
        }

        // ���ׂĂ̏ՓːڐG�_���擾
        foreach (ContactPoint contact in collision.contacts)
        {
            // �ڐG�_�̖@���x�N�g�����I�u�W�F�N�g�̏�����ɋ߂��ꍇ�A�ォ��̏Փ˂ƌ��Ȃ�
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                if (collision.gameObject.tag == "Pipe")
                {
                    //�W�����v���ł����Ԃɂ���
                    isJump = false;

                    animator.SetBool("fall", false);
                    animator.SetBool("jumpping", false);
                    animator.SetBool("jump", false);

                    //�y�ǂɓ����悤�ɂ���
                    isHitPipe = true;

                }
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            //�W�����v���ł����Ԃɂ���
            isJump = false;

            //�A�j���[�V������ύX�B
            animator.SetBool("fall", false);

        }

        // ���ׂĂ̏ՓːڐG�_���擾
        foreach (ContactPoint contact in collision.contacts)
        {
            // �ڐG�_�̖@���x�N�g�����I�u�W�F�N�g�̏�����ɋ߂��ꍇ�A�ォ��̏Փ˂ƌ��Ȃ�
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                if (collision.gameObject.tag == "Pipe")
                {
                    //�W�����v���ł����Ԃɂ���
                    isJump = false;

                    //�A�j���[�V������ύX�B
                    animator.SetBool("fall", false);
                }
            }
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Pipe")
        {
            isHitPipe = false;
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

    //�|�[�Y���J���֐�
    void OpenPose()
    {
        //����̃L�[����������|�[�Y��ʂ��J��
        if (Input.GetKeyDown(KeyCode.JoystickButton2) && !isPose && backCoolTime == 60 || Input.GetKeyDown(KeyCode.Escape) && !isPose && backCoolTime == 60)
        {
            //�|�[�Y��ʂ��J��
            isPose = true;
            backCoolTime = 0;
            //�A�j���[�V������ύX�B
            animator.SetBool("mode", false);
        }

    }

    //���C�Ƃ̓����蔻��
    void RayHit()
    {
        //�v���C���[�̉������փ��C���o��
        Vector3 rayPosition = transform.position + new Vector3(0, 0.5f, 0);
        Ray ray = new Ray(rayPosition, Vector3.down);

        //���C����������
        Debug.DrawRay(rayPosition, Vector3.down * rayDistance, Color.red);

        //���C���n�ʂƓ������Ă��邩�𔻒f����
        isBlock = Physics.Raycast(ray, rayDistance);

        if (isBlock)
        {
            //�ԐF�̃��C��\������
            Debug.DrawRay(rayPosition, Vector3.down * rayDistance, Color.red);

            //�d�͖���
            rb.isKinematic = false;

            //�U���A�j���[�V�����̏�Ԃ��Ď�
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //Attack���A�j���[�V�����̃X�e�[�g���ƈ�v����ꍇ
            if (stateInfo.IsName("landing") && stateInfo.normalizedTime >= 0.5f)
            {
                animator.SetBool("landing", false);
            }

        }
        else
        {
            //�F�̗��\������
            Debug.DrawRay(rayPosition, Vector3.down * rayDistance, Color.blue);

            //�u���b�N�ɂ������Ă��Ȃ����Ƃɂ���
            isHitBlock = false;

            //�W�����v���ł����Ԃɂ���
            isJump = true;

            //�A�j���[�V������ύX
            animator.SetBool("fall", true);

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
        if (Input.GetKey(KeyCode.D) && !isA && isInput || horizontalInput > 0 && isInput)
        {
            //D�{�^����������Ă��邩
            isD = true;
            isAttack = false;
            //���ݒn�ɏ�Ŏ擾�������l�𑫂��Ĉړ�����B
            transform.position += movedir;
            //�v���C���[�̌�����ύX
            transform.rotation = Quaternion.Euler(0, 90, 0);
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
        if (Input.GetKey(KeyCode.A) && !isD && isInput || horizontalInput < 0 && isInput)
        {
            //D�{�^����������Ă��邩
            isA = true;
            isAttack = false;
            //���ݒn�ɏ�Ŏ擾�������l�𑫂��Ĉړ�����B
            transform.position += movedir;
            //�v���C���[�̌�����ύX
            transform.rotation = Quaternion.Euler(0, -90, 0);
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
        if (Input.GetKeyDown(KeyCode.Space) && !isJump && isInput || Input.GetButtonDown("Fire1") && !isJump && isInput)
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
            //�W�����v��SE�𗬂�
            jumpSE.Play();
        }

        if (isJump && rb.velocity.y > 0)
        {
            //�A�j���[�V������ύX�B
            animator.SetBool("jumpping", true);
        }

        if (isJump && rb.velocity.y < 0)
        {
            //�A�j���[�V������ύX�B
            animator.SetBool("fall", true);
        }

    }

    //�U���̊֐�
    void Attack()
    {
        //�U���A�j���[�V�����̏�Ԃ��Ď�
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //�G���^�[��������B�{�^������������U��
        if (Input.GetKeyDown(KeyCode.Return) && !isJump && !isAttack && !isD && !isA && isInput || Input.GetButtonDown("Fire2") && !isJump && !isAttack && !isD && !isA && isInput)
        {
            //�A�j���[�V������ύX�B
            animator.SetBool("attack", true);
            isAttack = true;
            //1�b���SE���Đ�����R���[�`�����J�n
            StartCoroutine(PlaySwingSound(0.4f));
        }

        //Attack���A�j���[�V�����̃X�e�[�g���ƈ�v����ꍇ
        if (isAttack && stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("attack", false);
            isAttack = false;
        }
    }

    //1�b���SE���Đ�����R���[�`��
    private IEnumerator PlaySwingSound(float delay)
    {
        //�w�肳�ꂽ�b���Đ���҂�
        yield return new WaitForSeconds(delay);

        swingSwordSE.Play();

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
    }

}
