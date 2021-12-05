using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDiePlayer
{
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float jumpVelocity = 18f;
    [SerializeField] float fallMultiplayer = 2.5f;
    [SerializeField] LayerMask groundMask;

    private PlayerInputActions playerInput;
    private PlayerStateMachine plSM;
    private RandomSoundsPlayer plSounds;
    private Rigidbody2D rb2d;
    private InputAction movementInput;
    private InputAction jumpInput;

    private bool m_dirLeft;
    private bool m_isGrounded;
    private bool m_blockageMovement;
    private bool m_blockageAnims;

    void Awake()
    {
        plSM = GetComponent<PlayerStateMachine>();
        plSounds = GetComponent<RandomSoundsPlayer>();
        rb2d = GetComponent<Rigidbody2D>();
        m_isGrounded = true;
    }

    private void OnEnable()
    {
        playerInput = new PlayerInputActions();

        movementInput = playerInput.Movement.MovingAction;
        jumpInput = playerInput.Movement.Jump;

        playerInput.Movement.Jump.Enable();
        playerInput.Movement.MovingAction.Enable();
    }

    private void OnDisable()
    {
        playerInput.Movement.Jump.Disable();
        playerInput.Movement.MovingAction.Disable();
    }

    void FixedUpdate()
    {
        // detecting is player standing on the ground
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 0.125f, transform.position.y), Vector2.down, 1.4f, groundMask);

        if (hit.collider != null)
        {
            if (rb2d.velocity.y < 0 && !m_isGrounded)
            {
                plSounds.PlayTable("Land");
            }
            m_isGrounded = true;
        }
        else
        {
            m_isGrounded = false;
        }

        //Check update movement based on input
        Vector2 velocity = new Vector2(0, rb2d.velocity.y);
        // Movement
        if (!m_blockageMovement)
        {
            if (movementInput.ReadValue<float>() < 0)
            {
                velocity.x = -walkSpeed;
                m_dirLeft = true;
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (movementInput.ReadValue<float>() > 0)
            {
                velocity.x = walkSpeed;
                m_dirLeft = false;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                velocity.x = 0;
            }
        }
        // Check if trying to jump
        if (jumpInput.ReadValue<float>() != 0 && m_isGrounded)
        {
            if(!m_blockageAnims) 
                plSM.ChangeAnimationState(plSM.PLAYER_JUMP_UP);
            plSounds.PlayTable("Jump");
            velocity = Vector2.up * jumpVelocity;
        }
        // falls after jump
        if (rb2d.velocity.y < 0 && !m_isGrounded)
        {
            if (!m_blockageAnims) 
                plSM.ChangeAnimationState(plSM.PLAYER_JUMP_DOWN);
            velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplayer - 1) * Time.deltaTime;
        }
        // play idle animation
        if (movementInput.ReadValue<float>() != 0 && m_isGrounded)
        {
            if (!m_blockageAnims) 
                plSM.ChangeAnimationState(plSM.PLAYER_RUN);
            plSounds.PlayTable("Footsteps");
        }
        else if (m_isGrounded)
        {
            if (!m_blockageAnims) 
                plSM.ChangeAnimationState(plSM.PLAYER_IDLE);
        }
        //Assign the new velocity
        rb2d.velocity = velocity;
    }

    public void PlayerDead(bool isDead)
    {
        this.enabled = isDead;
    }
    public void BlockMovement(bool block)
    {
        m_blockageMovement = block;
    }
    public void BlockAnimations(bool block)
    {
        m_blockageAnims = block;
    }
    public bool GetDirection()
    {
        return m_dirLeft;
    }
}
