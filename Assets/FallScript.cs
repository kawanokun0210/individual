using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FallScript : MonoBehaviour
{
    //プレイヤーのオブジェを取得
    private playerScript playerController;
    
    public Image fadeImage;
    public float fadeDuration = 20.0f;//フェードアウトの時間
    private bool isFading = false;

    //Audio用の宣言
    public AudioSource stageBGM;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのオブジェクトを探し、そのスクリプトを取得
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

        //もしプレイヤーが一定の位置まで落ちたら
        if(playerController != null && playerController.isFall)
        {
            //ステージのBGMを止める
            stageBGM.Stop();
            //ゲームオーバー画面に行く
            StartCoroutine(FadeOutAndLoadScene("OverScene"));
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
