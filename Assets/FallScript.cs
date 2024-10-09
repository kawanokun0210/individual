using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FallScript : MonoBehaviour
{
    //�v���C���[�̃I�u�W�F���擾
    private playerScript playerController;
    
    public Image fadeImage;
    public float fadeDuration = 20.0f;//�t�F�[�h�A�E�g�̎���
    private bool isFading = false;

    //Audio�p�̐錾
    public AudioSource stageBGM;

    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[�̃I�u�W�F�N�g��T���A���̃X�N���v�g���擾
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

        //�����v���C���[�����̈ʒu�܂ŗ�������
        if(playerController != null && playerController.isFall)
        {
            //�X�e�[�W��BGM���~�߂�
            stageBGM.Stop();
            //�Q�[���I�[�o�[��ʂɍs��
            StartCoroutine(FadeOutAndLoadScene("OverScene"));
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
