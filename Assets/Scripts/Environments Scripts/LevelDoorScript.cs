using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelDoorScript : MonoBehaviour
{
    [Tooltip("This number of level will be loaded when Door is activated")]
    [SerializeField] private int loadScene = 0;
    [SerializeField] private int neededLevel = 0;
    [Header("In place {0} is displayed current needed level")]
    [TextArea]
    public string _infoPC = "Press E to load {0} level";
    [TextArea]
    public string _infoGamePad = "Press RB to load {0} level";

    private Light2D[] lights = new Light2D[2];
    private bool isPlayer;
    private UI_SpeechBalloonsScript speechBalloon;
    private PlayerStats player;
    private PlayerInputActions playerInput;

    void Awake()
    {
        speechBalloon = GameObject.FindObjectOfType<UI_SpeechBalloonsScript>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        lights = GetComponentsInChildren<Light2D>();

        if(player.GetCurrentLvL() >= neededLevel)
        { SwitchLight(true); } 
        else { SwitchLight(false); }
    }

    private void OnEnable()
    {
        
        playerInput = new PlayerInputActions();
        playerInput.Interactions.GotoLevel.performed += OnInteraction;
        playerInput.Enable();
        playerInput.Interactions.GotoLevel.Enable();
    }

    private void OnChangeControl(PlayerInput obj)
    {
        Debug.LogWarning("ZMIANA! " + obj.currentControlScheme);
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Interactions.GotoLevel.Disable();
    }

    private void OnInteraction(InputAction.CallbackContext obj)
    {
        
        if (isPlayer)
        {
            isPlayer = false;
            string info = Gamepad.current.enabled ? _infoGamePad : _infoPC;
            speechBalloon.InstantBalloon(string.Format(info, neededLevel), false);
            SceneManager.LoadScene(loadScene);
        }

        if (player.GetCurrentLvL() >= neededLevel)
        { SwitchLight(true); }
        else { SwitchLight(false); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player.GetCurrentLvL() >= neededLevel)
        {
            string info = Gamepad.current.enabled ? _infoGamePad : _infoPC;
            speechBalloon.InstantBalloon(string.Format(info, neededLevel), true);
            isPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string info = Gamepad.current.enabled ? _infoGamePad : _infoPC;
            speechBalloon.InstantBalloon(string.Format(info, neededLevel), false);
            isPlayer = false;
        }
    }

    void SwitchLight(bool active)
    {
        foreach(var light in lights)
        {
            if(light.enabled != active)
                light.enabled = active;
        }
    }
}
