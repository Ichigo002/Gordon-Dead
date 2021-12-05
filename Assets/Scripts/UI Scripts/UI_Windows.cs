using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Windows : MonoBehaviour
{
    [SerializeField] private GameObject subpausedMenu;
    [SerializeField] private GameObject subpausedOptions;

    private Animator animator;
    private PlayerInputActions playerInput;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerInput = new PlayerInputActions();
        playerInput.UI.Escape.performed += OnClickPause;

        playerInput.Enable();
        playerInput.UI.Escape.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.UI.Escape.Disable();
    }

    private void OnClickPause(InputAction.CallbackContext obj)
    {

        if (Time.timeScale == 1)
        {
            //pause game
            Time.timeScale = 0;
            OnBackMenu();
        }
        else
        {
            if(subpausedOptions.activeSelf)
            {
                OnBackMenu();
                return;
            }

            // play game
            Time.timeScale = 1;

        }

        RefreshAnimator();
    }

    private void RefreshAnimator() => animator.SetBool("paused", Time.timeScale == 0);

    //Resume button Action
    public void OnResume()
    {
        Time.timeScale = 1;
        RefreshAnimator();
    }

    //Options button Action
    public void OnShowOptions()
    {
        subpausedMenu.SetActive(false);
        subpausedOptions.SetActive(true);
    }

    //back from options to menu button action
    public void OnBackMenu()
    {
        subpausedMenu.SetActive(true);
        subpausedOptions.SetActive(false);
    }

    //Quit button Action
    public void OnQuit()
    {
        Debug.LogWarning("QUITTING FROM THAT HELL GAME!!!!");
    }

    // finish window Action
    public void OnFinished()
    {
        Time.timeScale = 0f;
        animator.SetTrigger("finished");
    }

}
