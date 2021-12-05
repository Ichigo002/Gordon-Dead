using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Player's animations")]
    public string PLAYER_IDLE = "Idle";
    public string PLAYER_RUN = "Run";
    public string PLAYER_JUMP_UP = "Jump_Up";
    public string PLAYER_JUMP_DOWN = "Jump_down";
    public string PLAYER_HIT = "Hit";
    public string PLAYER_ATTACK_1 = "Attack1";
    public string PLAYER_ATTACK_2 = "Attack2";
    public string PLAYER_DEATH = "Death";

    private Animator anim;
    private string currentStateAnim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// manager animation
    /// </summary>
    /// <param name="newAnim"></param>
    public void ChangeAnimationState(string newState)
    {
        if (currentStateAnim == newState) return;

        anim.Play(newState);
        currentStateAnim = newState;
    }

    public float GetLengthCurrentClip()
    {
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == currentStateAnim)        //If it has the same name as your clip
            {
                return ac.animationClips[i].length;
            }
        }
        Debug.LogError("current state info is null OR current playing animation doesn't exist");
        return 0;
    }

    public string GetCurrentState()
    {
        return currentStateAnim;
    }
}
