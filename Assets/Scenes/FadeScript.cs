using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class FadeScript : MonoBehaviour
{
    public GameObject firstStage;//スフィアの参照
    public GameObject secondStage;//スフィアの参照

    public Image fadeImage;
    public float fadeDuration = 20.0f;//フェードアウトの時間
    private bool isFading = false;

    public TextMeshProUGUI gameStartText;//TextMeshProUGUIをアタッチ
    public TextMeshProUGUI gameOverText;//TextMeshProUGUIをアタッチ

    public static float blinkInterval = 0.6f;//点滅間隔

    private Coroutine gameStartCoroutine;
    private Coroutine gameOverCoroutine;

    public static bool gameStart = true;
    public static bool gameOver = false;
    public static bool isInput = true;
    int coolTime = 0;

    //Audio系の宣言
    public AudioSource bgm;
    public AudioSource se;
    public AudioSource cursorSE;

    //回転スピード
    [SerializeField]
    private float rotateSpeed = 0.5f;
    //スカイボックスのマテリアル
    private Material skyboxMaterial;

    void Start()
    {
        //最初にフェードを解除する
        StartCoroutine(FadeIn());
        //Lighting Settingsで指定したスカイボックスのマテリアルを取得
        skyboxMaterial = RenderSettings.skybox;
    }

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
        //スカイボックスマテリアルのRotationを操作して角度を変化させる
        skyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(skyboxMaterial.GetFloat("_Rotation") + rotateSpeed * Time.deltaTime, 360f));

    }

    void SelectInputUp(float verticalInput)
    {
        //上入力されたとき
        if (verticalInput > 0 && gameOver && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && gameOver && coolTime >= 30 && isInput)
        {
            gameStart = true;
            gameOver = false;
            coolTime = 0;
            cursorSE.Play();
        }
        else if (verticalInput > 0 && gameStart && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && gameStart && coolTime >= 30 && isInput)
        {
            gameStart = false;
            gameOver = true;
            coolTime = 0;
            cursorSE.Play();
        }
    }

    void SelectInputDown(float verticalInput)
    {
        //下入力されたとき
        if (verticalInput < 0 && gameOver && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && gameOver && coolTime >= 30 && isInput)
        {
            gameStart = true;
            gameOver = false;
            coolTime = 0;
            cursorSE.Play();
        }
        else if (verticalInput < 0 && gameStart && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && gameStart && coolTime >= 30 && isInput)
        {
            gameStart = false;
            gameOver = true;
            coolTime = 0;
            cursorSE.Play();
        }
    }

    void SceneChange()
    {
        //スペースを押したらシーンを変更する
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && gameOver || Input.GetButtonDown("Fire1") && !isFading && gameOver)
        {
            isInput = false;
            blinkInterval = 0.1f;
            bgm.Stop();
            se.Play();
            QuitGame();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isFading && gameStart || Input.GetButtonDown("Fire1") && !isFading && gameStart)
        {
            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("StageSelectScene"));
            bgm.Stop();
            se.Play();
        }
    }

    void Blinking()
    {
        if (gameStart)
        {
            if (gameStartCoroutine == null)
            {
                gameStartCoroutine = StartCoroutine(BlinkText(gameStartText));
            }
            StopBlinkCoroutine(ref gameOverCoroutine, gameOverText);
        }
        else if (gameOver)
        {
            if (gameOverCoroutine == null)
            {
                gameOverCoroutine = StartCoroutine(BlinkText(gameOverText));
            }
            StopBlinkCoroutine(ref gameStartCoroutine, gameStartText);
        }
    }

    void QuitGame()
    {
        // Unityエディタ上での動作を確認するための処理
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
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