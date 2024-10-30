using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageSelectGameObject : MonoBehaviour
{
    //�v���n�u������ϐ�
    public GameObject block;//�u���b�N�̎Q��
    public GameObject firstStage;//�X�t�B�A�̎Q��
    public GameObject secondStage;//�X�t�B�A�̎Q��
    Vector3 position = Vector3.zero;
    Vector3 spherePosition = Vector3.zero;

    int[,] map =
    {
        {0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0 },
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
    };

    // Start is called before the first frame update
    void Start()
    {
        //��x�ύX�������̂��������ɖ߂��K�v����������͖̂߂�
        ReStart();

        //�X�e�[�W���N���A���Ă�����F�ɕύX
        StageClear();

        //�����Ńu���b�N�̐���������
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                position.y = -y + 0.5f;
                position.x = x - 5;

                spherePosition.y = -y;
                spherePosition.x = x - 5;

                //�X�e�[�W�u���b�N����
                if (map[y, x] == 1)
                {
                    Instantiate(block, position, Quaternion.identity);
                }

                //�X�e�[�W�X�t�B�A����
                if (map[y, x] == 2)
                {
                    Instantiate(firstStage, spherePosition, Quaternion.identity);
                }

                //�X�e�[�W�X�t�B�A����
                if (map[y, x] == 3)
                {
                    Instantiate(secondStage, spherePosition, Quaternion.identity);
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        //�A�v���P�[�V�����I�����ɐF�����ɖ߂�
        Renderer renderer = firstStage.GetComponent<Renderer>();
        Renderer secondRenderder = secondStage.GetComponent<Renderer>();

        //�X�e�[�W1��sphere��ԂɕύX
        if (renderer != null)
        {
            renderer.sharedMaterial.color = Color.red;
        }

        //�X�e�[�W2��sphere��ԂɕύX
        if (secondRenderder != null)
        {
            secondRenderder.sharedMaterial.color = Color.red;
        }

    }

    void StageClear()
    {
        //Renderer���擾���A�}�e���A���̐F��ɕύX
        Renderer renderer = firstStage.GetComponent<Renderer>();
        Renderer secondRenderder = secondStage.GetComponent<Renderer>();

        //�����X�e�[�W�P���N���A���Ă�����
        if (renderer != null && ClearScript.firstStageClear)
        {
            renderer.sharedMaterial.color = Color.blue;//���s���̃C���X�^���X�ɑ΂��ĐF��ύX
        }

        //�����X�e�[�W�Q���N���A���Ă�����
        if (secondRenderder != null && ClearScript.secondStageClear)
        {
            secondRenderder.sharedMaterial.color = Color.blue;//���s���̃C���X�^���X�ɑ΂��ĐF��ύX
        }

    }

    void ReStart()
    {
        //�����őI�������X�e�[�W�t���O��S��false�ɂ���
        GameManagerScript.isStage = false;

        //�X�e�[�W�Q
        SecondStageGameManager.isStage = false;

        //�|�[�Y��ʂ��J���Ȃ��悤�ɂ���
        playerScript.isPose = false;
        //�S�Ă̓��͂�L���ɂ���
        playerScript.isInput = true;
        //�v���C���[�̗̑͂����ɖ߂�
        playerScript.remainingHP = 3;

        //������u���b�N�̃��Z�b�g
        FallBlockScript.isMove = false;

        //�|�[�Y��ʂɊւ��邱�Ƃ̃��Z�b�g
        PoseScript.isInput = true;
        PoseScript.blinkInterval = 0.6f;
        PoseScript.stageSelect = true;
        PoseScript.backTitle = false;

        //�Q�[���N���A�Ŏg������
        ClearScript.isInput = true;
        ClearScript.blinkInterval = 0.6f;
        ClearScript.stageSelect = true;
        ClearScript.backTitle = false;

        //�Q�[���I�[�o�[�Ŏg������
        OverScript.isInput = true;
        OverScript.blinkInterval = 0.6f;
        OverScript.reStart = true;
        OverScript.backTitle = false;
        OverScript.stageSelect = false;
    }

}
