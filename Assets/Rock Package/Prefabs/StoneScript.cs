using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{
    public float fallDistance = 10.0f;//落下距離
    public float fallSpeed = 2.0f;
    public float riseSpeed = 0.5f;//上昇速度
    private playerScript playerController;

    public float shakeMagnitude = 0.05f;//震える幅
    public float shakeFrequency = 10.0f;//震える速度
    private Vector3 shakeOffset;

    private Vector3 initialPosition;
    private bool isFalling = false;
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
        if(!isFalling && IsPlayerBelow())
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
        float elapsedTime = 0f;
        Vector3 targetPosition = initialPosition + Vector3.down * fallDistance;

        while (isFalling == true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fallSpeed;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;//最終位置をセット
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
        float elapsedTime = 0f;
        Vector3 currentPosition = transform.position;

        while (transform.position != initialPosition)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / riseSpeed;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(currentPosition, initialPosition, t);
            yield return null;
        }

        transform.position = initialPosition;//最終位置をセット
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
