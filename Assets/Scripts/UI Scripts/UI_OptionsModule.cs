using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OptionsModule : MonoBehaviour
{
    [SerializeField] private Slider sld_volume;
    [SerializeField] private Slider sld_music;
    [Space]
    [SerializeField] private Config configFile;

    private void OnEnable()
    {
        LoadSettings(configFile);
    }

    void LoadSettings(Config con)
    {
        sld_volume.value = con.volume;
        sld_music.value = con.music;
    }
    void SaveSettings(ref Config con)
    {
        con.volume = sld_volume.value;
        con.music = sld_music.value;
    }
    public void OnValueChanged()
    {
        SaveSettings(ref configFile);
    }
}
