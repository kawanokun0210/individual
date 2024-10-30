using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageSelectGameObject : MonoBehaviour
{
    //プレハブを入れる変数
    public GameObject block;//ブロックの参照
    public GameObject firstStage;//スフィアの参照
    public GameObject secondStage;//スフィアの参照
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
        //一度変更した物のうち初期に戻す必要性があるものは戻す
        ReStart();

        //ステージをクリアしていたら青色に変更
        StageClear();

        //ここでブロックの生成をする
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                position.y = -y + 0.5f;
                position.x = x - 5;

                spherePosition.y = -y;
                spherePosition.x = x - 5;

                //ステージブロック生成
                if (map[y, x] == 1)
                {
                    Instantiate(block, position, Quaternion.identity);
                }

                //ステージスフィア生成
                if (map[y, x] == 2)
                {
                    Instantiate(firstStage, spherePosition, Quaternion.identity);
                }

                //ステージスフィア生成
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
        //アプリケーション終了時に色を元に戻す
        Renderer renderer = firstStage.GetComponent<Renderer>();
        Renderer secondRenderder = secondStage.GetComponent<Renderer>();

        //ステージ1のsphereを赤に変更
        if (renderer != null)
        {
            renderer.sharedMaterial.color = Color.red;
        }

        //ステージ2のsphereを赤に変更
        if (secondRenderder != null)
        {
            secondRenderder.sharedMaterial.color = Color.red;
        }

    }

    void StageClear()
    {
        //Rendererを取得し、マテリアルの色を青に変更
        Renderer renderer = firstStage.GetComponent<Renderer>();
        Renderer secondRenderder = secondStage.GetComponent<Renderer>();

        //もしステージ１をクリアしていたら
        if (renderer != null && ClearScript.firstStageClear)
        {
            renderer.sharedMaterial.color = Color.blue;//実行中のインスタンスに対して色を変更
        }

        //もしステージ２をクリアしていたら
        if (secondRenderder != null && ClearScript.secondStageClear)
        {
            secondRenderder.sharedMaterial.color = Color.blue;//実行中のインスタンスに対して色を変更
        }

    }

    void ReStart()
    {
        //ここで選択したステージフラグを全てfalseにする
        GameManagerScript.isStage = false;

        //ステージ２
        SecondStageGameManager.isStage = false;

        //ポーズ画面を開かないようにする
        playerScript.isPose = false;
        //全ての入力を有効にする
        playerScript.isInput = true;
        //プレイヤーの体力を元に戻す
        playerScript.remainingHP = 3;

        //落ちるブロックのリセット
        FallBlockScript.isMove = false;

        //ポーズ画面に関することのリセット
        PoseScript.isInput = true;
        PoseScript.blinkInterval = 0.6f;
        PoseScript.stageSelect = true;
        PoseScript.backTitle = false;

        //ゲームクリアで使うもの
        ClearScript.isInput = true;
        ClearScript.blinkInterval = 0.6f;
        ClearScript.stageSelect = true;
        ClearScript.backTitle = false;

        //ゲームオーバーで使うもの
        OverScript.isInput = true;
        OverScript.blinkInterval = 0.6f;
        OverScript.reStart = true;
        OverScript.backTitle = false;
        OverScript.stageSelect = false;
    }

}
