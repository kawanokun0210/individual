using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GostScript : MonoBehaviour
{
    public float moveDistance = 5f;//移動距離
    public float moveSpeed = 2f;//移動速度

    private Vector3 originalPosition;//元の位置
    private Vector3 targetPosition;//目標位置
    private bool movingForward = true;//移動方向

    //アニメーターコントローラー
    public Animator animator;

    private void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + Vector3.right * moveDistance; // 右方向に移動
    }

    private void Update()
    {
        //移動関数
        MoveObject();
    }

    private void MoveObject()
    {
        //現在のターゲット位置に基づいてオブジェクトを移動
        if (movingForward)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                //反転して移動方向を切り替え
                movingForward = false;
                targetPosition = originalPosition;//戻り位置
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);//X軸で反転
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                //反転して移動方向を切り替え
                movingForward = true;
                targetPosition = originalPosition + Vector3.right * moveDistance;//新しい目標位置
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);//X軸で反転
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }

}
