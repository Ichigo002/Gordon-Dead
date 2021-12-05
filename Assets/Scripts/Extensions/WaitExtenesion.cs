using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class WaitExtenesion
{
    public static void Wait(this MonoBehaviour mono, float delay, UnityAction action)
    {
        mono.StartCoroutine(ExecuteAction(delay, action));
    }

    public static IEnumerator ExecuteAction(float delay, UnityAction action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
        yield break;
    }
}
