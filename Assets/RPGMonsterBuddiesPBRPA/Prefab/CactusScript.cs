using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CactusScript : MonoBehaviour
{
    //�v���C���[�̃I�u�W�F���擾
    private playerScript playerController;
    public GameObject cactus;

    //�A�j���[�^�[�R���g���[���[
    public Animator animator;

    //�����Ă��邩�̔���
    public bool isDead = false;

    //�U���n
    bool isAttack = false;
    float attackDistance = 3;
    int coolTime = 0;

    //���񂾂Ƃ��̃p�[�e�B�N��
    public GameObject collectEffect;

    // Start is called before the first frame update
    void Start()
    {
        //���[�v����false�ɖ߂�
        isDead = false;

        // �v���C���[�̃I�u�W�F�N�g��T���A���̃X�N���v�g���擾
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<playerScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�Ƃ̋������v�Z
        float distanceToPlayer = Vector3.Distance(transform.position, playerController.transform.position);

        //�U���ƕ����A�j���[�V�����؂�ւ�
        AttackToWait(distanceToPlayer);

        //������ς���֐�
        DirectionToPlayer();

        //���񂾂Ƃ��̊֐�
        Dead();

    }

    private void OnTriggerEnter(Collider other)
    {
        //�����v���C���[���U�����Ă�����
        if (playerController != null && playerController.isAttack)
        {
            //�������Ɠ������Ă�����
            if (other.gameObject.tag == "Sword")
            {
                isDead = true;
            }
        }
    }

    void DirectionToPlayer()
    {
        //�v���C���[�̕����ɓG��������
        if (!isDead)
        {
            Vector3 direction = playerController.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }//��������ł�����
        else
        {
            animator.SetBool("isDead", true);
        }
    }

    void AttackToWait(float distanceToPlayer)
    {
        //���̋����߂Â�����U�����n�߂�
        if (distanceToPlayer <= attackDistance && !isAttack)
        {
            animator.SetBool("isAttack", true);
            isAttack = true;
        }

        //�U���A�j���[�V�����̏�Ԃ��Ď�
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //�U���̃N�[���_�E���v��
        if (isAttack)
        {
            coolTime++;
            if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
            {
                animator.SetBool("isAttack", false);
            }
        }

        if (coolTime >= 120)
        {
            isAttack = false;
            coolTime = 0;
        }

    }

    //���񂾂Ƃ��̏���
    void Dead()
    {
        //�U���A�j���[�V�����̏�Ԃ��Ď�
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Attack���A�j���[�V�����̃X�e�[�g���ƈ�v����ꍇ
        if (isDead && stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f)
        {
            Collect();
        }
    }

    public void Collect()
    {

        if (collectEffect)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
