using mySystem;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerResultData
{
    // ゴール時のスプライト
    public Sprite finalSpriteHead;
    public Sprite finalSpriteBody;
    public Sprite finalSpriteLeg;

    // ゴール順
    public int gloalRank;

    // 進化回数
    public int evolution;
}

public class GameInstance : GameInstanceBase
{
    // Key   : プレイヤー判別 Index(1P, 2P)
    // Value : リザルトデータ 
    Dictionary<int, PlayerResultData> _playerResultDataList = new Dictionary<int, PlayerResultData>();

    public void AddPlayerResultData(int InPlayerIndex, PlayerResultData InData)
    {
        _playerResultDataList.Add(InPlayerIndex, InData);
    }

    public void ResetPlayerResultData()
    {
        _playerResultDataList.Clear();
    }

    public PlayerResultData GetPlayerResultData(int InPlayerIndex)
    {
        if (_playerResultDataList.ContainsKey(InPlayerIndex))
        {
            return _playerResultDataList[InPlayerIndex];
        }
        return new PlayerResultData();
    }
}
