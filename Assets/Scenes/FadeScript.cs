using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class FadeScript : MonoBehaviour
{
    public GameObject firstStage;//スフィアの参照
    public GameObject secondStage;//スフィアの参照

    public Image fadeImage;
    public float fadeDuration = 20.0f;//フェードアウトの時間
    private bool isFading = false;

    //Audio系の宣言
    public AudioSource bgm;
    public AudioSource se;

    void Start()
    {
        //最初にフェードを解除する
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        //スペースキーが押されたらフェードアウトを開始
        if (Input.GetKeyDown(KeyCode.Space) && !isFading || Input.GetButtonDown("Fire1") && !isFading)
        {
            //BGMが流れていたらBGMを止める
            if (bgm.isPlaying)
            {
                bgm.Stop();
            }
            se.Play();
            GameStartScript.blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("StageSelectScene"));
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

    private void OnApplicationQuit()
    {
        //アプリケーション終了時に色を元に戻す
        Renderer renderer = firstStage.GetComponent<Renderer>();
        Renderer secondRenderder = secondStage.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sharedMaterial.color = Color.red;
        }

        if (secondRenderder != null)
        {
            secondRenderder.sharedMaterial.color = Color.red;
        }

    }

}