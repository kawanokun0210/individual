using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoTextScript : MonoBehaviour
{
    public float scaleSpeed = 2;//�g��E�k���̑��x
    public Vector3 maxScale = new Vector3(3, 1, 1);//�ő�T�C�Y
    public Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);//�ŏ��T�C�Y

    private Vector3 targetScale;//�ڕW�̃X�P�[��

    void Start()
    {
        transform.localScale = minScale;//�ŏ��͏k�����ꂽ���
        targetScale = minScale;//�����̖ڕW�X�P�[��
    }

    void Update()
    {
        //�����𖞂����Ă���Ƃ��Ɋg��
        if (StageSelectPlayer.secondStage)
        {
            targetScale = maxScale;
        }
        //�����𖞂����Ă��Ȃ��Ƃ��ɏk��
        else
        {
            targetScale = minScale;
        }

        //Cube�̃X�P�[�������X�ɕω�������
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

    }
}
