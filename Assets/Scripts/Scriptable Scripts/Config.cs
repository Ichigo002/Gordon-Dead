using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Config : ScriptableObject
{
    [Range(0,1)]
    public float volume = .5f;
    [Range(0,1)]
    public float music = 1f;
}
