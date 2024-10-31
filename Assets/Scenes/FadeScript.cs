using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class FadeScript : MonoBehaviour
{
    public GameObject firstStage;//�X�t�B�A�̎Q��
    public GameObject secondStage;//�X�t�B�A�̎Q��

    public Image fadeImage;
    public float fadeDuration = 20.0f;//�t�F�[�h�A�E�g�̎���
    private bool isFading = false;

    public TextMeshProUGUI gameStartText;//TextMeshProUGUI���A�^�b�`
    public TextMeshProUGUI gameOverText;//TextMeshProUGUI���A�^�b�`

    public static float blinkInterval = 0.6f;//�_�ŊԊu

    private Coroutine gameStartCoroutine;
    private Coroutine gameOverCoroutine;

    public static bool gameStart = true;
    public static bool gameOver = false;
    public static bool isInput = true;
    int coolTime = 0;

    //Audio�n�̐錾
    public AudioSource bgm;
    public AudioSource se;
    public AudioSource cursorSE;

    //��]�X�s�[�h
    [SerializeField]
    private float rotateSpeed = 0.5f;
    //�X�J�C�{�b�N�X�̃}�e���A��
    private Material skyboxMaterial;

    void Start()
    {
        //�ŏ��Ƀt�F�[�h����������
        StartCoroutine(FadeIn());
        //Lighting Settings�Ŏw�肵���X�J�C�{�b�N�X�̃}�e���A�����擾
        skyboxMaterial = RenderSettings.skybox;
    }

    void Update()
    {
        //�ē��͂܂ł̃N�[���^�C��
        if (coolTime <= 120)
        {
            coolTime++;
        }

        //�c�̓��͑҂�
        float verticalInput = Input.GetAxis("Vertical");

        //�㉺���͂����ۂ̏����̊֐�
        SelectInputUp(verticalInput);
        SelectInputDown(verticalInput);

        //�����̓_�ł�������֐�
        Blinking();

        //���肵���ۂɃV�[����ύX����֐�
        SceneChange();
        //�X�J�C�{�b�N�X�}�e���A����Rotation�𑀍삵�Ċp�x��ω�������
        skyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(skyboxMaterial.GetFloat("_Rotation") + rotateSpeed * Time.deltaTime, 360f));

    }

    void SelectInputUp(float verticalInput)
    {
        //����͂��ꂽ�Ƃ�
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
        //�����͂��ꂽ�Ƃ�
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
        //�X�y�[�X����������V�[����ύX����
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
        // Unity�G�f�B�^��ł̓�����m�F���邽�߂̏���
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