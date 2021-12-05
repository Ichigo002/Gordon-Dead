using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int gems = 0;
    private int evilCoins = 0;
    private int currentLvl = 1;

    private UI_PointsModule points;

    private void Awake()
    {
        points = GameObject.FindObjectOfType<UI_PointsModule>();
    }

    public void AddGem()
    {
        gems++;
        points.AddGem(gems);
    }
    public void AddEvilCoin()
    {
        evilCoins++;
        points.AddCoin(evilCoins);
    }
    public int GetEvilCoins()
    {
        return evilCoins;
    }
    public int GetGems()
    {
        return gems;
    }
    public int GetCurrentLvL()
    {
        return currentLvl;
    }
    public void SetCurrentLvL(int lvl)
    {
        currentLvl = lvl;
    }
}
