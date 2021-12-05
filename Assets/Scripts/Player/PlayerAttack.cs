using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MarchingBytes;

public class PlayerAttack : MonoBehaviour, IDiePlayer
{
    [SerializeField] float attackRange = 2.5f;
    [SerializeField] float damage = 15f;
    [SerializeField] float delayAttack = 0.2f;

    private PlayerInputActions playerInput;
    private SoundManager audioSfx;
    private PlayerStateMachine plSM;
    private PlayerMovement plMovement;

    private bool doneAttack;
    private bool startDelayAttack;
    private int numberAttack;
    private float timeDelayAttack;

    public delegate void InSightAttack(float sight, float damage, Vector3 posPlayer, bool directionLeft);
    public static event InSightAttack InSightAttackEvent;

    void Awake()
    {
        plSM = GetComponent<PlayerStateMachine>();
        audioSfx = GetComponent<SoundManager>();
        plMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        playerInput = new PlayerInputActions();
        playerInput.Actions.Atttack.performed += OnAttackAction;

        playerInput.Actions.Atttack.Enable();
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Actions.Atttack.Disable();
        playerInput.Disable();
    }

    private void OnAttackAction(InputAction.CallbackContext obj)
    {
        // Check if trying to attack;
        if (!doneAttack)
        {
            doneAttack = true;
            if (numberAttack > 1)
            {
                //Second attack
                numberAttack = 0;
                audioSfx.PlaySound("Player_Attack_2");
                StartCoroutine(Attack(plSM.PLAYER_ATTACK_2));
            }
            else
            {
                //First attack
                audioSfx.PlaySound("Player_Attack_1");
                StartCoroutine(Attack(plSM.PLAYER_ATTACK_1));
            }
        }
    }

    void Update()
    {
        // delay between attacks
        if (doneAttack && startDelayAttack)
        {
            if (timeDelayAttack > delayAttack)
            {
                timeDelayAttack = 0;
                doneAttack = false;
                startDelayAttack = false;
            }
            else
            {
                timeDelayAttack += Time.deltaTime;
            }
        }
    }

    IEnumerator Attack(string anim)
    {
        plMovement.BlockMovement(true);
        plMovement.BlockAnimations(true);

        InSightAttackEvent?.Invoke(attackRange, damage, transform.position, plMovement.GetDirection());

        plSM.ChangeAnimationState(anim);
        yield return new WaitForSeconds(plSM.GetLengthCurrentClip());

        plMovement.BlockMovement(false);
        plMovement.BlockAnimations(false);
        startDelayAttack = true;
        numberAttack++;
    }

    public void PlayerDead(bool isDead)
    {
        this.enabled = isDead;
    }
}
