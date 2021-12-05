using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public Sound[] sounds = new Sound[1];

    [SerializeField] private Config configFile;
    private AudioSource outputAudio;

    private void Start()
    {
        outputAudio = GetComponent<AudioSource>();

        if (sounds.Length == 0)
        {
            Debug.LogWarning("List of sounds is empty");
        }
        if (outputAudio == null)
        {
            Debug.LogError("output has no reference");
        }
        if(configFile == null)
        {
            Debug.LogError("Config file has no reference");
        }
    }
    private void OnEnable()
    {
        outputAudio = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Play chosen sound
    /// </summary>
    /// <param name="soundName">Name of chosen sound from list</param>
    public void PlaySound(string soundName)
    {
        int noSound = 0;
        if (CheckExistSound(soundName, ref noSound))
        {
            PlaySound(noSound);
        }
    }
    /// <summary>
    /// Play chosen sound
    /// </summary>
    /// <param name="noSound">Index of chosen sound from list</param>
    public void PlaySound(int noSound)
    {
        if (noSound >= 0 && noSound < sounds.Length)
        {
            SetSettings(noSound);
            outputAudio.Play();
        }
        else
        {
            Debug.LogError(string.Format("Index sound: {0} is out range", noSound));
        }
    }

    public float GetLengthCurrentSound()
    {
        return outputAudio.clip.length;
    }

    private bool CheckExistSound(string name, ref int noSound)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                noSound = i;
                return true;
            }
        }
        Debug.LogError(string.Format("Sound {0} doesn't exist in list {1}", name, sounds));
        noSound = -1;
        return false;
    }

    /// <summary>
    /// set settings for audio source
    /// </summary>
    /// <param name="noSound">number of sound from which get parameters</param>
    private void SetSettings(int noSound)
    {
        if(sounds[noSound].clip == null)
        {
            Debug.LogWarning("Clip is equals NULL");
        }
        if (sounds[noSound].pitch == 0)
        {
            Debug.LogWarning("Pitch is equals 0");
        }

        outputAudio.clip = sounds[noSound].clip;
        outputAudio.volume = sounds[noSound].volume * configFile.volume;
        outputAudio.pitch = sounds[noSound].pitch;
        outputAudio.priority = sounds[noSound].priority;
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 256)]
    public int priority = 128;
    [Range(0, 1)]
    public float volume = 1;
    [Range(-3, 3)]
    public float pitch = 1;
}
