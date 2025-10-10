using mySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
    [SerializeField]
    float spawnIntervalTime = 5.0f;

    [SerializeField]
    int spawnMinNum = 2;
    [SerializeField]
    int spawnMaxNum = 10;

    EvolutionItemManager _evolutionItemManager = null;
    float _spawnTimer = 0.0f;

    public EvolutionItemManager EvoItemManager { get { return _evolutionItemManager; } }


    void UpdateEvoItem()
    {
        if (_spawnTimer >= spawnIntervalTime)
        {
            // ランダムな個数生成する
            int spawnNum = Random.Range(spawnMinNum, spawnMaxNum);
            for (int i = 0; i < spawnNum; i++)
            {
                int evoType = Random.Range((int)PartData.EvoType.Normal, (int)PartData.EvoType.Max);
                int partType = Random.Range((int)PartData.PartName.Head, (int)PartData.PartName.Max);

                Vector2 pos = EvolutionItemManager.CalcItemSpawnPosition(Camera.main);

                EvolutionItemManager.SpawnInfo spawnInfo;
                spawnInfo.evoType = (PartData.EvoType)evoType;
                spawnInfo.isDegenerate = false;
                if (spawnInfo.evoType == PartData.EvoType.Normal)
                {
                    // 人間であれば進化or退化をランダムで生成する
                    int rand = Random.Range(0, 10);
                    spawnInfo.isDegenerate = (rand <= 6) ? false : true;
                }
                spawnInfo.partName = (PartData.PartName)partType;
                spawnInfo.pos = pos;
                _evolutionItemManager?.SpawnEvoItem(spawnInfo);
            }

            _spawnTimer = 0.0f;
        }
        else
        {
            _spawnTimer += Time.deltaTime;
        }

    }

    #region Unity Methots
    // Start is called before the first frame update
    void OnEnable()
    {
        _evolutionItemManager = GetComponent<EvolutionItemManager>();
        _spawnTimer = 0.0f;
    }

    private void OnDisable()
    {
        _evolutionItemManager = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEvoItem();
    }
    #endregion
}
