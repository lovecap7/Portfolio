using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconList : MonoBehaviour
{
    [SerializeField] private Sprite iconMemberNum_2;
    [SerializeField] private Sprite iconMemberNum_3;
    [SerializeField] private Sprite iconMemberNum_4;

    public static int playerNum = 0;

    private Image targetImage;
    private Sprite[] iconList = new Sprite[3];
    private int iconCount = 0;

    const int PLAYER_NUM_SELECT_NUM_DIFF = 2;   //IdxÇ∆é¿ç€ÇÃêlêîÇÃç∑ï™
    const int PLAYER_NUM_MAX = 3;   //IdxÇ∆é¿ç€ÇÃêlêîÇÃç∑ï™

    private void Awake()
    {
        iconList[0] = iconMemberNum_2;
        iconList[1] = iconMemberNum_3;
        iconList[2] = iconMemberNum_4;

        targetImage = GetComponent<Image>();
        targetImage.sprite = iconList[0];
    }

    public void TrgDownLeft()
    {
        iconCount--;
        if (iconCount < 0) iconCount = PLAYER_NUM_MAX - 1;

        targetImage.sprite = iconList[iconCount];
        playerNum = iconCount + PLAYER_NUM_SELECT_NUM_DIFF;
    }

    public void TrgDownRight()
    {
        iconCount++;
        if (iconCount >= PLAYER_NUM_MAX) iconCount = 0;

        targetImage.sprite = iconList[iconCount];
        playerNum = iconCount + PLAYER_NUM_SELECT_NUM_DIFF;
    }
}
