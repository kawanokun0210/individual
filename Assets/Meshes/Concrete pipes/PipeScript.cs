using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PipeScript : MonoBehaviour
{
    private playerScript playerController;
    public float moveDuration = 1.0f;//移動にかける時間
    public static bool isMoving = false;//移動中かどうかのフラグ
    private Vector3 startPosition;//移動開始位置
    private Vector3 targetPosition;//移動目標位置
    private float moveTime = 0;//経過時間

    public float moveDistance = 2.0f; // 移動する距離
    public float moveSpeed = 1.0f; // 移動速度

    //Rigidbodyを取得
    public Rigidbody rb;

    //トランジション用
    public Image fadeImage;//フェードアウトするイメージ
    public float fadeDuration = 1.0f;//フェードアウトの時間
    private bool isFading = false;

    private BoxCollider boxCollider;//BoxColliderの参照
    private bool isX = false;
    private static bool isSceneChange = false;

    private void Start()
    {
        //プレイヤーのオブジェクトを探し、そのスクリプトを取得
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
        //Xキーが押されたとき、移動を開始
        if (Input.GetKeyDown(KeyCode.X) && !isMoving && playerScript.isHitPipe)
        {
            startPosition = playerController.transform.position;
            targetPosition = transform.position + new Vector3(0, -1, 0);
            isMoving = true;
            isX = true;
            moveTime = 0;
            boxCollider.enabled = !boxCollider.enabled;
        }

        //移動中の場合、Lerpで移動
        if (isMoving && isX)
        {
            moveTime += Time.deltaTime;
            float t = moveTime / moveDuration;
            playerController.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            //移動が完了したらフラグをリセット
            if (t >= 1.0f)
            {
                isX = false;
                isMoving = false;
                isSceneChange = true;
                StartCoroutine(FadeOutAndLoadScene("SecondStageSceneNo2"));
            }
        }

        if (fadeImage.color.a <= 0 && isSceneChange)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveUp());
            }
        }

    }

    private IEnumerator MoveUp()
    {
        isMoving = true;

        Vector3 startPosition = playerController.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y + moveDistance, startPosition.z);

        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);

        while (playerController.transform.position != targetPosition)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            playerController.transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;
        }

        isMoving = false;
        isSceneChange = false;
        boxCollider.enabled = boxCollider.enabled;

    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float timer = 0;
        isFading = true;

        //フェードアウト(アルファ値を0から1にする）
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration;
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1);

        //シーンをロード
        SceneManager.LoadScene(sceneName);
    }

    private void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }

}
