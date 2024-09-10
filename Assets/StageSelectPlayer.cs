using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StageSelectPlayer : MonoBehaviour
{

    //アニメーターコントローラー
    public Animator animator;
    //移動スピード
    float moveSpeed = 6.0f;
    private Vector3 move;
    //押されているボタン
    bool isA = false;
    bool isD = false;
    //回転速度
    float rotateSpeed = 10.0f;

    //どのステージに行くか
    public static bool firstStage = false;
    public static bool secondStage = false;

    //コントローラー入力を受け付けるか
    public static bool isInput = true;

    // Start is called before the first frame update
    void Start()
    {
        //戻ってきたら入力を受け付ける
        isInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        //水平方向の入力を取得し、それぞれの移動速度をかける。
        float Xvalue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float horizontalInput = Input.GetAxis("Horizontal");

        //現在のX,Zベクトルに上の処理で取得した値を渡す。
        Vector3 movedir = new Vector3(Xvalue, 0, 0);

        //移動ベクトルを設定
        move = new Vector3(horizontalInput, 0, 0);

        //移動関数
        Move(horizontalInput, movedir);

    }

    //移動関数
    void Move(float horizontalInput, Vector3 movedir)
    {
        //Dを押したら右へ移動
        if (Input.GetKey(KeyCode.D) && !isA && isInput || horizontalInput > 0 && isInput)
        {
            //Dボタンが押されているか
            isD = true;
            //現在地に上で取得をした値を足して移動する。
            transform.position += movedir;
            //進む方向に滑らかに向く。
            transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
            //アニメーションを変更。
            animator.SetBool("mode", true);
        }
        else
        {
            //何も押されてなければアニメーションを戻す
            animator.SetBool("mode", false);
            isD = false;
        }

        //Aを押したら左へ移動
        if (Input.GetKey(KeyCode.A) && !isD && isInput || horizontalInput < 0 && isInput)
        {
            //Dボタンが押されているか
            isA = true;
            //現在地に上で取得をした値を足して移動する。
            transform.position += movedir;
            //進む方向に滑らかに向く。
            transform.forward = Vector3.Slerp(transform.forward, movedir, Time.deltaTime * rotateSpeed);
            //アニメーションを変更。
            animator.SetBool("mode", true);
        }
        else
        {
            isA = false;
        }
    }

    //球体に当たっている時はそのステージに行ける
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "FirstStage")
        {
            firstStage = true;
        }

        if (other.gameObject.tag == "SecondStage")
        {
            secondStage = true;
        }

    }

    //球体に当たっていないときはステージに飛ばないようにする
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FirstStage")
        {
            firstStage = false;
        }

        if (other.gameObject.tag == "SecondStage")
        {
            secondStage = false;
        }

    }

}
