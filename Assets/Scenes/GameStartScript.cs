using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartScript : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;//TextMeshProUGUI���A�^�b�`
    public static float blinkInterval = 0.6f;//�_�ŊԊu

    private void Start()
    {
        //�R���[�`�����J�n���ē_�ł𐧌�
        StartCoroutine(BlinkText());
    }

    private void Update()
    {
        
    }

    private IEnumerator BlinkText()
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
}
