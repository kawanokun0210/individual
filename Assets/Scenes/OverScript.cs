using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverScript : MonoBehaviour
{
    public GameObject firstStage;//�X�t�B�A�̎Q��
    public GameObject secondStage;//�X�t�B�A�̎Q��

    public Image fadeImage;
    public float fadeDuration = 20.0f;//�t�F�[�h�A�E�g�̎���
    private bool isFading = false;

    public TextMeshProUGUI reStartText;//TextMeshProUGUI���A�^�b�`
    public TextMeshProUGUI stageSelectText;//TextMeshProUGUI���A�^�b�`
    public TextMeshProUGUI backTitleText;//TextMeshProUGUI���A�^�b�`

    public static float blinkInterval = 0.6f;//�_�ŊԊu

    private Coroutine reStartCoroutine;
    private Coroutine stageSelectCoroutine;
    private Coroutine backTitleCoroutine;

    //�I�����p�t���O
    public static bool reStart = true;
    public static bool stageSelect = false;
    public static bool backTitle = false;
    public static bool isInput = true;
    int coolTime = 0;

    //��]�X�s�[�h
    [SerializeField]
    private float rotateSpeed = 0.5f;
    //�X�J�C�{�b�N�X�̃}�e���A��
    private Material skyboxMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //�ŏ��Ƀt�F�[�h����������
        StartCoroutine(FadeIn());

        //Lighting Settings�Ŏw�肵���X�J�C�{�b�N�X�̃}�e���A�����擾
        skyboxMaterial = RenderSettings.skybox;

    }

    // Update is called once per frame
    void Update()
    {

        if(coolTime <= 120)
        {
            coolTime++;
        }

        //�c�̓��͑҂�
        float verticalInput = Input.GetAxis("Vertical");

        //�X�J�C�{�b�N�X�}�e���A����Rotation�𑀍삵�Ċp�x��ω�������
        skyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(skyboxMaterial.GetFloat("_Rotation") + rotateSpeed * Time.deltaTime, 360f));

        //�㉺�̓��͂����ۂ̏����̊֐�
        SelectInputUp(verticalInput);
        SelectInputDown(verticalInput);

        //�����̓_�ł�������֐�
        Blinking();

        //����������ۂɃV�[����ύX���鏈���̊֐�
        SceneChange();

    }
    void SelectInputUp(float verticalInput) {
        //����͂��ꂽ�Ƃ�
        if (verticalInput > 0 && reStart && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && reStart && coolTime >= 30 && isInput)
        {
            backTitle = true;
            reStart = false;
            stageSelect = false;
            coolTime = 0;
        }
        else if (verticalInput > 0 && backTitle && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && backTitle && coolTime >= 30 && isInput)
        {
            stageSelect = true;
            backTitle = false;
            reStart = false;
            coolTime = 0;
        }
        else if (verticalInput > 0 && stageSelect && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && stageSelect && coolTime >= 30 && isInput)
        {
            reStart = true;
            stageSelect = false;
            backTitle = false;
            coolTime = 0;
        }
    } 

    void SelectInputDown(float verticalInput)
    {
        //�����͂��ꂽ�Ƃ�
        if (verticalInput < 0 && reStart && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && reStart && coolTime >= 30 && isInput)
        {
            backTitle = false;
            reStart = false;
            stageSelect = true;
            coolTime = 0;
        }
        else if (verticalInput < 0 && backTitle && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && backTitle && coolTime >= 30 && isInput)
        {
            stageSelect = false;
            backTitle = false;
            reStart = true;
            coolTime = 0;
        }
        else if (verticalInput < 0 && stageSelect && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && stageSelect && coolTime >= 30 && isInput)
        {
            reStart = false;
            stageSelect = false;
            backTitle = true;
            coolTime = 0;
        }
    }

    void Blinking()
    {
        if (reStart)
        {

            if (reStartCoroutine == null)
            {
                reStartCoroutine = StartCoroutine(BlinkText(reStartText));
            }
            StopBlinkCoroutine(ref stageSelectCoroutine, stageSelectText);
            StopBlinkCoroutine(ref backTitleCoroutine, backTitleText);
        }
        else if (stageSelect)
        {
            if (stageSelectCoroutine == null)
            {
                stageSelectCoroutine = StartCoroutine(BlinkText(stageSelectText));
            }
            StopBlinkCoroutine(ref reStartCoroutine, reStartText);
            StopBlinkCoroutine(ref backTitleCoroutine, backTitleText);
        }
        else if (backTitle)
        {
            if (backTitleCoroutine == null)
            {
                backTitleCoroutine = StartCoroutine(BlinkText(backTitleText));
            }
            StopBlinkCoroutine(ref stageSelectCoroutine, stageSelectText);
            StopBlinkCoroutine(ref reStartCoroutine, reStartText);
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
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isFading && reStart || Input.GetButtonDown("Fire1") && !isFading && reStart)
        {
            if (GameManagerScript.isStage)
            {
                isInput = false;
                blinkInterval = 0.1f;
                StartCoroutine(FadeOutAndLoadScene("SampleScene"));
            }
            else if (SecondStageGameManager.isStage)
            {
                isInput = false;
                blinkInterval = 0.1f;
                StartCoroutine(FadeOutAndLoadScene("SecondStageScene"));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isFading && stageSelect || Input.GetButtonDown("Fire1") && !isFading && stageSelect)
        {
            isInput = false;
            blinkInterval = 0.1f;
            StartCoroutine(FadeOutAndLoadScene("StageSelectScene"));
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
