using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private bool isOrderChanged = false;
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
        //��Ƀv���C���[�Ɠ����|�W�V�����Ɉړ�
        var playerPosition = playerController.transform.position;
        transform.position = playerPosition;
        stageSelectText.transform.position = playerPosition + new Vector3(546.6f, 251.3f, 0);
        backTitleText.transform.position = playerPosition + new Vector3(506.6f, 201.3f, 0);

        //�c�̓��͑҂�
        float verticalInput = Input.GetAxis("Vertical");

        if (playerScript.isPose)
        {

            //�ē��͂܂ł̃N�[���^�C��
            if (coolTime <= 120)
            {
                coolTime++;
            }

            //�|�[�Y��ʂ��J���ꂽ�Ƃ��p
            OpenPose();

            //�㉺���͂����ۂ̏����̊֐�
            SelectInputUp(verticalInput);
            SelectInputDown(verticalInput);

            //�����̓_�ł�������֐�
            Blinking();

            //���肵���ۂɃV�[����ύX����֐�
            SceneChange();

        }
        else
        {
            //�|�[�Y��ʂ������
            ClosePose();
        }

    }

    void ClosePose()
    {
        //�������\�� 
        fadeImage.enabled = false;
        backTitleText.enabled = false;
        stageSelectText.enabled = false;

        //�|�[�Y��ʂ���v���C��ʂ�
        playerScript.isPose = false;

    }

    void OpenPose()
    {
        //�|�[�Y��ʂ��J��
        fadeImage.enabled = true;

        //�\������ύX
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
        //�X�y�[�X����������V�[����ύX����
        if (Input.GetKeyDown(KeyCode.Space) && !isFading && backTitle || Input.GetButtonDown("Fire1") && !isFading && backTitle)
        {
            //�\������ύX
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
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isFading && stageSelect || Input.GetButtonDown("Fire1") && !isFading && stageSelect)
        {
            //�\������ύX
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

        // �t�F�[�h�A�E�g�J�n���̃A���t�@�l��ۑ�
        float startAlpha = fadeImage.color.a;

        //�t�F�[�h�A�E�g(�A���t�@�l��0����1�ɂ���j
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1, timer / fadeDuration);
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
