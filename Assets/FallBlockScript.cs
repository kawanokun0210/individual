using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBlockScript : MonoBehaviour
{
    public float fallDelay = 3.0f;//ブロックが落下を開始するまでの時間
    public float fallSpeed = 5.0f;//ブロックの落下速度

    private bool isPlayerOnBlock = false;
    private bool isFalling = false;
    private float timeOnBlock = 0f;

    //再度出現させるための宣言
    private float respornTimer = 0;//リセットまでのカウント
    private float respornTime = 10.0f;//リセットまでの時間
    private Vector3 initialPosition;
    private Vector3 initialScale;
    public float respawnScaleSpeed = 2.0f;//拡大速度
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        //最初のポジションを保存しておく
        initialPosition = transform.position;
        //最初のスケールを保存しておく
        initialScale = transform.localScale;
        //BoxColliderを取得
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーがブロックの上にいる場合経過時間を増加させる
        if (isPlayerOnBlock && !isFalling)
        {
            timeOnBlock += Time.deltaTime;

            //指定時間が経過したら落下を開始
            if (timeOnBlock >= fallDelay)
            {
                StartCoroutine(Fall());
            }
        }

        //ブロックが落下していたら
        if (isFalling)
        {
            //下に移動させる
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            //リスポーンまでの時間を図る
            respornTimer += Time.deltaTime;
        }

        //指定の時間になったら
        if(respornTimer >= respornTime && isFalling)
        {
            //元の位置に戻す
            transform.position = initialPosition;
            //拡大させながら出す
            StartCoroutine(Respawn());
            //タイマーをリセットする
            respornTimer = 0;
            //落下してないことにする
            isFalling = false;
            isPlayerOnBlock = false;
            timeOnBlock = 0;
        }

    }

    //プレイヤーがブロックの上に乗ったら呼ばれる
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBlock = true;
            collision.transform.SetParent(transform);
        }
    }

    //プレイヤーがブロックから離れたら呼ばれる
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBlock = false;
            timeOnBlock = 0f;//時間をリセット
            collision.transform.SetParent(null);
        }
    }

    //落下処理のコルーチン
    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(1.0f);//落下開始までの短い待ち時間
    }

    //ブロックを滑らかに拡大しながら再出現させるコルーチン
    private IEnumerator Respawn()
    {
        transform.localScale = Vector3.zero;
        boxCollider.enabled = false;
        float elapsedTime = 0f;

        while (transform.localScale.x < initialScale.x)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, elapsedTime * respawnScaleSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale;//最終的にスケールを正確に戻す
        boxCollider.enabled = true;
    }
}
