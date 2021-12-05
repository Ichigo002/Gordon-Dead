using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundsPlayer : MonoBehaviour
{
    public TableRandom[] table = new TableRandom[1];

    private SoundManager _audio;
    private int _noSound = 0;
    private int _noTableInSeq;
    private bool isPlaying;

    private void Awake()
    {
        _audio = GetComponent<SoundManager>();
    }
    private void Start()
    {
        if(table.Length == 0)
        {
            Debug.LogWarning("Table is empty");
        }
        if(_audio == null)
        {
            Debug.LogError(string.Format("Didn't found ManagerAudioScript in components this object"));
            Debug.LogError("This script must be in this same game object in which is ManagerAudioScript");
        }
    }

    /// <summary>
    ///  play random noise from chosen table.
    /// </summary>
    /// <param name="name">name of table</param>
    public void PlayTable(string name)
    {
        int t = CheckExistTable(name);
        if (t != -1)
        {
            if (!table[t].Random)
            {
               if(!isPlaying) StartCoroutine(PlayInSequency(t));
            }
            else
            {
                if (!isPlaying) StartCoroutine(PlayRandom(t));
            }
            
        }
    }

    private int CheckExistTable(string name)
    {
        for(int i = 0; i < table.Length; i++)
        {
            if(table[i].nameOfTable == name)
            {
                return i;
            }
        }
        Debug.LogError(string.Format("Table {0} doesn't exist in {1}", name, table));
        return -1;
    }

    IEnumerator PlayInSequency(int noTable)
    {
        if(_noTableInSeq != noTable)
        {
            _noTableInSeq = noTable;
            _noSound = 0;
        }
        _audio.PlaySound(table[noTable].NumbersOfSounds[_noSound]);
        isPlaying = true;
        yield return new WaitForSeconds(_audio.GetLengthCurrentSound() + table[noTable].timeDelay);
        isPlaying = false;

        if(_noSound == table[noTable].NumbersOfSounds.Length - 1)
        {
            _noSound = 0;
        }
        else
        {
            _noSound++;
        }
    }

    IEnumerator PlayRandom(int noTable)
    {
        if (_noTableInSeq != noTable)
        {
            _noTableInSeq = noTable;
            _noSound = 0;
        }
        _noSound = Random.Range(0, table[noTable].NumbersOfSounds.Length);
        _audio.PlaySound(table[noTable].NumbersOfSounds[_noSound]);
        isPlaying = true;
        yield return new WaitForSeconds(_audio.GetLengthCurrentSound() + table[noTable].timeDelay);
        isPlaying = false;
    }
}

[System.Serializable]
public class TableRandom
{
    public string nameOfTable;
    [Tooltip("If Random is disabled, then sounds are played in sequence")]
    public bool Random = true;
    [Tooltip("Delay between current sound and next clip. Delay can be negative, then it's played faster")]
    public float timeDelay = 0;
    [Space] [Tooltip("Type indexes of sounds from Manager Audio Source")]
    public int[] NumbersOfSounds = new int[1];
}
