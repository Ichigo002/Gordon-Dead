using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevelScript : MonoBehaviour
{
    public SoundManager sndManager;
    private UI_FinishWindowModule clm;
    private PlayerStats playerSt;
    private GameObject[] allGems = new GameObject[1];
    private bool isCompleted = false;
    void Awake()
    {
        clm = GameObject.FindObjectOfType<UI_FinishWindowModule>();
        playerSt = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        allGems = GameObject.FindGameObjectsWithTag("Gem");
    }

    void Update()
    {
        if(playerSt.GetGems() >= allGems.Length && !isCompleted)
        {
            isCompleted = true;
            CompletedLevel();
        }
    }
    void CompletedLevel()
    {
        sndManager.PlaySound(0);
        clm.FinishLevel(playerSt.GetGems(), playerSt.GetEvilCoins());
    }
}
