using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class FadeScript : MonoBehaviour
{
    public GameObject firstStage;//�X�t�B�A�̎Q��
    public GameObject secondStage;//�X�t�B�A�̎Q��

    public Image fadeImage;
    public float fadeDuration = 20.0f;//�t�F�[�h�A�E�g�̎���
    private bool isFading = false;

    //Audio�n�̐錾
    public AudioSource bgm;
    public AudioSource se;

    void Start()
    {
        //�ŏ��Ƀt�F�[�h����������
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        //�X�y�[�X�L�[�������ꂽ��t�F�[�h�A�E�g���J�n
        if (Input.GetKeyDown(KeyCode.Space) && !isFading || Input.GetButtonDown("Fire1") && !isFading)
        {
            //BGM������Ă�����BGM���~�߂�
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

        // �t�F�[�h�C���i�A���t�@�l��1����0�ɂ���j
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

        //�t�F�[�h�A�E�g(�A���t�@�l��0����1�ɂ���j
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration;
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1);

        //�V�[�������[�h
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
        //�A�v���P�[�V�����I�����ɐF�����ɖ߂�
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