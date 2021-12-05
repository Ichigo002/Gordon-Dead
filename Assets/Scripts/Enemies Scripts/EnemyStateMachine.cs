using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public string ENEMY_IDLE = "Idle";
    public string ENEMY_WALK = "Walk";
    public string ENEMY_PROTECT = "Shield";
    public string ENEMY_HIT = "Hit";
    public string ENEMY_ATTACK_1 = "Attack";
    public string ENEMY_ATTACK_2 = "Attack2";
    public string ENEMY_DEATH = "Death";

    private string currentStateAnim;
    [HideInInspector]
    public Animator anim;

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
}
