using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
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
    private bool isOrderChanged = false;
    public bool isBlinking = false;
    int coolTime = 0;
    int backCoolTime = 0;

    //Audioの宣言
    public AudioSource stageBGM;
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
        //常にプレイヤーと同じポジションに移動
        var playerPosition = playerController.transform.position;
        transform.position = playerPosition;
        stageSelectText.transform.position = playerPosition + new Vector3(956.6f, 531.3f, 0);
        backTitleText.transform.position = playerPosition + new Vector3(976.6f, 471.3f, 0);

        //縦の入力待ち
        float verticalInput = Input.GetAxis("Vertical");

        //ポーズ画面が開いていたら
        if (playerScript.isPose)
        {

            //再入力までのクールタイム
            if (coolTime <= 120)
            {
                coolTime++;
            }

            //プレイ画面に戻れるようになるまでのクールタイム
            if(backCoolTime != 60)
            {
                backCoolTime++;
            }

            //ポーズ画面が開かれたとき用
            OpenPose();

            //上下入力した際の処理の関数
            SelectInputUp(verticalInput);
            SelectInputDown(verticalInput);

            //文字の点滅をさせる関数
            Blinking();

            //決定した際にシーンを変更する関数
            SceneChange();

            if (Input.GetKeyDown(KeyCode.JoystickButton7) && backCoolTime == 60 || Input.GetKeyDown(KeyCode.Escape) && backCoolTime == 60)
            {
                //ポーズ画面を閉じる
                playerScript.isPose = false;
                backCoolTime = 0;
            }

        }
        else
        {
            //ポーズ画面を閉じた時
            ClosePose();
        }

    }

    void ClosePose()
    {
        //文字を非表示 
        fadeImage.enabled = false;
        backTitleText.enabled = false;
        stageSelectText.enabled = false;

        //リセットする
        stageSelectCoroutine = null;
        backTitleCoroutine = null;

        //点滅を終了する
        isBlinking = false;

    }

    void OpenPose()
    {
        //ポーズ画面を開く
        fadeImage.enabled = true;

        //点滅を開始する
        isBlinking = true;

        //表示順を変更
        if (!isOrderChanged)
        {
            fadeImage.transform.SetAsFirstSibling();
            backTitleText.transform.SetAsLastSibling();
            stageSelectText.transform.SetAsLastSibling();
            isOrderChanged = true;
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

    void SceneChange()
    {
        //スペースを押したらシーンを変更する
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && backTitle || Input.GetButtonDown("Fire1") && !isFading && backTitle)
        {
            //表示順を変更
            if (isOrderChanged)
            {
                fadeImage.transform.SetAsLastSibling();
                backTitleText.transform.SetAsFirstSibling();
                stageSelectText.transform.SetAsFirstSibling();
            }

            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("TitleScene"));
            dicisionSE.Play();
            stageBGM.Stop();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isFading && stageSelect || Input.GetButtonDown("Fire1") && !isFading && stageSelect)
        {
            //表示順を変更
            if (isOrderChanged)
            {
                fadeImage.transform.SetAsLastSibling();
                backTitleText.transform.SetAsFirstSibling();
                stageSelectText.transform.SetAsFirstSibling();
            }

            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("StageSelectScene"));
            dicisionSE.Play();
            stageBGM.Stop();
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
        while (isBlinking)
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

        // フェードアウト開始時のアルファ値を保存
        float startAlpha = fadeImage.color.a;

        //フェードアウト(アルファ値を0から1にする）
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1, timer / fadeDuration);
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
