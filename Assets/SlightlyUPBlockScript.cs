using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlightlyUPBlockScript : MonoBehaviour
{
    public float fallDistance = 3f;//ブロックが落ちる距離
    public float fallSpeed = 2f;//ブロックが落ちる速度
    public float returnSpeed = 1f;//ブロックが戻る速度
    private Vector3 initialPosition;
    private bool playerOnBlock = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (playerOnBlock)
        {
            //プレイヤーがブロックの上にいる場合、ブロックを落とす
            Vector3 targetPosition = initialPosition + new Vector3(0, fallDistance, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, fallSpeed * Time.deltaTime);
        }
        else
        {
            //プレイヤーがブロックの上にいない場合、ブロックを元の位置に戻す
            transform.position = Vector3.Lerp(transform.position, initialPosition, returnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // プレイヤーの接触点がブロックの上面かどうかを確認
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.down) > 0.5f)
                {
                    playerOnBlock = true;
                    collision.transform.SetParent(transform);
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // プレイヤーの接触点がブロックの上面かどうかを確認
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.down) > 0.5f)
                {
                    collision.transform.SetParent(transform);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnBlock = false;
            collision.transform.SetParent(null);
        }
    }
}

