using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{
    public float fallDistance = 10.0f;//落下距離
    public float fallSpeed = 0.5f;
    public float riseSpeed = 0.5f;//上昇速度
    private playerScript playerController;

    public float shakeMagnitude = 0.05f;//震える幅
    public float shakeFrequency = 10.0f;//震える速度
    private Vector3 shakeOffset;

    private Vector3 initialPosition;
    private bool isFalling = false;
    private bool isRise = false;
    private int fallTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのオブジェクトを探し、そのスクリプトを取得
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<playerScript>();
        }

        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFalling && IsPlayerBelow() && !isRise)
        {
            ShakeBlock();
            fallTimer++;
        }

        if(fallTimer >= 120)
        {
            StartCoroutine(Fall());
        }

    }

    private bool IsPlayerBelow()
    {
        //石とプレイヤーの水平距離を計算
        float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(playerController.transform.position.x, 0, playerController.transform.position.z));
        return distance < 1.0f && playerController.transform.position.y < transform.position.y;
    }

    //石を下に落とすコルーチン
    private IEnumerator Fall()
    {
        isFalling = true;
       
        while (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        isFalling = false;//落下終了
    }

    //ブロックを震えさせる処理
    private void ShakeBlock()
    {
        // 震えのオフセットを計算
        shakeOffset = Random.insideUnitSphere * shakeMagnitude;
        shakeOffset.z = 0;//Z方向の揺れを防ぐ（2Dゲーム向け）

        //見た目の揺れとして位置変更を適用
        Vector3 newPosition = initialPosition + shakeOffset;
        transform.position = newPosition;
    }

    //石をゆっくり上昇させるコルーチン
    private IEnumerator Rise()
    {
        isRise = true;

        while (transform.position.y < initialPosition.y)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;//最終位置をセット
        isRise = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ブロックと衝突した時
        if (collision.gameObject.tag == "Block")
        {
            StopCoroutine(Fall());//落下を停止
            isFalling = false;
            fallTimer = 0;
            StartCoroutine(Rise());//上昇を開始
        }
    }

}
