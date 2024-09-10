using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectCamera : MonoBehaviour
{
    //プレイヤーのオブジェを追跡
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //プレイヤー追跡型のスクロール
        //カメラとプレイヤーの位置を取得
        var playerPosition = player.transform.position;
        var position = transform.position;

        //ここでカメラにプレイヤーの座標を代入
        position.x = playerPosition.x;
        position.y = playerPosition.y + 2;
        position.z = playerPosition.z - 15;
        transform.position = position;

    }
}
