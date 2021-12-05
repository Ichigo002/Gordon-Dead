using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMessageScript : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string _message = "Example message";
    [SerializeField] private bool disposable = false;
    [Header("To disable timer you set its 0.")]
    [SerializeField] private float timeDisplay = 2.0f;

    private bool activated;
    private bool isPlayer;
    private UI_SpeechBalloonsScript speechBalloon;
    
    void Awake()
    {
        speechBalloon = GameObject.FindObjectOfType<UI_SpeechBalloonsScript>();
    }

    void Update()
    {
        if(isPlayer && !activated)
        {
            speechBalloon.AddBalloonToQueue(_message, timeDisplay);
          //  speechBalloon.SetBalloon(_message);
           // speechBalloon.SetStatus(true);
            if(disposable) { activated = true; }
            StartCoroutine(HideMsg());
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { isPlayer = true; }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { isPlayer = true; }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { isPlayer = false; }
       // if (timeDisplay == 0.0f || !disposable) { speechBalloon.SetStatus(false); }
    }
    IEnumerator HideMsg()
    {
        yield return new WaitForSeconds(timeDisplay);
       // if (timeDisplay != 0.0f && disposable) { speechBalloon.SetStatus(false); }
    }
}
