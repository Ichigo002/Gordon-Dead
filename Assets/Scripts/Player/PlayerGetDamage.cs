using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetDamage : MonoBehaviour, IDiePlayer
{
    [SerializeField] private SpikeData spikeData;
    [SerializeField] private float health = 100f;

    private PlayerStateMachine plSM;
    private RandomSoundsPlayer plSounds;
    private PlayerMovement plMovement;
    private UI_PointsModule UI_points;

    private bool TouchSpikes;
    private bool nextDelayDam;
    private float delayDamage;
    private float maxHp;

    void Awake()
    {
        plMovement = GetComponent<PlayerMovement>();
        plSM = GetComponent<PlayerStateMachine>();
        plSounds = GetComponent<RandomSoundsPlayer>();
        UI_points = GameObject.FindObjectOfType<UI_PointsModule>();
        maxHp = health;
    }
    private void OnEnable()
    {
        SkeletonAI.GetAttackEnemy += GetDamage;
    }
    private void OnDisable()
    {
        SkeletonAI.GetAttackEnemy -= GetDamage;
    }
    void Update()
    {
        //die
        if (health <= 0)
            GetComponent<PlayerDie>().DiePlayer();
        //damage from spikes
        if (TouchSpikes)
        {
            if (delayDamage > spikeData.delayHits)
            {
                delayDamage = 0;
                nextDelayDam = false;
            }
            else
            {
                delayDamage += Time.deltaTime;
            }
        }
        if (TouchSpikes && !nextDelayDam)
        {
            GetDamage(spikeData.damage);
            nextDelayDam = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spikes"))
        {
            TouchSpikes = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Spikes"))
        {
            TouchSpikes = false;
        }
    }
    public void PlayerDead(bool isDead)
    {
        this.enabled = isDead;
    }
    void GetDamage(float health)
    {
        StartCoroutine(DamageAnim());
        plSounds.PlayTable("Hits");
        this.health -= health;
        UI_points.LoseHeart(this.health, maxHp);
    }

    IEnumerator DamageAnim()
    {
        plMovement.BlockAnimations(true);
        plSM.ChangeAnimationState(plSM.PLAYER_HIT);
        yield return new WaitForSeconds(plSM.GetLengthCurrentClip() + 0.1f);
        plMovement.BlockAnimations(false);
    }
}
