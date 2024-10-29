using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class playerScript : MonoBehaviour
{
    //シーンの名前
    public string nextSceneName;
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
    //ジャンプ
    Rigidbody rb;
    float jumpPower = 8.2f;
    bool isJump = false;
    bool isHitBlock = true;
    //待機モーション
    int waitTimer = 0;
    //体力系
    bool isHit = false;
    int coolTime = 0;
    int remainingHP = 3;
    public GameObject heartPrefab;
    private GameObject[] hearts;
    public Transform cameraTransform;
    //攻撃
    public bool isAttack = false;
    //落下
    public bool isFall = false;
    //ゴール
    public bool isGoal = false;
    //回復
    bool isHeel = false;

    //レイ用の宣言
    float rayDistance = 0.5f;
    private bool isBlock = true;

    //キー入力の宣言
    public static bool isInput = true;

    //Audio系の宣言
    public AudioSource healSE;
    public AudioSource swingSwordSE;
    public AudioSource jumpSE;

    //ポーズ画面用
    public static bool isPose = false;
    int backCoolTime = 0;

    //土管との判定
    public static bool isHitPipe = false;
    public static bool isPushPipe = false;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbodyを関連付ける
        rb = GetComponent<Rigidbody>();
        //ハート用
        hearts = new GameObject[remainingHP];
        CreateHearts();

        //一度土管に入ったら土管のポジションに変更する
        if(PipeScript.isTableScene && PipeScript.isSceneChange)
        {
            transform.position = new Vector3(11, -3.5f, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // カメラの位置に基づいてハートを更新
        Vector3 offset = new Vector3(-9, 4.5f, 10); // カメラからのオフセット

        if (!isPose)
        {
            //プレイ画面に戻れるようになるまでのクールタイム
            if (backCoolTime != 60)
            {
                backCoolTime++;
            }

            //ジャンプの処理
            Jump();

            //攻撃関数
            Attack();

            //ハートのポジション関数
            UpdateHeartPositions(offset);

            //待機アニメーション関数
            StartIdleAnimation();

            //敵と当たった時のダメージ処理
            ApplyDamageFromEnemy();

            //回復の関数
            HeelAction();

            //落下の関数
            Fall();

            //ポーズ画面を開く処理
            OpenPose();

        }

    }

    private void FixedUpdate()
    {
        //水平方向の入力を取得し、それぞれの移動速度をかける。
        float Xvalue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float horizontalInput = Input.GetAxis("Horizontal");

        //移動ベクトルを設定
        move = new Vector3(horizontalInput, 0, 0);

        //現在のX,Zベクトルに上の処理で取得した値を渡す。
        Vector3 movedir = new Vector3(Xvalue, 0, 0);

        if (!isPose)
        {
            //移動処理
            Move(movedir, horizontalInput);

        }
        //レイによる当たり判定
        RayHit();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            //ジャンプができる状態にする
            isJump = false;

            animator.SetBool("fall", false);
            animator.SetBool("jumpping", false);
            animator.SetBool("jump", false);
        }

        // すべての衝突接触点を取得
        foreach (ContactPoint contact in collision.contacts)
        {
            // 接触点の法線ベクトルがオブジェクトの上方向に近い場合、上からの衝突と見なす
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                if (collision.gameObject.tag == "Pipe")
                {
                    //ジャンプができる状態にする
                    isJump = false;

                    animator.SetBool("fall", false);
                    animator.SetBool("jumpping", false);
                    animator.SetBool("jump", false);

                    //土管に入れるようにする
                    isHitPipe = true;

                }
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            //ジャンプができる状態にする
            isJump = false;

            //アニメーションを変更。
            animator.SetBool("fall", false);

        }

        // すべての衝突接触点を取得
        foreach (ContactPoint contact in collision.contacts)
        {
            // 接触点の法線ベクトルがオブジェクトの上方向に近い場合、上からの衝突と見なす
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                if (collision.gameObject.tag == "Pipe")
                {
                    //ジャンプができる状態にする
                    isJump = false;

                    //アニメーションを変更。
                    animator.SetBool("fall", false);
                }
            }
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Pipe")
        {
            isHitPipe = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーと当たった時
        MashroomScript mashroom = other.gameObject.GetComponent<MashroomScript>();
        CactusScript cactus = other.GetComponentInParent<CactusScript>();

        //キノコに当たった時
        if (!isAttack && mashroom != null && !mashroom.isDead)
        {
            if (other.gameObject.tag == "Mashroom")
            {
                //当たった瞬間にtrueにする
                isHit = true;
            }
        }

        //サボテンに当たった時
        if (!isAttack && cactus != null && !cactus.isDead)
        {
            if (other.gameObject.CompareTag("Cactus"))
            {
                //当たった瞬間にtrueにする
                isHit = true;
            }
        }

        //お化けに当たった時
        if (other.gameObject.tag == "Gost")
        {
            isHit = true;
        }

        //ゴールした時
        if (other.gameObject.tag == "Goal")
        {
            isGoal = true;
        }

        //もし回復アイテムに当たったら
        if (other.gameObject.tag == "Heel")
        {
            isHeel = true;
        }

    }

    //ポーズを開く関数
    void OpenPose()
    {
        //特定のキーを押したらポーズ画面を開く
        if (Input.GetKeyDown(KeyCode.JoystickButton2) && !isPose && backCoolTime == 60 || Input.GetKeyDown(KeyCode.Escape) && !isPose && backCoolTime == 60)
        {
            //ポーズ画面を開く
            isPose = true;
            backCoolTime = 0;
            //アニメーションを変更。
            animator.SetBool("mode", false);
        }

    }

    //レイとの当たり判定
    void RayHit()
    {
        //プレイヤーの下方向へレイを出す
        Vector3 rayPosition = transform.position + new Vector3(0, 0.5f, 0);
        Ray ray = new Ray(rayPosition, Vector3.down);

        //レイを可視化する
        Debug.DrawRay(rayPosition, Vector3.down * rayDistance, Color.red);

        //レイが地面と当たっているかを判断する
        isBlock = Physics.Raycast(ray, rayDistance);

        if (isBlock)
        {
            //赤色のレイを表示する
            Debug.DrawRay(rayPosition, Vector3.down * rayDistance, Color.red);

            //重力無効
            rb.isKinematic = false;

            //攻撃アニメーションの状態を監視
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //Attackがアニメーションのステート名と一致する場合
            if (stateInfo.IsName("landing") && stateInfo.normalizedTime >= 0.5f)
            {
                animator.SetBool("landing", false);
            }

        }
        else
        {
            //青色の例を表示する
            Debug.DrawRay(rayPosition, Vector3.down * rayDistance, Color.blue);

            //ブロックにあたっていないことにする
            isHitBlock = false;

            //ジャンプができる状態にする
            isJump = true;

            //アニメーションを変更
            animator.SetBool("fall", true);

        }
    }

    //最初のハートを作る関数
    void CreateHearts()
    {
        for (int i = 0; i < remainingHP; i++)
        {
            GameObject heart = Instantiate(heartPrefab);
            hearts[i] = heart;
        }
    }

    //ハートのポジションを固定する関数
    void UpdateHeartPositions(Vector3 offset)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            Vector3 position = cameraTransform.position + offset + new Vector3(i * 1.5f, 0, 0);
            hearts[i].transform.position = position;
        }
    }

    //移動関数
    void Move(Vector3 movedir, float horizontalInput)
    {
        //Dを押したら右へ移動
        if (Input.GetKey(KeyCode.D) && !isA && isInput || horizontalInput > 0 && isInput)
        {
            //Dボタンが押されているか
            isD = true;
            isAttack = false;
            //現在地に上で取得をした値を足して移動する。
            transform.position += movedir;
            //プレイヤーの向きを変更
            transform.rotation = Quaternion.Euler(0, 90, 0);
            //アニメーションを変更。
            animator.SetBool("mode", true);
            //アニメーションを変更。
            animator.SetBool("attack", false);
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
            isAttack = false;
            //現在地に上で取得をした値を足して移動する。
            transform.position += movedir;
            //プレイヤーの向きを変更
            transform.rotation = Quaternion.Euler(0, -90, 0);
            //アニメーションを変更。
            animator.SetBool("mode", true);
            //アニメーションを変更。
            animator.SetBool("attack", false);
        }
        else
        {
            isA = false;
        }
    }

    //ジャンプの関数
    void Jump()
    {
        //スペース押したらジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && !isJump && isInput || Input.GetButtonDown("Fire1") && !isJump && isInput)
        {
            //上にジャンプさせる
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            //ジャンプしているかを判断(trueはジャンプしていると判断)
            isJump = true;
            isAttack = false;
            //アニメーションを変更。
            animator.SetBool("jump", true);
            //アニメーションを変更。
            animator.SetBool("attack", false);
            //ジャンプのSEを流す
            jumpSE.Play();
        }

        if (isJump && rb.velocity.y > 0)
        {
            //アニメーションを変更。
            animator.SetBool("jumpping", true);
        }

        if (isJump && rb.velocity.y < 0)
        {
            //アニメーションを変更。
            animator.SetBool("fall", true);
        }

    }

    //攻撃の関数
    void Attack()
    {
        //攻撃アニメーションの状態を監視
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //エンターもしくはBボタンを押したら攻撃
        if (Input.GetKeyDown(KeyCode.Return) && !isJump && !isAttack && !isD && !isA && isInput || Input.GetButtonDown("Fire2") && !isJump && !isAttack && !isD && !isA && isInput)
        {
            //アニメーションを変更。
            animator.SetBool("attack", true);
            isAttack = true;
            //1秒後にSEを再生するコルーチンを開始
            StartCoroutine(PlaySwingSound(0.4f));
        }

        //Attackがアニメーションのステート名と一致する場合
        if (isAttack && stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("attack", false);
            isAttack = false;
        }
    }

    //1秒後にSEを再生するコルーチン
    private IEnumerator PlaySwingSound(float delay)
    {
        //指定された秒数再生を待つ
        yield return new WaitForSeconds(delay);

        swingSwordSE.Play();

    }

    //待機アニメーション用の処理
    private void StartIdleAnimation()
    {
        //待機モーションに移行させるかどうか
        if (!isA && !isD && !isJump)
        {
            //どのくらい放置しているかをチェック
            waitTimer++;
            //1分放置していたらアニメーションさせる
            if (waitTimer >= 3600)
            {
                //アニメーションを変更。
                animator.SetBool("waitMotion", true);
            }
        }
        else
        {
            //アニメーションを変更。
            animator.SetBool("waitMotion", false);
            //放置時間をリセット
            waitTimer = 0;
        }
    }

    //回復の関数
    private void HeelAction()
    {
        //もしHPが3以下だったら回復
        if (isHeel && remainingHP <= 3)
        {
            remainingHP++;
            isHeel = false;
            healSE.Play();

            //ハートの表示個数を増やす
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < remainingHP)
                {
                    hearts[i].SetActive(true);
                    hearts[i].transform.rotation = hearts[0].transform.rotation;
                }
                else
                {
                    hearts[i].SetActive(false);
                }
            }

        }
        else if (isHeel && remainingHP > 3)
        {
            remainingHP = 3;
            isHeel = false;
        }
    }

    //敵と当たった時に体力を減らす処理
    private void ApplyDamageFromEnemy()
    {
        //敵にあたり残り体力が0じゃない時
        if (isHit && remainingHP > 0 && coolTime == 0)
        {
            //体力を減らす
            remainingHP--;
            //ハートの表示個数を減らす
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < remainingHP)
                {
                    hearts[i].SetActive(true);
                }
                else
                {
                    hearts[i].SetActive(false);
                }
            }
        }
        else if (isHit && remainingHP == 0)
        {
            isFall = true;
        }

        //体力が0じゃなければ
        if (isHit && remainingHP > 0)
        {
            //クールタイム時間を測る
            coolTime++;
        }

        if (coolTime == 60)
        {
            //無敵を解除
            isHit = false;
            //クールタイムをリセット
            coolTime = 0;
        }
    }

    //落下の関数
    void Fall()
    {
        //一定の位置よりも下に行けば死亡
        if (transform.position.y < -15.0f)
        {
            isFall = true;
        }
    }

}
