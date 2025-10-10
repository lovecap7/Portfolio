using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Status
{
    //基礎ステータス
    public int   m_jumpNum      = 0;     //ジャンプ回数
    public float m_jumpPower    = 0.0f;  //ジャンプ力
    public float m_groundSpeed  = 0.0f;  //地上移動速度
    public float m_swimSpeed    = 0.0f;  //水中移動速度
}
