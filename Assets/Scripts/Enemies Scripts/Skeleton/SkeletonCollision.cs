using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SkeletonCollision : MonoBehaviour
{
    public SpikeData spikeData;
    public LayerMask obstacles;

    private CapsuleCollider2D myCollider;
    private EnemyStateMachine stateMach;
    private SkeletonAI ai;
    private SkeletonStats stats;
    private Rigidbody2D rb;
    private float delayBtwHits;
    private Vector2 dirRay;
    private Vector3 offsetRay;
    private bool doneHit;
    private bool isInSpikes;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMach = GetComponent<EnemyStateMachine>();
        ai = GetComponent<SkeletonAI>();
        stats = GetComponent<SkeletonStats>();
        myCollider = GetComponent<CapsuleCollider2D>();

        Physics2D.IgnoreLayerCollision(7, 9, true);
        Physics2D.IgnoreLayerCollision(9, 9, true);
    }

    void Update()
    {
        // hits from spikes
        if (isInSpikes)
        {
            if (delayBtwHits > spikeData.delayHits)
            {
                delayBtwHits = 0;
                doneHit = false;
            }
            else
            {
                delayBtwHits += Time.deltaTime;
            }
        }
        //Check if trying to first attack
        if (isInSpikes && !doneHit)
        {
            doneHit = true;
            OnHit(spikeData.damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spikes"))
        {
            isInSpikes = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Spikes"))
        {
            isInSpikes = false;
        }
    }

    private void FixedUpdate()
    {
        // set direction & offset of raycast in depend on moving direction
        if (ai.movingInLeft)
        {
            dirRay = Vector2.left;
            offsetRay = new Vector3(myCollider.size.x / -2 - 0.5f, 0, 0);
        }
        else 
        {
            dirRay = Vector2.right;
            offsetRay = new Vector3(myCollider.size.x / 2 + 0.5f, 0, 0);
        }

        RaycastHit2D obstacleInfo = Physics2D.Raycast(transform.position + offsetRay, dirRay, 0.4f, obstacles);
        if (obstacleInfo)
        {
            ai.movingInLeft = !ai.movingInLeft;
        }
    }

    /// <summary>
    /// Skeleton is hitted 
    /// </summary>
    public void OnHit(float dam)
    {
        Knockback(stats.knockback);
        StartCoroutine(HitPlayAnim());
    }

    /// <summary>
    /// set knockback
    /// </summary>
    /// <param name="force"> knockback force</param>
    public void Knockback(float force)
    {
        float dependForce = force;
        if (!ai.movingInLeft)
            dependForce = -dependForce; 

        rb.AddForce(new Vector2(dependForce, force), ForceMode2D.Impulse);
    }

    IEnumerator HitPlayAnim()
    {
        stateMach.ChangeAnimationState(stateMach.ENEMY_HIT);
        ai.isHitting = true;
        yield return new WaitForSeconds(0.3f);
        ai.isHitting = false;

        if (stats.health > 0) { stateMach.ChangeAnimationState(stateMach.ENEMY_IDLE); }
    }
}
