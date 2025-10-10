using System;
using System.Collections.Generic;
using UnityEngine;
using static EvolutionItemManager;
using static PartData;

public class EvolutionItemManager : MonoBehaviour
{
    [Serializable]
    public struct PartItemData
    {
        public PartData.PartName partName;
        public Sprite sprite;
    }

    [Serializable]
    public struct EvelutionItemData
    {
        public PartData.EvoType evoType;
        public List<PartItemData> itemDatas;
    }
    [Serializable]
    public struct HumanEvelutionItemData
    {
        public List<PartItemData> itemDatas;
        public bool isDegenerate;
    }

    public struct SpawnInfo
    {
        public PartData.PartName partName;
        public PartData.EvoType evoType;
        public Vector3 pos;
        public bool isDegenerate;
    }

    [SerializeField]
    GameObject EvoItemPrefab = null;
    [SerializeField]
    List<EvelutionItemData> _evoItemDataTable = new List<EvelutionItemData>();
    [SerializeField]
    List<HumanEvelutionItemData> _humanEvoItemDataTable = new List<HumanEvelutionItemData>();

    List<GameObject> _evoItemObjectList = new List<GameObject>();
    static readonly int instanceMaxNum = 100;

    static readonly float spawnCameraDistance = 5.0f;

    static readonly float cameraHeightOffset = 5.0f;

    public Sprite GetSprite(SpawnInfo info)
    {
        if (info.evoType == EvoType.Normal)
        {
            foreach (var evoItemData in _humanEvoItemDataTable)
            {
                foreach (var itemData in evoItemData.itemDatas)
                {
                    if (itemData.partName == info.partName)
                    {
                        return itemData.sprite;
                    }
                }
            }
        }
        else
        {
            foreach (var evoItemData in _evoItemDataTable)
            {
                foreach (var itemData in evoItemData.itemDatas)
                {
                    if (evoItemData.evoType == info.evoType
                     && itemData.partName == info.partName)
                    {
                        return itemData.sprite;
                    }
                }
            }
        }

        return null;
    }

    public static Vector2 CalcItemSpawnPosition(Camera InCamera)
    {
        Camera camera = InCamera;
        if (camera == null)
        {
            camera = Camera.main;
        }

        Vector3 camPos = camera.transform.position;
        float camHeight = camera.orthographicSize * 2f;
        float camWidth = camHeight * camera.aspect;

        // カメラ右端
        float rightEdge = camPos.x + camWidth / 2f;
        rightEdge += spawnCameraDistance;
        float x = UnityEngine.Random.Range(rightEdge, rightEdge + camWidth);
        float y = UnityEngine.Random.Range((camPos.y - camHeight / 2f) + 3.0f, (camPos.y + camHeight / 2f) - cameraHeightOffset);
        return new Vector2(x, y);
    }

    void Awake()
    {
        for (int i = 0; i < instanceMaxNum; i++)
        {
            GameObject instanceObj = Instantiate(EvoItemPrefab);
            instanceObj.name = "eveItem" + i.ToString();
            instanceObj.SetActive(false);
            _evoItemObjectList.Add(instanceObj);
        }
    }

    public void SpawnEvoItem(SpawnInfo InSpawnInfo)
    {
        if (InSpawnInfo.evoType == EvoType.Normal)
        {
            foreach (var evoItemData in _humanEvoItemDataTable)
            {
                foreach (var itemData in evoItemData.itemDatas)
                {
                    if (itemData.partName == InSpawnInfo.partName)
                    {
                        SpawnEvoItem_Impl(itemData.sprite, InSpawnInfo);
                        return;
                    }
                }
            }
        }
        else
        {
            foreach (var evoItemData in _evoItemDataTable)
            {
                foreach (var itemData in evoItemData.itemDatas)
                {
                    if (evoItemData.evoType == InSpawnInfo.evoType
                     && itemData.partName == InSpawnInfo.partName)
                    {
                        SpawnEvoItem_Impl(itemData.sprite, InSpawnInfo);
                        return;
                    }
                }
            }
        }
    }

    public void DestoryEvoItem(GameObject InDestoryItem)
    {
        EvolutionItem item = InDestoryItem.GetComponent<EvolutionItem>();
        if (item == null)
        {
            return;
        }
        InDestoryItem.SetActive(false);
    }

    GameObject SpawnEvoItem_Impl(Sprite sprite, SpawnInfo spawnInfo)
    {
        foreach (var obj in _evoItemObjectList)
        {
            if (obj.activeSelf == false)
            {
                obj.transform.position = spawnInfo.pos;
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer)
                {
                    spriteRenderer.sprite = sprite;
                }

                EvolutionItem item = obj.GetComponent<EvolutionItem>();
                if (item)
                {
                    item.partName = spawnInfo.partName;
                    item.evoType  = spawnInfo.evoType;
                    item.isDegenerate = spawnInfo.isDegenerate;

                }

                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
}
