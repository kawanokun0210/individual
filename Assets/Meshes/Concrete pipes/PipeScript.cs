using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PipeScript : MonoBehaviour
{
    private playerScript playerController;
    public float moveDuration = 1.0f;//�ړ��ɂ����鎞��
    public static bool isMoving = false;//�ړ������ǂ����̃t���O
    private Vector3 startPosition;//�ړ��J�n�ʒu
    private Vector3 targetPosition;//�ړ��ڕW�ʒu
    private float moveTime = 0;//�o�ߎ���

    public float moveDistance = 2.0f; // �ړ����鋗��
    public float moveSpeed = 1.0f; // �ړ����x

    //Rigidbody���擾
    public Rigidbody rb;

    //�g�����W�V�����p
    public Image fadeImage;//�t�F�[�h�A�E�g����C���[�W
    public float fadeDuration = 1.0f;//�t�F�[�h�A�E�g�̎���
    private bool isFading = false;

    private BoxCollider boxCollider;//BoxCollider�̎Q��
    private bool isX = false;
    private static bool isSceneChange = false;

    private void Start()
    {
        //�v���C���[�̃I�u�W�F�N�g��T���A���̃X�N���v�g���擾
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<playerScript>();
        }

        //BoxCollider���擾
        boxCollider = GetComponent<BoxCollider>();

    }

    void Update()
    {
        //X�L�[�������ꂽ�Ƃ��A�ړ����J�n
        if (Input.GetKeyDown(KeyCode.X) && !isMoving && playerScript.isHitPipe)
        {
            startPosition = playerController.transform.position;
            targetPosition = transform.position + new Vector3(0, -1, 0);
            isMoving = true;
            isX = true;
            moveTime = 0;
            boxCollider.enabled = !boxCollider.enabled;
        }

        //�ړ����̏ꍇ�ALerp�ňړ�
        if (isMoving && isX)
        {
            moveTime += Time.deltaTime;
            float t = moveTime / moveDuration;
            playerController.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            //�ړ�������������t���O�����Z�b�g
            if (t >= 1.0f)
            {
                isX = false;
                isMoving = false;
                isSceneChange = true;
                StartCoroutine(FadeOutAndLoadScene("SecondStageSceneNo2"));
            }
        }

        if (fadeImage.color.a <= 0 && isSceneChange)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveUp());
            }
        }

    }

    private IEnumerator MoveUp()
    {
        isMoving = true;

        Vector3 startPosition = playerController.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y + moveDistance, startPosition.z);

        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);

        while (playerController.transform.position != targetPosition)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            playerController.transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;
        }

        isMoving = false;
        isSceneChange = false;
        boxCollider.enabled = boxCollider.enabled;

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
