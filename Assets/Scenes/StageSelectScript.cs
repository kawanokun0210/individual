using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StageSelectScript : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 20.0f;//�t�F�[�h�A�E�g�̎���
    private bool isFading = false;

    private StageSelectPlayer playerController;

    void Start()
    {
        //�ŏ��Ƀt�F�[�h����������
        StartCoroutine(FadeIn());
        // �v���C���[�̃I�u�W�F�N�g��T���A���̃X�N���v�g���擾
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<StageSelectPlayer>();
        }

        //�_�Ŋ��o�����Z�b�g
        GameStartScript.blinkInterval = 0.6f;

    }

    void Update()
    {
        //�X�y�[�X�L�[�������ꂽ��t�F�[�h�A�E�g���J�n
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && playerController != null && StageSelectPlayer.firstStage || Input.GetButtonDown("Fire1") && !isFading && playerController != null && StageSelectPlayer.firstStage)
        {
            StageSelectPlayer.isInput = false;
            StartCoroutine(FadeOutAndLoadScene("SampleScene"));
        }

        //�X�y�[�X�L�[�������ꂽ��t�F�[�h�A�E�g���J�n
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
}