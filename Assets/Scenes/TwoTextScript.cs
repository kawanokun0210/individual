using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoTextScript : MonoBehaviour
{
    public float scaleSpeed = 2;//拡大・縮小の速度
    public Vector3 maxScale = new Vector3(3, 1, 1);//最大サイズ
    public Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);//最小サイズ

    private Vector3 targetScale;//目標のスケール

    void Start()
    {
        transform.localScale = minScale;//最初は縮小された状態
        targetScale = minScale;//初期の目標スケール
    }

    void Update()
    {
        //条件を満たしているときに拡大
        if (StageSelectPlayer.secondStage)
        {
            targetScale = maxScale;
        }
        //条件を満たしていないときに縮小
        else
        {
            targetScale = minScale;
        }

        //Cubeのスケールを徐々に変化させる
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

    }
}
