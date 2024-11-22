using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public int playerWood;
    public int playerStone;
    public int playerMetal;
    public int playerFabric;

    public TMP_Text woodDisplay;
    public TMP_Text stoneDisplay;
    public TMP_Text metalDisplay;
    public TMP_Text fabricDisplay;



    // Update is called once per frame
    void Update()
    {
        woodDisplay.SetText(playerWood.ToString());
        fabricDisplay.SetText(playerFabric.ToString());
        metalDisplay.SetText(playerMetal.ToString());
        stoneDisplay.SetText(playerStone.ToString());
    }


}
