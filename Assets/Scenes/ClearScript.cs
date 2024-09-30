using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ClearScript : MonoBehaviour
{
    public GameObject firstStage;//スフィアの参照
    public GameObject secondStage;//スフィアの参照

    public Image fadeImage;
    public float fadeDuration = 1.0f;//フェードアウトの時間
    private bool isFading = false;

    public TextMeshProUGUI stageSelectText;//TextMeshProUGUIをアタッチ
    public TextMeshProUGUI backTitleText;//TextMeshProUGUIをアタッチ

    public static float blinkInterval = 0.6f;//点滅間隔

    private Coroutine stageSelectCoroutine;
    private Coroutine backTitleCoroutine;

    public static bool stageSelect = true;
    public static bool backTitle = false;
    public static bool isInput = true;
    int coolTime = 0;

    public static bool firstStageClear = false;
    public static bool secondStageClear = false;

    // Start is called before the first frame update
    void Start()
    {
        // 最初にフェードを解除する
        StartCoroutine(FadeIn());

        //ここでクリアしたことにする
        if (GameManagerScript.isStage)
        {
            firstStageClear = true;
        }

        //ここでクリアしたとこにする
        if (SecondStageGameManager.isStage)
        {
            secondStageClear = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

        //再入力までのクールタイム
        if (coolTime <= 120)
        {
            coolTime++;
        }

        //縦の入力待ち
        float verticalInput = Input.GetAxis("Vertical");

        //上下入力した際の処理の関数
        SelectInputUp(verticalInput);
        SelectInputDown(verticalInput);

        //文字の点滅をさせる関数
        Blinking();

        //決定した際にシーンを変更する関数
        SceneChange();

    }

    void SelectInputUp(float verticalInput)
    {
        //上入力されたとき
        if (verticalInput > 0 && backTitle && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && backTitle && coolTime >= 30 && isInput)
        {
            stageSelect = true;
            backTitle = false;
            coolTime = 0;
        }
        else if (verticalInput > 0 && stageSelect && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && stageSelect && coolTime >= 30 && isInput)
        {
            stageSelect = false;
            backTitle = true;
            coolTime = 0;
        }
    }

    void SelectInputDown(float verticalInput)
    {
        //下入力されたとき
        if (verticalInput < 0 && backTitle && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && backTitle && coolTime >= 30 && isInput)
        {
            stageSelect = true;
            backTitle = false;
            coolTime = 0;
        }
        else if (verticalInput < 0 && stageSelect && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && stageSelect && coolTime >= 30 && isInput)
        {
            stageSelect = false;
            backTitle = true;
            coolTime = 0;
        }
    }

    void SceneChange()
    {
        //スペースを押したらシーンを変更する
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && backTitle || Input.GetButtonDown("Fire1") && !isFading && backTitle) 
        {
            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("TitleScene"));
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isFading && stageSelect || Input.GetButtonDown("Fire1") && !isFading && stageSelect)
        {
            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("StageSelectScene"));
        }
    }

    void Blinking()
    {
        if (stageSelect)
        {
            if (stageSelectCoroutine == null)
            {
                stageSelectCoroutine = StartCoroutine(BlinkText(stageSelectText));
            }
            StopBlinkCoroutine(ref backTitleCoroutine, backTitleText);
        }
        else if (backTitle)
        {
            if (backTitleCoroutine == null)
            {
                backTitleCoroutine = StartCoroutine(BlinkText(backTitleText));
            }
            StopBlinkCoroutine(ref stageSelectCoroutine, stageSelectText);
        }
    }

    IEnumerator FadeIn()
    {
        float timer = fadeDuration;
        isFading = true;

        //フェードイン(アルファ値を1から0にする）
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

    private void StopBlinkCoroutine(ref Coroutine coroutine, TextMeshProUGUI textMeshPro)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        textMeshPro.enabled = true; // テキストを再表示
    }

    private IEnumerator BlinkText(TextMeshProUGUI textMeshPro)
    {
        while (true)
        {
            //テキストを非表示にする
            textMeshPro.enabled = false;
            yield return new WaitForSeconds(blinkInterval);

            //テキストを表示する
            textMeshPro.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
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
