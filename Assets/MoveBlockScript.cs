using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveBlockScript : MonoBehaviour
{
    public float moveDistance = 5;//移動距離
    public float moveSpeed = 1;//移動速度

    private Vector3 originalPosition;//元の位置
    private Vector3 targetPosition;//目標位置
    private bool movingForward = true;//移動方向

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + Vector3.right * moveDistance;//右方向に移動
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerScript.isPose)
        {
            //移動関数
            MoveObject();
        }
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
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
