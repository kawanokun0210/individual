using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StageSelectScript : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 20.0f;//フェードアウトの時間
    private bool isFading = false;

    private StageSelectPlayer playerController;

    void Start()
    {
        //最初にフェードを解除する
        StartCoroutine(FadeIn());
        // プレイヤーのオブジェクトを探し、そのスクリプトを取得
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<StageSelectPlayer>();
        }

        //点滅感覚をリセット
        GameStartScript.blinkInterval = 0.6f;

    }

    void Update()
    {
        //スペースキーが押されたらフェードアウトを開始
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && playerController != null && StageSelectPlayer.firstStage || Input.GetButtonDown("Fire1") && !isFading && playerController != null && StageSelectPlayer.firstStage)
        {
            StageSelectPlayer.isInput = false;
            StartCoroutine(FadeOutAndLoadScene("SampleScene"));
        }

        //スペースキーが押されたらフェードアウトを開始
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && playerController != null && StageSelectPlayer.secondStage || Input.GetButtonDown("Fire1") && !isFading && playerController != null && StageSelectPlayer.secondStage)
        {
            StageSelectPlayer.isInput = false;
            StartCoroutine(FadeOutAndLoadScene("SecondStageScene"));
        }

    }

    IEnumerator FadeIn()
    {
        float timer = fadeDuration;
        isFading = true;

        // フェードイン（アルファ値を1から0にする）
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float alpha = timer / fadeDuration;
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0);
        isFading = false;
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