using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartScript : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;//TextMeshProUGUIをアタッチ
    public static float blinkInterval = 0.6f;//点滅間隔

    private void Start()
    {
        //コルーチンを開始して点滅を制御
        StartCoroutine(BlinkText());
    }

    private void Update()
    {
        
    }

    private IEnumerator BlinkText()
    {
        while (true)
        {
            //テキストを非表示にする
            textMeshPro.enabled = false;
            yield return new WaitForSeconds(blinkInterval);

            //テキストを表示する
            textMeshPro.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
