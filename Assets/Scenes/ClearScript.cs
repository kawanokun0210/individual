using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ClearScript : MonoBehaviour
{
    public GameObject firstStage;//�X�t�B�A�̎Q��
    public GameObject secondStage;//�X�t�B�A�̎Q��

    public Image fadeImage;
    public float fadeDuration = 1.0f;//�t�F�[�h�A�E�g�̎���
    private bool isFading = false;

    public TextMeshProUGUI stageSelectText;//TextMeshProUGUI���A�^�b�`
    public TextMeshProUGUI backTitleText;//TextMeshProUGUI���A�^�b�`

    public static float blinkInterval = 0.6f;//�_�ŊԊu

    private Coroutine stageSelectCoroutine;
    private Coroutine backTitleCoroutine;

    public static bool stageSelect = true;
    public static bool backTitle = false;
    public static bool isInput = true;
    int coolTime = 0;

    public static bool firstStageClear = false;
    public static bool secondStageClear = false;

    //��]�X�s�[�h
    [SerializeField]
    private float rotateSpeed = 0.5f;
    //�X�J�C�{�b�N�X�̃}�e���A��
    private Material skyboxMaterial;

    //Audio�̐錾
    public AudioSource clearBGM;
    public AudioSource dicisionSE;
    public AudioSource cursorSE;

    // Start is called before the first frame update
    void Start()
    {
        // �ŏ��Ƀt�F�[�h����������
        StartCoroutine(FadeIn());

        //�����ŃN���A�������Ƃɂ���
        if (GameManagerScript.isStage)
        {
            firstStageClear = true;
        }

        //�����ŃN���A�����Ƃ��ɂ���
        if (SecondStageGameManager.isStage)
        {
            secondStageClear = true;
        }

        //Lighting Settings�Ŏw�肵���X�J�C�{�b�N�X�̃}�e���A�����擾
        skyboxMaterial = RenderSettings.skybox;

    }

    // Update is called once per frame
    void Update()
    {

        //�ē��͂܂ł̃N�[���^�C��
        if (coolTime <= 120)
        {
            coolTime++;
        }

        //�c�̓��͑҂�
        float verticalInput = Input.GetAxis("Vertical");

        //�X�J�C�{�b�N�X�}�e���A����Rotation�𑀍삵�Ċp�x��ω�������
        skyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(skyboxMaterial.GetFloat("_Rotation") + rotateSpeed * Time.deltaTime, 360f));

        //�㉺���͂����ۂ̏����̊֐�
        SelectInputUp(verticalInput);
        SelectInputDown(verticalInput);

        //�����̓_�ł�������֐�
        Blinking();

        //���肵���ۂɃV�[����ύX����֐�
        SceneChange();

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

    void SceneChange()
    {
        //�X�y�[�X����������V�[����ύX����
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && backTitle || Input.GetButtonDown("Fire1") && !isFading && backTitle) 
        {
            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("TitleScene"));
            clearBGM.Stop();
            dicisionSE.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isFading && stageSelect || Input.GetButtonDown("Fire1") && !isFading && stageSelect)
        {
            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("StageSelectScene"));
            clearBGM.Stop();
            dicisionSE.Play();
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

        //�t�F�[�h�C��(�A���t�@�l��1����0�ɂ���j
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
