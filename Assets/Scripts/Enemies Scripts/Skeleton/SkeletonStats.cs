using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStats : MonoBehaviour
{
    public float health = 60f;
    public float dealtDamage = 10f;
    public float knockback = 7f;
    public int numberOfCoins = 25;
    [SerializeField] Rigidbody2D rb2d;

    private EnemyStateMachine stateMach;
    private ManagerPOScript poolObject;

    void Awake()
    {
        stateMach = GetComponent<EnemyStateMachine>();
        poolObject = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<ManagerPOScript>();
        if (!poolObject)
        {
            Debug.LogError("not found ManagerPOScript! in the scene");
        }
    }

    /// <summary>
    /// Enemy dies
    /// </summary>
    public void Die()
    {
        stateMach.ChangeAnimationState(stateMach.ENEMY_DEATH);
        for(int i = 0; i < numberOfCoins; i++)
        {
            poolObject.CreateEvilcoin(new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y, transform.position.y + 1.5f)), Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
        rb2d.gravityScale = 0;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SkeletonAI>().enabled = false;
        GetComponent<SkeletonStats>().enabled = false;
        GetComponent<SkeletonCollision>().enabled = false;
    }
}
