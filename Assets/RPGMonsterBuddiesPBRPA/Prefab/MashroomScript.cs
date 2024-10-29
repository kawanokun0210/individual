using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleCollectibleScript;

public class MashroomScript : MonoBehaviour
{
    //プレイヤーのオブジェを取得
    private playerScript playerController;
    public GameObject mashroom;

    //アニメーターコントローラー
    public Animator animator;

    //生きているかの判定
    public bool isDead = false;

    //パーティクル用
    public GameObject collectEffect;

    // Start is called before the first frame update
    void Start()
    {
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

        if (!playerScript.isPose)
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

            //攻撃アニメーションの状態を監視
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //Attackがアニメーションのステート名と一致する場合
            if (isDead && stateInfo.IsName("Dead") && stateInfo.normalizedTime >= 1.0f)
            {
                Collect();
            }
        }

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

    public void Collect()
    {

        if (collectEffect)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
