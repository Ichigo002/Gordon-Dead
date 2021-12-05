using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelDoorScript : MonoBehaviour
{
    [Tooltip("This number of level will be loaded when Door is activated")]
    [SerializeField] private int loadScene = 0;
    [SerializeField] private int neededLevel = 0;
    [Header("In place {0} is displayed current needed level")]
    [TextArea]
    public string _info = "Press E to load {0} level";

    private Light2D[] lights = new Light2D[2];
    private bool isPlayer;
    private UI_SpeechBalloonsScript speechBalloon;
    private PlayerStats player;

    void Start()
    {
        speechBalloon = GameObject.FindObjectOfType<UI_SpeechBalloonsScript>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        lights = GetComponentsInChildren<Light2D>();

        if(player.GetCurrentLvL() >= neededLevel)
        { SwitchLight(true); } 
        else { SwitchLight(false); }
    }

    void Update()
    {
        if (Input.GetButtonDown("Interaction") && isPlayer)
        {
            isPlayer = false;
            speechBalloon.InstantBalloon(string.Format(_info,neededLevel), false);
            SceneManager.LoadScene(loadScene);
        }

        if (player.GetCurrentLvL() >= neededLevel)
        { SwitchLight(true); } else { SwitchLight(false); }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player.GetCurrentLvL() >= neededLevel)
        {
            speechBalloon.InstantBalloon(string.Format(_info, neededLevel), true);
            isPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            speechBalloon.InstantBalloon(string.Format(_info, neededLevel), false);
            isPlayer = false;
        }
    }

    void SwitchLight(bool active)
    {
        foreach(var light in lights)
        {
            if(light.enabled != active)
                light.enabled = active;
        }
    }
}
