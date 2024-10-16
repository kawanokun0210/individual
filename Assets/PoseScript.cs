using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PoseScript : MonoBehaviour
{
    //プレイヤーのオブジェを取得
    private playerScript playerController;

    public Image fadeImage;
    public float fadeDuration = 20.0f;//フェードアウトの時間
    private bool isFading = false;

    public TextMeshProUGUI stageSelectText;//TextMeshProUGUIをアタッチ
    public TextMeshProUGUI backTitleText;//TextMeshProUGUIをアタッチ

    private Coroutine stageSelectCoroutine;
    private Coroutine backTitleCoroutine;

    public static float blinkInterval = 0.6f;//点滅間隔

    public static bool stageSelect = true;
    public static bool backTitle = false;
    public static bool isInput = true;
    int coolTime = 0;

    //Audioの宣言
    public AudioSource dicisionSE;
    public AudioSource cursorSE;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのオブジェクトを探し、そのスクリプトを取得
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<playerScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerPosition = playerController.transform.position;
        transform.position = playerPosition;
        stageSelectText.transform.position = playerPosition + new Vector3(546.6f, 251.3f, 0);
        backTitleText.transform.position = playerPosition + new Vector3(506.6f, 201.3f, 0);

        if (playerScript.isPose)
        {
            //ポーズ画面が開かれたとき用
            OpenPose();



        }

    }

    void OpenPose()
    {
        //ポーズ画面を開く
        Color imageColor = fadeImage.color;
        imageColor.a = 0.9f;
        fadeImage.color = imageColor;

        //文字を表示 
        Color selectText = stageSelectText.color;
        selectText.a = 1.0f;
        stageSelectText.color = selectText;

        Color titleText = backTitleText.color;
        titleText.a = 1.0f;
        backTitleText.color = titleText;

        //ゲーム画面を固める
        Time.timeScale = 0.0f;
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

    void SceneChange()
    {
        //スペースを押したらシーンを変更する
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && backTitle || Input.GetButtonDown("Fire1") && !isFading && backTitle)
        {
            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("TitleScene"));
            dicisionSE.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isFading && stageSelect || Input.GetButtonDown("Fire1") && !isFading && stageSelect)
        {
            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("StageSelectScene"));
            dicisionSE.Play();
        }
    }

    void SelectInputUp(float verticalInput)
    {
        //上入力されたとき
        if (verticalInput > 0 && backTitle && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && backTitle && coolTime >= 30 && isInput)
        {
            stageSelect = true;
            backTitle = false;
            coolTime = 0;
            cursorSE.Play();
        }
        else if (verticalInput > 0 && stageSelect && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && stageSelect && coolTime >= 30 && isInput)
        {
            stageSelect = false;
            backTitle = true;
            coolTime = 0;
            cursorSE.Play();
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
            cursorSE.Play();
        }
        else if (verticalInput < 0 && stageSelect && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && stageSelect && coolTime >= 30 && isInput)
        {
            stageSelect = false;
            backTitle = true;
            coolTime = 0;
            cursorSE.Play();
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
