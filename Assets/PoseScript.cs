using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PoseScript : MonoBehaviour
{
    //�v���C���[�̃I�u�W�F���擾
    private playerScript playerController;

    public Image fadeImage;
    public float fadeDuration = 20.0f;//�t�F�[�h�A�E�g�̎���
    private bool isFading = false;

    public TextMeshProUGUI stageSelectText;//TextMeshProUGUI���A�^�b�`
    public TextMeshProUGUI backTitleText;//TextMeshProUGUI���A�^�b�`

    private Coroutine stageSelectCoroutine;
    private Coroutine backTitleCoroutine;

    public static float blinkInterval = 0.6f;//�_�ŊԊu

    public static bool stageSelect = true;
    public static bool backTitle = false;
    public static bool isInput = true;
    int coolTime = 0;

    //Audio�̐錾
    public AudioSource dicisionSE;
    public AudioSource cursorSE;

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�̃I�u�W�F�N�g��T���A���̃X�N���v�g���擾
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
            //�|�[�Y��ʂ��J���ꂽ�Ƃ��p
            OpenPose();



        }

    }

    void OpenPose()
    {
        //�|�[�Y��ʂ��J��
        Color imageColor = fadeImage.color;
        imageColor.a = 0.9f;
        fadeImage.color = imageColor;

        //������\�� 
        Color selectText = stageSelectText.color;
        selectText.a = 1.0f;
        stageSelectText.color = selectText;

        Color titleText = backTitleText.color;
        titleText.a = 1.0f;
        backTitleText.color = titleText;

        //�Q�[����ʂ��ł߂�
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
        //�X�y�[�X����������V�[����ύX����
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
        //����͂��ꂽ�Ƃ�
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
        //�����͂��ꂽ�Ƃ�
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

        textMeshPro.enabled = true; // �e�L�X�g���ĕ\��
    }

    private IEnumerator BlinkText(TextMeshProUGUI textMeshPro)
    {
        while (true)
        {
            //�e�L�X�g���\���ɂ���
            textMeshPro.enabled = false;
            yield return new WaitForSeconds(blinkInterval);

            //�e�L�X�g��\������
            textMeshPro.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
        }
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
