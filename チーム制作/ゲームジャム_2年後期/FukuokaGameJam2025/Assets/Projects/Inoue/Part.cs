using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PartData;
using static UnityEngine.Rendering.DebugUI;

public class Part : MonoBehaviour
{
    //ステータス
    public Status m_status;
    //進化レベル
    public PartData.EvoLevel m_evoLevel = PartData.EvoLevel.Monkey;
    //タイプ
    public PartData.EvoType m_evoType = PartData.EvoType.Normal;
    //通常以外の場合のタイマー
    private const int kEvoTime = 6;
    private float m_evoTime;
    //進化した数
    public int m_evoNum = 0;
    //現在のスプライト
    public Sprite m_sprite;
    //親
    private Player m_parent;

    [Serializable]
    public struct PartDataList
    {
        public PartData.EvoType evoType;
        public PartData.EvoLevel evoLevel;
        public Sprite sprite;
        public Status status;
    }
    //リスト
    public List<PartDataList> m_partDataList = new();
    //アニメーション
    public Animator m_animator;
    //パーツの名前
    public PartData.PartName m_partName;

    private void Start()
    {
        SetPartEvoType(m_evoType);
        SetPartEvoLevel(m_evoLevel);
        //親オブジェクトを取得
        m_parent = transform.root.gameObject.GetComponent<Player>();
    }

    void Update()
    {
        if(m_parent != null)
        {
            if (!m_parent.m_isActive) return;
        }
        if(m_partName != PartData.PartName.Head)
        {
            m_animator.SetBool("Walk", m_parent.m_isWalk);
        }
        
        //進化が切れたら通常に戻る
        if (m_evoTime > 0.0f)
        {
            m_evoTime -= Time.deltaTime;
        }
        else
        {
            SetPartEvoType(PartData.EvoType.Normal);
            SetPartEvoLevel(m_evoLevel);
        }
    }

    //パーツ
    public void SetPartEvoType(PartData.EvoType evoType)
    {
        m_evoType = evoType;
        foreach (PartDataList data in m_partDataList)
        {
            //一致するものがあったらセット
            if (data.evoType == m_evoType)
            {
                //普通の進化以外
                if (m_evoType != PartData.EvoType.Normal)
                {
                    m_evoTime = kEvoTime;
                    m_sprite = data.sprite;
                    gameObject.GetComponent<SpriteRenderer>().sprite = data.sprite;
                    m_status = data.status;
                    //アニメーション
                    m_animator.SetInteger("AnimState", GetAnimIndex());
                    //進化数カウント
                    ++m_evoNum;
                }
                else
                {
                    //一致するものがあったらセット
                    if (data.evoLevel == m_evoLevel)
                    {
                        m_sprite = data.sprite;
                        gameObject.GetComponent<SpriteRenderer>().sprite = data.sprite;
                        m_status = data.status;
                        //アニメーション
                        m_animator.SetInteger("AnimState", GetAnimIndex());
                    }
                }
                break;
            }
        }
    }
    public void SetPartEvoLevel(PartData.EvoLevel evoLevel)
    {
        var beforeLevel = m_evoLevel;
        m_evoLevel = (PartData.EvoLevel)Mathf.Clamp((int)evoLevel, (int)PartData.EvoLevel.Monkey, (int)PartData.EvoLevel.Human);
        foreach (PartDataList data in m_partDataList)
        {
            //普通の進化
            if (m_evoType == PartData.EvoType.Normal)
            {
                //一致するものがあったらセット
                if (data.evoLevel == m_evoLevel)
                {
                    m_sprite = data.sprite;
                    gameObject.GetComponent<SpriteRenderer>().sprite = data.sprite;
                    m_status = data.status;
                    //アニメーション
                    m_animator.SetInteger("AnimState", GetAnimIndex());
                    //レベルが上がったら
                    if(beforeLevel < evoLevel)
                    {
                        ++m_evoNum;
                    }
                    break;
                }
            }
        }
    }
    public void AddPartEvoLevel()
    {
        int level = (int)m_evoLevel + 1;
        m_evoLevel = (PartData.EvoLevel)Mathf.Clamp(level, (int)PartData.EvoLevel.Monkey, (int)PartData.EvoLevel.Human);
        foreach (PartDataList data in m_partDataList)
        {
            //普通の進化
            if (m_evoType == PartData.EvoType.Normal)
            {
                //一致するものがあったらセット
                if (data.evoLevel == m_evoLevel)
                {
                    m_sprite = data.sprite;
                    gameObject.GetComponent<SpriteRenderer>().sprite = data.sprite;
                    m_status = data.status;
                    //アニメーション
                    m_animator.SetInteger("AnimState", GetAnimIndex());
                    //進化数カウント
                    ++m_evoNum;
                    break;
                }
            }
        }
    }
    public void SubPartEvoLevel()
    {
        int level = (int)m_evoLevel - 1;
        m_evoLevel = (PartData.EvoLevel)Mathf.Clamp(level, (int)PartData.EvoLevel.Monkey, (int)PartData.EvoLevel.Human);
        foreach (PartDataList data in m_partDataList)
        {
            //普通の進化
            if (m_evoType == PartData.EvoType.Normal)
            {
                //一致するものがあったらセット
                if (data.evoLevel == m_evoLevel)
                {
                    m_sprite = data.sprite;
                    gameObject.GetComponent<SpriteRenderer>().sprite = data.sprite;
                    m_status = data.status;
                    //アニメーション
                    m_animator.SetInteger("AnimState", GetAnimIndex());
                    break;
                }
            }
        }
    }
    private int GetAnimIndex()
    {
        //普通の状態
        if(m_evoType == PartData.EvoType.Normal)
        {
            if (m_evoLevel == PartData.EvoLevel.Monkey)
            {
                return 1;
            }
            else if (m_evoLevel == PartData.EvoLevel.Middle)
            {
                return 2;
            }
            else if (m_evoLevel == PartData.EvoLevel.Human)
            {
                return 3;
            }
        }
        else
        {
            if (m_evoType == PartData.EvoType.Bird)
            {
                return 4;
            }
            else if (m_evoType == PartData.EvoType.Fish)
            {
                return 5;
            }
            else if (m_evoType == PartData.EvoType.Horse)
            {
                return 6;
            }
        }
       
        return 0;
    }
}
