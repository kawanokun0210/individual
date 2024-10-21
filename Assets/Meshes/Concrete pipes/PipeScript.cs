using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    private playerScript playerController;
    public float moveDuration = 1.0f; // 移動にかける時間
    public static bool isMoving = false; // 移動中かどうかのフラグ
    private Vector3 startPosition; // 移動開始位置
    private Vector3 targetPosition; // 移動目標位置
    private float moveTime = 0; // 経過時間

    private BoxCollider boxCollider; // BoxColliderの参照

    private void Start()
    {
        // プレイヤーのオブジェクトを探し、そのスクリプトを取得
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<playerScript>();
        }

        //BoxColliderを取得
        boxCollider = GetComponent<BoxCollider>();

    }

    void Update()
    {
        // Xキーが押されたとき、移動を開始
        if (Input.GetKeyDown(KeyCode.X) && !isMoving && playerScript.isHitPipe)
        {
            startPosition = playerController.transform.position;
            targetPosition = transform.position + new Vector3(0, -1, 0);
            isMoving = true;
            moveTime = 0;
            boxCollider.enabled = !boxCollider.enabled;
        }

        // 移動中の場合、Lerpで移動
        if (isMoving)
        {
            moveTime += Time.deltaTime;
            float t = moveTime / moveDuration;
            playerController.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // 移動が完了したらフラグをリセット
            if (t >= 1.0f)
            {
                isMoving = false;
            }
        }
    }
}
