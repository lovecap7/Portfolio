using UnityEngine;

public class EvolutionItem : MonoBehaviour
{
    //[HideInInspector]
    public PartData.PartName partName;

    //[HideInInspector]
    public PartData.EvoType evoType;
    
    //[HideInInspector]
    public PartData.EvoLevel evoLevel;

    // 退化するかどうか
    //[HideInInspector]
    public bool isDegenerate = false;

    public bool isFixedPlace = false;

    static readonly float _destoryTimer = 20.0f;
    float _timer = 0.0f;

    private void OnValidate()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            return;
        }
        if (!isFixedPlace)
        {
            return;
        }
        GameObject obj = GameObject.Find("PF_StageManager");
        EvolutionItemManager evo = obj.GetComponent<EvolutionItemManager>();

        EvolutionItemManager.SpawnInfo info = new EvolutionItemManager.SpawnInfo();
        info.partName = partName;
        info.evoType = evoType;
        info.isDegenerate = isDegenerate;
        Sprite sprite = evo?.GetSprite(info);
        if (sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    private void OnDisable()
    {
        _timer = 0.0f;
    }

    private void Update()
    {
        // 時間経過で破棄する
        if (_timer >= _destoryTimer && !isFixedPlace)
        {
            EvolutionItemManager evoItemManager = StageManager.Instance?.EvoItemManager;
            if (evoItemManager)
            {
                evoItemManager.DestoryEvoItem(gameObject);
            }
        }
        _timer += Time.deltaTime;
    }

}
