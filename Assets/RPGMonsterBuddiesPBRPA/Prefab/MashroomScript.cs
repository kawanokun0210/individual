using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleCollectibleScript;

public class MashroomScript : MonoBehaviour
{
    //�v���C���[�̃I�u�W�F���擾
    private playerScript playerController;
    public GameObject mashroom;

    //�A�j���[�^�[�R���g���[���[
    public Animator animator;

    //�����Ă��邩�̔���
    public bool isDead = false;

    //�p�[�e�B�N���p
    public GameObject collectEffect;

    // Start is called before the first frame update
    void Start()
    {
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

        if (!playerScript.isPose)
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

            //�U���A�j���[�V�����̏�Ԃ��Ď�
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //Attack���A�j���[�V�����̃X�e�[�g���ƈ�v����ꍇ
            if (isDead && stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f)
            {
                Collect();
            }
        }

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

    public void Collect()
    {

        if (collectEffect)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
