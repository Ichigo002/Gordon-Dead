using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_FinishWindowModule : MonoBehaviour
{
    [System.Serializable]
    public struct infoCounter
    {
        public TMP_Text counter;
        public Animator animator;
    }

    [SerializeField] private infoCounter gemCounter;
    [SerializeField] private infoCounter coinsCounter;
    [Space]
    [SerializeField] private float timeCounting = .5f;
    [SerializeField] private UI_Windows winsHandling;

    /// <summary>
    /// call this function when level is completed.
    /// </summary>
    public void FinishLevel(int numberGems, int numberCoins)
    {
        StartCoroutine(DisplayPoints(numberGems, numberCoins));
    }

    private IEnumerator DisplayPoints(int gems, int coins)
    {
        gemCounter.counter.text = InsertZeroBefore(1, 0);
        coinsCounter.counter.text = InsertZeroBefore(1, 0);

        winsHandling.OnFinished();
        yield return new WaitForSecondsRealtime(2f);

        int g = 0, c = 0;

        for(int i = 0; i < (gems > coins ? gems : coins); i++)
        {
            if (i < gems)
            {
                g++;
                gemCounter.animator.SetTrigger("count");
                gemCounter.counter.text = InsertZeroBefore(2, g);
            }
            if (i < coins)
            {
                c++;
                coinsCounter.animator.SetTrigger("count");
                coinsCounter.counter.text = InsertZeroBefore(2, c);
            }

            yield return new WaitForSecondsRealtime(timeCounting);
            continue;
        }
    }

    private string InsertZeroBefore(int numbersOfPrefix, float number, char sign = '0')
    {
        string result = "";

        for(int i = 0; i < numbersOfPrefix; i++)
        {
            // check is number lower than addition of 10
            if (number < Mathf.Pow(10, i))
            {
                result += sign + "";
            }
        }
        return result + number + "";
    }

    /// <summary>
    /// function is called when player clicks Menu button
    /// </summary>
    public void GotoMenu()
    {
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// function is called when player clicks Next LvL button
    /// </summary>
    public void NextLevel()
    {

    }

}
