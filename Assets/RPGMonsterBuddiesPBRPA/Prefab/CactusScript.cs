using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CactusScript : MonoBehaviour
{
    //プレイヤーのオブジェを取得
    private playerScript playerController;
    public GameObject cactus;

    //アニメーターコントローラー
    public Animator animator;

    //生きているかの判定
    public bool isDead = false;

    //攻撃系
    bool isAttack = false;
    float attackDistance = 3;
    int coolTime = 0;

    //死んだときのパーティクル
    public GameObject collectEffect;

    // Start is called before the first frame update
    void Start()
    {
        //ループ時にfalseに戻す
        isDead = false;

        // プレイヤーのオブジェクトを探し、そのスクリプトを取得
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<playerScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, playerController.transform.position);

        //攻撃と歩くアニメーション切り替え
        AttackToWait(distanceToPlayer);

        //向きを変える関数
        DirectionToPlayer();

        //死んだときの関数
        Dead();

    }

    private void OnTriggerEnter(Collider other)
    {
        //もしプレイヤーが攻撃していたら
        if (playerController != null && playerController.isAttack)
        {
            //もし剣と当たっていたら
            if (other.gameObject.tag == "Sword")
            {
                isDead = true;
            }
        }
    }

    void DirectionToPlayer()
    {
        //プレイヤーの方向に敵を向ける
        if (!isDead)
        {
            Vector3 direction = playerController.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }//もし死んでいた時
        else
        {
            animator.SetBool("isDead", true);
        }
    }

    void AttackToWait(float distanceToPlayer)
    {
        //一定の距離近づいたら攻撃し始める
        if (distanceToPlayer <= attackDistance && !isAttack)
        {
            animator.SetBool("isAttack", true);
            isAttack = true;
        }

        //攻撃アニメーションの状態を監視
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //攻撃のクールダウン計測
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

    //死んだときの処理
    void Dead()
    {
        //攻撃アニメーションの状態を監視
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Attackがアニメーションのステート名と一致する場合
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
