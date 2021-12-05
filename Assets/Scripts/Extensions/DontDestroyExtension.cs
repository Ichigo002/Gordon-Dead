using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyExtension : MonoBehaviour
{
    public static DontDestroyExtension instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
