using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using mySystem.Maths;

public class Player : MonoBehaviour
{
    //進化した数
    public int m_totalEvoNum = 0;
    //自身のステータス
    public Status m_totalStatus;
    public Status m_baseStatus;
    //パーツ毎のステータス
    public Part m_head;
    public Part m_body;
    public Part m_leg;
    //移動量
    private float m_inputValueX;
    //歩きかどうか
    public bool m_isWalk;
    //Rigidbody2Dを取得
    Rigidbody2D m_rb;
    //ジャンプ
    int m_jumpNum = 0;
    //次のジャンプまでにかかる秒数
    const float kNextJumpTime = 1.0f;
    float m_nextJumpTime = 0.0f;
    //動きを有効かどうか
    public bool m_isActive;
    //順位
    public int m_rank = 0;

    [SerializeField] private SEController jumpSound;
    [SerializeField] private SEController evoHorce;
    [SerializeField] private SEController evoFish;
    [SerializeField] private SEController evoBird;
    [SerializeField] private SEController evoMonkey;

    //範囲外のオフセット
    const float kCameraOutOffset = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_isActive = false;
        m_isWalk = false;
        //初期化
        m_head.SetPartEvoType(PartData.EvoType.Normal);
        m_head.SetPartEvoLevel(PartData.EvoLevel.Monkey);
        m_body.SetPartEvoType(PartData.EvoType.Normal);
        m_body.SetPartEvoLevel(PartData.EvoLevel.Monkey);
        m_leg.SetPartEvoType(PartData.EvoType.Normal);
        m_leg.SetPartEvoLevel(PartData.EvoLevel.Monkey);
        //ステータスの合計
        CalcTotalStatus();
        //進化数の合計
        CalcTotalEvoNum();

        m_rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isActive) return;
        //部位の合計ステータスの更新
        CalcTotalStatus();
        //進化数の合計
        CalcTotalEvoNum();

        //死亡座標
        var deadPos = Camera.main.ViewportToWorldPoint(Vector2.zero);
        if (transform.position.x < deadPos.x - kCameraOutOffset || transform.position.y < deadPos.y - kCameraOutOffset)
        {
            Debug.Log("カメラの範囲外なので死亡");

            var rigitbody2D = this.gameObject.GetComponent<Rigidbody2D>();
            if (rigitbody2D)
            {
                rigitbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            OnDisablePlayer(); // 動作無効化

            GameOverManager.Instance.ShowGameOver(); // 自動でタイトルへ
        }
        //移動
        Vector2 move = new Vector2(m_inputValueX, 0.0f) * m_totalStatus.m_groundSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
        m_isWalk = true;
        //向きの更新
        if (m_inputValueX < 0.0f)
        {
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else if (m_inputValueX > 0.0f)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
        {
            m_isWalk = false;
        }
        if (kNextJumpTime > 0.0f)
        {
            m_nextJumpTime -= Time.deltaTime;
        }
    }


    // 有効化
    public void OnEnablePlayer()
    {
        m_isActive = true;
    }

    // 無効化
    public void OnDisablePlayer()
    {
        m_isActive = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!m_isActive) return;
        m_inputValueX = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!m_isActive) return;
        if (m_jumpNum >= m_totalStatus.m_jumpNum || m_nextJumpTime > 0.0f) return;
        Vector2 jump = new Vector2(0.0f, m_totalStatus.m_jumpPower);
        m_rb.AddForce(jump, ForceMode2D.Impulse);
        m_jumpNum++;
        m_nextJumpTime = kNextJumpTime;

        jumpSound.Play();
    }

    //ステータス合計
    private void CalcTotalStatus()
    {
        //ジャンプ回数
        m_totalStatus.m_jumpNum =
            m_baseStatus.m_jumpNum + m_head.m_status.m_jumpNum + m_body.m_status.m_jumpNum + m_leg.m_status.m_jumpNum;
        //ジャンプ力
        m_totalStatus.m_jumpPower =
            m_baseStatus.m_jumpPower + m_head.m_status.m_jumpPower + m_body.m_status.m_jumpPower + m_leg.m_status.m_jumpPower;
        //地上速度
        m_totalStatus.m_groundSpeed =
            m_baseStatus.m_groundSpeed + m_head.m_status.m_groundSpeed + m_body.m_status.m_groundSpeed + m_leg.m_status.m_groundSpeed;
        //水中速度
        m_totalStatus.m_swimSpeed =
            m_baseStatus.m_swimSpeed + m_head.m_status.m_swimSpeed + m_body.m_status.m_swimSpeed + m_leg.m_status.m_swimSpeed;
    }
    private void CalcTotalEvoNum()
    {
        //進化数
        m_totalEvoNum =
            m_head.m_evoNum + m_body.m_evoNum + m_leg.m_evoNum;
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            m_jumpNum = 0;
            m_nextJumpTime = 0;
            Debug.Log("ジャンプ可能");
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        var item = col.GetComponent<EvolutionItem>();
        if (item != null)
        {
            //場所とタイプとレベル取得
            var partName = item.partName;
            var evoType = item.evoType;
            var evoLevel = item.evoLevel;

            SEController playSE = evoMonkey;

            if (item.evoType == PartData.EvoType.Horse)
            {
                playSE = evoHorce;
            }
            else if (item.evoType == PartData.EvoType.Fish)
            {
                playSE = evoFish;
            }
            else if (item.evoType == PartData.EvoType.Bird)
            {
                playSE = evoBird;
            }

            switch (partName)
            {
                case PartData.PartName.Head:
                    if (item.evoType == PartData.EvoType.Normal)
                    {
                        if (item.isDegenerate)
                        {
                            m_head.AddPartEvoLevel();
                            playSE.Play();
                        }
                        else
                        {
                            m_head.SubPartEvoLevel();
                        }
                    }
                    else
                    {
                        m_head.SetPartEvoType(evoType);
                        playSE.Play();
                    }
                    break;
                case PartData.PartName.Body:
                    if (item.evoType == PartData.EvoType.Normal)
                    {
                        if (item.isDegenerate)
                        {
                            m_body.AddPartEvoLevel();
                            playSE.Play();
                        }
                        else
                        {
                            m_body.SubPartEvoLevel();
                        }
                    }
                    else
                    {
                        m_body.SetPartEvoType(evoType);
                        playSE.Play();
                    }
                    break;
                case PartData.PartName.Leg:
                    if (item.evoType == PartData.EvoType.Normal)
                    {
                        if (item.isDegenerate)
                        {
                            m_leg.AddPartEvoLevel();
                            playSE.Play();
                        }
                        else
                        {
                            m_leg.SubPartEvoLevel();
                        }
                    }
                    else
                    {
                        m_leg.SetPartEvoType(evoType);
                        playSE.Play();
                    }
                    break;
            }

            EvolutionItemManager evoItemManager = StageManager.Instance?.EvoItemManager;
            if (evoItemManager)
            {
                if (item.isFixedPlace)
                {
                    Destroy(item.gameObject);
                }
                else
                {
                    evoItemManager.DestoryEvoItem(item.gameObject);
                }
            }
        }
    }
    public List<Sprite> GetPartsSprite()
    {
        List<Sprite> parts = new();
        parts.Add(m_head.m_sprite);
        parts.Add(m_body.m_sprite);
        parts.Add(m_leg.m_sprite);
        return parts;
    }
    public Vector3 GetPos()
    {
        return transform.position;
    }
}

