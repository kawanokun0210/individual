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

    //Audio系の宣言
    public AudioSource bgm;
    public AudioSource se;

    //回転スピード
    [SerializeField]
    private float rotateSpeed = 0.5f;
    //スカイボックスのマテリアル
    private Material skyboxMaterial;

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

        //Lighting Settingsで指定したスカイボックスのマテリアルを取得
        skyboxMaterial = RenderSettings.skybox;

    }

    void Update()
    {
        //スペースキーが押されたらフェードアウトを開始
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && playerController != null && StageSelectPlayer.firstStage || Input.GetButtonDown("Fire1") && !isFading && playerController != null && StageSelectPlayer.firstStage)
        {
            //BGMが流れていたらBGMを止める
            if (bgm.isPlaying)
            {
                bgm.Stop();
            }
            se.Play();
            StageSelectPlayer.isInput = false;
            StartCoroutine(FadeOutAndLoadScene("SampleScene"));
        }

        //スペースキーが押されたらフェードアウトを開始
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && playerController != null && StageSelectPlayer.secondStage || Input.GetButtonDown("Fire1") && !isFading && playerController != null && StageSelectPlayer.secondStage)
        {
            //BGMが流れていたらBGMを止める
            if (bgm.isPlaying)
            {
                bgm.Stop();
            }
            se.Play();
            StageSelectPlayer.isInput = false;
            StartCoroutine(FadeOutAndLoadScene("SecondStageScene"));
        }

        //スカイボックスマテリアルのRotationを操作して角度を変化させる
        skyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(skyboxMaterial.GetFloat("_Rotation") + rotateSpeed * Time.deltaTime, 360f));

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