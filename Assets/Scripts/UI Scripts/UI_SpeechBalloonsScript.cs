using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpeechBalloonsScript : MonoBehaviour
{
    public class balloon
    {
        public balloon(string txt, float timeShow)
        {
            this.txt = txt;
            this.timeShow = timeShow;
        }
        public string txt;
        public float timeShow;
    }

    private Queue<balloon> balloons = new Queue<balloon>();
    private Animator anim;
    private Text txt;
    private bool next = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        txt = GetComponentInChildren<Text>();
    }
    private void Update()
    {
        // display msg
        if(next && balloons.Count != 0)
        {
            // get first element of balloons
            balloon bn = balloons.Peek();
            SetBalloonMsg(bn.txt);
            StartCoroutine(ShowMsg(bn));
        }
    }
    
    /// <summary>
    /// Show message, If in queue is another obj. Then will be showed when previous 
    /// msg will be hide.
    /// </summary>
    /// <param name="_text">message in speech balloon</param>
    /// <param name="timeShowing"> within time msg is showing</param>
    public void AddBalloonToQueue(string _text, float timeShowing)
    {
        balloons.Enqueue(new balloon(_text, timeShowing));
    }
    /// <summary>
    /// Show message, If in queue is another obj. Then will be showed when previous 
    /// msg will be hide.
    /// </summary>
    /// <param name="_text">message in speech balloon</param>
    /// <param name="isShowed">set status of current msg</param>
    public void InstantBalloon(string _text, bool isShowed)
    {
        SetBalloonMsg(_text);
        SetStatus(isShowed);
    }

    IEnumerator ShowMsg(balloon bn)
    {
        SetStatus(true);
        next = false;
        yield return new WaitForSeconds(bn.timeShow);

        balloons.Dequeue();
        if (balloons.Count == 0)
            SetStatus(false);
        next = true;
    }

    private void SetBalloonMsg(string _text)
    {
        txt.text = _text;
    }

    private void SetStatus(bool enabled)
    {
        anim.SetBool("Showed", enabled);
    }
}
