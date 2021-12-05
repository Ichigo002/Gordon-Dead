using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public float walkSpeed = 3;
    public float attackDistance = 2.0f;
    public float _fallMultiplier = 2.5f;
    public LayerMask groundMask;

    public delegate void AttackEnemy(float damage);
    public static event AttackEnemy GetAttackEnemy;

    private SkeletonStats stats;
    private SkeletonCollision collisionScript;
    private EnemyStateMachine stateMach;
    private Rigidbody2D rb;
    private GameObject player;
    private StatesEnemy state = StatesEnemy.Guarding;
    private Vector2 velocity;
    private float spaceEnemy;
    private float gotDamage;
    [HideInInspector]
    public bool isAttacking, isProtecting, isHitting, isDying;
    //private bool isStrikingByPl;
    public bool movingInLeft;
    private int noAttack = 0;

    public enum StatesEnemy
    {
        Guarding, Attacking, Protecting, Dying, Hitting, Idle
    }

    private void Awake()
    {
        stats = GetComponent<SkeletonStats>();
        stateMach = GetComponent<EnemyStateMachine>();
        collisionScript = GetComponent<SkeletonCollision>();
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        PlayerAttack.InSightAttackEvent += InRangeAttackPlayer;
        rb.simulated = true;
        isDying = false;

    }

    private void OnDisable()
    {
        PlayerAttack.InSightAttackEvent -= InRangeAttackPlayer;
    }

    private void FixedUpdate()
    {
        if (movingInLeft)
        { spaceEnemy = -1f; } 
        else
        { spaceEnemy = 1f; }
        //==========================================
        //             Calling states     
        //==========================================
        //Default state
        if(!isHitting && !isAttacking && !isProtecting)
            state = StatesEnemy.Guarding;

        //check if player is in attack's range
        if (Vector2.Distance(transform.position, player.transform.position) < attackDistance && !isHitting &&
                   ((player.transform.position.x < transform.position.x && movingInLeft) ||
                   (player.transform.position.x > transform.position.x && !movingInLeft))) 
        {
            if (player.transform.position.y >= transform.position.y - 0.78f && 
                player.transform.position.y <= transform.position.y + 1.72f)
            {
                state = StatesEnemy.Attacking;
            }
            
        }
        // protecting before player or hitting by player
        if (isHitting)
        {
            if(Random.Range(-13, -1) < 0)
            {
                state = StatesEnemy.Protecting;
            }
            else
            {
                state = StatesEnemy.Hitting;
            }
        }
        // dying
        if (stats.health <= 0)
        {
            state = StatesEnemy.Dying;
        }
        //==========================================
        switch (state)
        {
            // Enemy is patroling platfrom
            case StatesEnemy.Guarding:
                RaycastHit2D groundInfo = Physics2D.Raycast(new Vector2(transform.position.x + spaceEnemy, transform.position.y), Vector2.down, 1.2f, groundMask);
                if (groundInfo)
                {
                    Move(movingInLeft);
                }
                else
                {
                    movingInLeft = !movingInLeft;
                }
                break;
            // Enemy is attacking player
            case StatesEnemy.Attacking:
                if (!isAttacking)
                {
                    if (noAttack > 2)
                    {
                        StartCoroutine(Attack(stateMach.ENEMY_ATTACK_2));
                        noAttack = 0;
                    }
                    else
                    {
                        StartCoroutine(Attack(stateMach.ENEMY_ATTACK_1));
                        noAttack += 1;
                    }
                }
                break;
            // Enemy is striking by player
            case StatesEnemy.Hitting:
                if (!isHitting)
                {
                    collisionScript.OnHit(gotDamage);
                }
                break;
            // Using shield. Enemy is protecting itself
            case StatesEnemy.Protecting:
                if (!isProtecting)
                {
                    StartCoroutine(UseShield());
                }
                break;
            // Enemy is dying
            case StatesEnemy.Dying:
                isDying = true;
                stats.Die();
                break;

            case StatesEnemy.Idle:
                stateMach.ChangeAnimationState(stateMach.ENEMY_IDLE);
                break;
        }

        // block moving during hitting
        rb.simulated = !isHitting;
    }

    /// <summary>
    /// attack Player by Enemy
    /// </summary>
    IEnumerator Attack(string anim)
    {
        stateMach.ChangeAnimationState(anim);
        isAttacking = true;
        yield return new WaitForSeconds(stateMach.anim.GetCurrentAnimatorClipInfo(0).Length);
        isAttacking = false;

        // giving damage for Player
        if (Vector2.Distance(transform.position, player.transform.position) < attackDistance && !isHitting && !isProtecting
             && ((player.transform.position.x < transform.position.x && movingInLeft) ||
                   (player.transform.position.x > transform.position.x && !movingInLeft)) && !isDying)
        {
            if (GetAttackEnemy != null) { GetAttackEnemy(stats.dealtDamage); }
        }
    }

    /// <summary>
    /// protect Enemy before player's attacks
    /// </summary>
    IEnumerator UseShield()
    {
        stateMach.ChangeAnimationState(stateMach.ENEMY_PROTECT);
        isProtecting = true;
        yield return new WaitForSeconds(stateMach.anim.GetCurrentAnimatorClipInfo(0).Length);
        isProtecting = false;
    }
    /// <summary>
    /// moving enemy in left or right directions
    /// </summary>
    void Move(bool isGoingLeft)
    {
        velocity = new Vector2(0, 0);
        stateMach.ChangeAnimationState(stateMach.ENEMY_WALK);
        if (isGoingLeft)
        {
            velocity.x = -walkSpeed;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            velocity.x = walkSpeed;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        // falling
        if (rb.velocity.y < 0)
        {
            velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }

        //assign velocity
        rb.velocity = velocity;
    }

    void InRangeAttackPlayer(float sight, float damage, Vector3 posPlayer, bool directionLeft)
    {
        if (Vector2.Distance(posPlayer, transform.position) <= sight && !isProtecting)
        {
            if ((transform.position.x < posPlayer.x && directionLeft) || (transform.position.x > posPlayer.x && !directionLeft))
            {
                // max is 2.7f 
                if (transform.position.y >= posPlayer.y - 1.2f && transform.position.y <= posPlayer.y + 1.5f)
                {
                    stats.health -= damage;
                    if(stats.health > 0)
                        collisionScript.Knockback(stats.knockback);
                }
            }
        }
    }
}
