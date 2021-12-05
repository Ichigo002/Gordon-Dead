using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    private IDiePlayer[] scripts;

    private PlayerStateMachine plSM;
    private SoundManager plSounds;

    void Awake()
    {
        scripts = GetComponents<IDiePlayer>();
        plSM = GetComponent<PlayerStateMachine>();
        plSounds = GetComponent<SoundManager>();
    }

    void ExecuteDieProcess()
    {
        foreach (var item in scripts)
        {
            item.PlayerDead(false);
        }
    }
    public void DiePlayer()
    {
        ExecuteDieProcess();
        plSM.ChangeAnimationState(plSM.PLAYER_DEATH);
        plSounds.PlaySound(0);
    }
    public void WakePlayer()
    {
        foreach (var item in scripts)
        {
            item.PlayerDead(true);
        }
    }
}