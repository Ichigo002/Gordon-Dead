using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevelScript : MonoBehaviour
{
    public SoundManager sndManager;
    private PlayerStats playerSt;
    private GameObject[] allGems = new GameObject[1];
    private bool isCompleted = false;
    void Awake()
    {
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
      //  FindObjectOfType<UI_FinishWindowModule>().FinishLevel(playerSt.GetGems(), playerSt.GetEvilCoins());
        FindObjectOfType<UI_FinishWindowModule>().FinishLevel(30, 120);
    }
}
