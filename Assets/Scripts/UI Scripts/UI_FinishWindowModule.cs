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
    [SerializeField] private float maxTime = 5f;
    [SerializeField] private float delay = 2f;
    [SerializeField] private UI_Windows winsHandling;

    /// <summary>
    /// call this function when level is completed.
    /// </summary>
    public void FinishLevel(int numberGems, int numberCoins)
    {
        winsHandling.OnFinished();

        StartCoroutine(CountingGems(numberGems));
        StartCoroutine(CountingCoins(numberCoins));
    }

    private IEnumerator CountingGems(int gems)
    {
        gemCounter.counter.text = PrefixBefore(1, 0);
        yield return new WaitForSecondsRealtime(delay);

        for(int i = 1; i <= gems; i++)
        {
            gemCounter.animator.SetTrigger("count");
            gemCounter.counter.text = PrefixBefore(2, i);
            yield return new WaitForSecondsRealtime(maxTime / gems);
        }
    }
    private IEnumerator CountingCoins(int coins)
    {
        coinsCounter.counter.text = PrefixBefore(1, 0);
        yield return new WaitForSecondsRealtime(delay);

        for (int i = 1; i <= coins; i++)
        {
            coinsCounter.animator.SetTrigger("count");
            coinsCounter.counter.text = PrefixBefore(2, i);
            yield return new WaitForSecondsRealtime(maxTime / coins);
        }
    }

    public static string PrefixBefore(int numbersOfPrefix, float number, char sign = '0')
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
