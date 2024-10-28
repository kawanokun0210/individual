using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SecondStageGameManager : MonoBehaviour
{
    public GameObject firstStage;//�X�t�B�A�̎Q��
    public GameObject secondStage;//�X�t�B�A�̎Q��

    //�v���n�u������ϐ�
    public GameObject block;//�u���b�N�̎Q��
    public GameObject goal;//�S�[���̎Q��
    public GameObject mashroom;//�L�m�R�̓G��Prefab
    public GameObject player;//�v���C���[�̎Q��
    public GameObject heel;//�񕜃A�C�e���̎Q��
    public GameObject cactus;//�T�{�e���̓G
    public GameObject gost;
    public GameObject moveBlock;
    public GameObject fallBlock;
   
    Vector3 position = Vector3.zero;
    Vector3 enemyPosition = Vector3.zero;
  
    //�Q�[���I�[�o�[�p
    public static bool isStage = false;

    //�g�����W�V�����p
    public Image fadeImage;//�t�F�[�h�A�E�g����C���[�W
    public float fadeDuration = 1.0f;//�t�F�[�h�A�E�g�̎���
    private bool isFading = false;

    //��]�X�s�[�h
    [SerializeField]
    private float rotateSpeed = 0.5f;
    //�X�J�C�{�b�N�X�̃}�e���A��
    private Material skyboxMaterial;

    //�}�b�v�̔z��
    //1���X�e�[�W�u���b�N
    //2���S�[��
    //3���L�m�R�̓G
    //4���񕜃A�C�e��
    //5���T�{�e��
    //6��������
    //7�������u���b�N
    int[,] map =
   {
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        {0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,5,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,5,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,5,0,0,0,2 },
        {1,1,1,1,1,1,1,8,7,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1 },
    };

    // Start is called before the first frame update
    void Start()
    {
        //�ŏ��Ƀt�F�[�h����������
        StartCoroutine(FadeIn());

        //�����őI�������X�e�[�W�̃t���O��ύX
        StageSelectPlayer.secondStage = false;

        //�Q�[���I�[�o�[�Ŏg������
        OverScript.isInput = true;
        OverScript.blinkInterval = 0.6f;

        //�X�e�[�W2�ɗ�����t���O��ύX
        isStage = true;

        //Lighting Settings�Ŏw�肵���X�J�C�{�b�N�X�̃}�e���A�����擾
        skyboxMaterial = RenderSettings.skybox;

        //�����Ńu���b�N�̐���������
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                position.y = -y + 5;
                position.x = x - 5;

                enemyPosition.y = -y + 4.5f;
                enemyPosition.x = x - 5;
                //�X�e�[�W�u���b�N����
                if (map[y, x] == 1)
                {
                    Instantiate(block, position, Quaternion.identity);
                }
                //�S�[������
                if (map[y, x] == 2)
                {
                    Instantiate(goal, position, Quaternion.identity);
                }
                //�G�̐���
                if (map[y, x] == 3)
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, 0);
                    Instantiate(mashroom, enemyPosition, rotation);
                }
                //�񕜃A�C�e���̐���
                if (map[y, x] == 4)
                {
                    Instantiate(heel, position, Quaternion.identity);
                }
                //�G�̐���
                if (map[y, x] == 5)
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, 0);
                    Instantiate(cactus, enemyPosition, rotation);
                }
                //�G�̐���
                if (map[y, x] == 6)
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, 0);
                    Instantiate(gost, enemyPosition, rotation);
                }
                //�X�e�[�W�u���b�N����
                if (map[y, x] == 7)
                {
                    Instantiate(moveBlock, position, Quaternion.identity);
                }
                //�X�e�[�W�u���b�N����
                if (map[y, x] == 8)
                {
                    Instantiate(fallBlock, position, Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�X�J�C�{�b�N�X�}�e���A����Rotation�𑀍삵�Ċp�x��ω�������
        skyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(skyboxMaterial.GetFloat("_Rotation") + rotateSpeed * Time.deltaTime, 360f));
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
