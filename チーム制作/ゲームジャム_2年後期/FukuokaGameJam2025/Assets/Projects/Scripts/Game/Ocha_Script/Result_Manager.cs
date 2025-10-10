using UnityEngine;
using UnityEngine.UI;
using mySystem;
using System.Collections;

public class ResultManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerGaugeSet
    {
        public GameObject gaugeObject;
        public RectTransform rankGauge;
        public RectTransform finishGauge;
        public RectTransform distanceGauge;
        
        public Image headImage;
        public Image bodyImage;
        public Image legImage;
    }

    public PlayerGaugeSet[] playerGauges = new PlayerGaugeSet[4];
    [Header("ゲージ設定")]
    private const float H = 80f;
    const float TOTAL = 450f;
    const float OVERLAP = 1f;
    const float RANK_S = 0.5f;      
    const float EVOLUTION_S = 1.0f; 
    const float HUMAN_S = 1.0f;

    [Header("アニメーション設定")]
    public float initialDelay = 2.0f;      // 最初の待ち時間
    public float gaugeAnimTime = 1.0f;     // 各ゲージの伸びる時間
    public float playerDelay = 0.2f;       // プレイヤー間の待ち時間

    void Start()
    {
        GameInstance gameInst = (GameInstance)GameInstanceBase.Instance;
        if (gameInst == null)
        {
            Debug.LogError("GameInstance が見つかりません");
            return;
        }

        // ゲージの表示/非表示設定
        for (int i = 0; i < playerGauges.Length; i++)
        {
            if (playerGauges[i].gaugeObject != null)
            {
                playerGauges[i].gaugeObject.SetActive(i < PlayerNum.playerNum);
                
                // ゲージを最初は非表示（背景だけ見える）
                if (i < PlayerNum.playerNum)
                {
                    if (playerGauges[i].rankGauge != null)
                        playerGauges[i].rankGauge.gameObject.SetActive(false);
                    if (playerGauges[i].finishGauge != null)
                        playerGauges[i].finishGauge.gameObject.SetActive(false);
                    if (playerGauges[i].distanceGauge != null)
                        playerGauges[i].distanceGauge.gameObject.SetActive(false);
                }
            }
        }

        // アニメーション開始
        StartCoroutine(AnimateGauges(gameInst));
    }

    IEnumerator AnimateGauges(GameInstance gameInst)
    {
        // 初期待機（この間は背景だけ見える）
        yield return new WaitForSeconds(initialDelay);

        // プレイヤーごとにアニメーション
        for (int i = 0; i < PlayerNum.playerNum; i++)
        {
            PlayerResultData data = gameInst.GetPlayerResultData(i + 1);
        
            int rankPoint = GetRankPoint(data.gloalRank);
            int evolutionPoint = data.evolution;
            float humanPoint = CalcHumanPoint(data);
        
            // ゲージをアニメーション
            yield return StartCoroutine(AnimateGauge(i, rankPoint, evolutionPoint, humanPoint));
            
            // 次のプレイヤーまで少し待つ
            yield return new WaitForSeconds(playerDelay);
        }
    }

    IEnumerator AnimateGauge(int idx, int rank, int evolution, float human)
    {
        var g = playerGauges[idx];
        if (g.rankGauge == null || g.finishGauge == null || g.distanceGauge == null) yield break;

        
        // プレイヤーのスプライトを表示
        GameInstance gameInst = (GameInstance)GameInstanceBase.Instance;
        if (gameInst != null)
        {
            PlayerResultData data = gameInst.GetPlayerResultData(idx + 1);
        
            // nullチェック削除してそのまま使う
            if (g.headImage != null) g.headImage.sprite = data.finalSpriteHead;
            if (g.bodyImage != null) g.bodyImage.sprite = data.finalSpriteBody;
            if (g.legImage != null) g.legImage.sprite = data.finalSpriteLeg;
        }
        
        // ゲージを表示
        g.rankGauge.gameObject.SetActive(true);
        g.finishGauge.gameObject.SetActive(true);
        g.distanceGauge.gameObject.SetActive(true);

        SetLeftAnchor(g.rankGauge);
        SetLeftAnchor(g.finishGauge);
        SetLeftAnchor(g.distanceGauge);

        // 最終的な幅を計算
        float rRaw = rank * RANK_S;
        float eRaw = evolution * EVOLUTION_S;
        float hRaw = human * HUMAN_S;
        float sumRaw = rRaw + eRaw + hRaw;

        float factor = (sumRaw > TOTAL && sumRaw > 0f) ? TOTAL / sumRaw : 1f;

        float wRankFinal = Mathf.Round(rRaw * factor);
        float wEvolutionFinal = Mathf.Round(eRaw * factor);
        float wHumanFinal = Mathf.Floor(Mathf.Max(0f, TOTAL - wRankFinal - wEvolutionFinal));

        // 初期状態（幅0）
        g.rankGauge.sizeDelta = new Vector2(0, H);
        g.finishGauge.sizeDelta = new Vector2(0, H);
        g.distanceGauge.sizeDelta = new Vector2(0, H);

        float elapsed = 0f;

        // アニメーション
        while (elapsed < gaugeAnimTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / gaugeAnimTime);
            
            // イージング（滑らかな動き）
            float eased = 1f - Mathf.Pow(1f - t, 3f);

            float wRank = wRankFinal * eased;
            float wEvolution = wEvolutionFinal * eased;
            float wHuman = wHumanFinal * eased;

            // ゲージ配置
            g.rankGauge.anchoredPosition = new Vector2(0, g.rankGauge.anchoredPosition.y);
            g.rankGauge.sizeDelta = new Vector2(wRank, H);

            g.finishGauge.anchoredPosition = new Vector2(wRank - OVERLAP, g.finishGauge.anchoredPosition.y);
            g.finishGauge.sizeDelta = new Vector2(wEvolution, H);

            g.distanceGauge.anchoredPosition = new Vector2(wRank + wEvolution - OVERLAP * 2f, g.distanceGauge.anchoredPosition.y);
            g.distanceGauge.sizeDelta = new Vector2(wHuman, H);

            yield return null;
        }

        // 最終的な値を確実に設定
        g.rankGauge.sizeDelta = new Vector2(wRankFinal, H);
        g.finishGauge.anchoredPosition = new Vector2(wRankFinal - OVERLAP, g.finishGauge.anchoredPosition.y);
        g.finishGauge.sizeDelta = new Vector2(wEvolutionFinal, H);
        g.distanceGauge.anchoredPosition = new Vector2(wRankFinal + wEvolutionFinal - OVERLAP * 2f, g.distanceGauge.anchoredPosition.y);
        g.distanceGauge.sizeDelta = new Vector2(wHumanFinal, H);
    }

    float CalcHumanPoint(PlayerResultData data)
    {
        float point = 0.0f;
        
        if (data.finalSpriteHead != null && IsHumanSprite(data.finalSpriteHead))
            point += 33.33f;
        if (data.finalSpriteBody != null && IsHumanSprite(data.finalSpriteBody))
            point += 33.33f;
        if (data.finalSpriteLeg != null && IsHumanSprite(data.finalSpriteLeg))
            point += 33.34f;
        
        return point;
    }

    bool IsHumanSprite(Sprite sprite)
    {
        return !sprite.name.Contains("Monkey");
    }

    int GetRankPoint(int rank)
    {
        switch (rank)
        {
            case 1: return 100;
            case 2: return 80;
            case 3: return 60;
            case 4: return 40;
            default: return 0;
        }
    }

    void SetLeftAnchor(RectTransform rt)
    {
        rt.anchorMin = new Vector2(0f, 0.5f);
        rt.anchorMax = new Vector2(0f, 0.5f);
        rt.pivot = new Vector2(0f, 0.5f);
    }
}