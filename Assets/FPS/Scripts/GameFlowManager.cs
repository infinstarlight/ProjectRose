using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameFlowManager : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Duration of the fade-to-black at the end of the game")]
    public float endSceneLoadDelay = 3f;
    [Tooltip("The canvas group of the fade-to-black screen")]
    public CanvasGroup endGameFadeCanvasGroup;

    [Header("Win")]
    [Tooltip("This string has to be the name of the scene you want to load when winning")]
    public string winSceneName = "WinScene";
    [Tooltip("Duration of delay before the fade-to-black, if winning")]
    public float delayBeforeFadeToBlack = 4f;
    [Tooltip("Duration of delay before the win message")]
    public float delayBeforeWinMessage = 2f;
    [Tooltip("Sound played on win")]
    public AudioClip victorySound;
    [Tooltip("Prefab for the win game message")]
    public GameObject WinGameMessagePrefab;

    [Header("Lose")]
    [Tooltip("This string has to be the name of the scene you want to load when losing")]
    public string loseSceneName = "LoseScene";


    public bool gameIsEnding { get; private set; }

    public GameObject GameHUD;

    public IS_PlayerCharacterController[] m_Players;
    NotificationHUDManager m_NotificationHUDManager;
    ObjectiveManager m_ObjectiveManager;
    float m_TimeLoadEndGameScene;
    string m_SceneToLoad;

    RoseInputSettings roseControls;
    public GameObject playerPrefab;

    void Awake()
    {
       // roseControls = new RoseInputSettings();
        
        // m_Players = FindObjectsOfType<IS_PlayerCharacterController>();
    }

    void Start()
    {
        // roseControls = new RoseInputSettings();
        // var bindingGroup = roseControls.controlSchemes.First(x => x.name == "Gamepad").bindingGroup;
        // // Spawn players with specific devices.
        // var p1 = PlayerInput.Instantiate(playerPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[0]);
        // var p2 = PlayerInput.Instantiate(playerPrefab, controlScheme: "PC_KBM", pairWithDevice: Keyboard.current);
        // p1.user.UnpairDevice(Keyboard.current);
        // p1.user.UnpairDevice(Mouse.current);
        // p2.user.UnpairDevice(Gamepad.all[0]);
        // p1.user.actions.bindingMask = InputBinding.MaskByGroup(bindingGroup);
      
    
    
        m_ObjectiveManager = GetComponent<ObjectiveManager>();
        //  DebugUtility.HandleErrorIfNullFindObject<ObjectiveManager, GameFlowManager>(m_ObjectiveManager, this);

        AudioUtility.SetMasterVolume(1);

        FindPlayers();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {


        if (gameIsEnding)
        {
            float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / endSceneLoadDelay;
            endGameFadeCanvasGroup.alpha = timeRatio;

            AudioUtility.SetMasterVolume(1 - timeRatio);

            // See if it's time to load the end scene (after the delay)
            if (Time.time >= m_TimeLoadEndGameScene)
            {
                SceneManager.LoadScene(m_SceneToLoad);
                gameIsEnding = false;
            }
        }
        else
        {
            if (m_ObjectiveManager.AreAllObjectivesCompleted())
                EndGame(true);

            // // Test if player died
            // if (m_Players[0].isDead)
            //     EndGame(false);
        }
    }

    void EndGame(bool win)
    {
        // unlocks the cursor before leaving the scene, to be able to click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Remember that we need to load the appropriate end scene after a delay
        gameIsEnding = true;
        endGameFadeCanvasGroup.gameObject.SetActive(true);
        if (win)
        {
            m_SceneToLoad = winSceneName;
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay + delayBeforeFadeToBlack;

            // play a sound on win
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = victorySound;
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
            audioSource.PlayScheduled(AudioSettings.dspTime + delayBeforeWinMessage);

            // create a game message
            var message = Instantiate(WinGameMessagePrefab).GetComponent<DisplayMessage>();
            if (message)
            {
                message.delayBeforeShowing = delayBeforeWinMessage;
                message.GetComponent<Transform>().SetAsLastSibling();
            }
        }
        else
        {
            m_SceneToLoad = loseSceneName;
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay;
        }
    }

    public void FindPlayers()
    {
        int i = 0;
        m_Players = FindObjectsOfType<IS_PlayerCharacterController>();
        Debug.Log("We have " + m_Players.Length + " player(s)!");
        for (i = 0; i < m_Players.Length; i++)
        {
            m_Players[0].GetComponent<IS_PlayerInputHandler>().playerIndex = 0;
            m_Players[0].GetComponent<IS_PlayerInputHandler>().bIsHost = true;
            if (!m_Players[i].GetComponent<IS_PlayerInputHandler>().bIsHost)
            {
                m_Players[i].GetComponent<IS_PlayerInputHandler>().playerIndex = i + 1;
                m_Players[i].playerCamera.GetComponent<AudioListener>().enabled = false;

            }
            // var NewHUD = Instantiate(GameHUD);
            // NewHUD.GetComponent<JetpackCounter>().desiredPlayer = m_Players[i].gameObject;
            // NewHUD.GetComponent<WeaponHUDManager>().desiredPlayer = m_Players[i].gameObject;
            // NewHUD.GetComponent<PlayerHealthBar>().myPlayerCharacter = m_Players[i].GetComponent<IS_PlayerCharacterController>();
            // NewHUD.GetComponent<FeedbackFlashHUD>().myPlayerCharacter = m_Players[i].GetComponent<IS_PlayerCharacterController>();
            // NewHUD.GetComponent<NotificationHUDManager>().PlayerWeaponsManager = m_Players[i].GetComponent<IS_PlayerWeaponsManager>();
            // NewHUD.GetComponent<StanceHUD>().character = m_Players[i].GetComponent<IS_PlayerCharacterController>();
            // NewHUD.GetComponent<CrosshairManager>().m_WeaponsManager = m_Players[i].GetComponent<IS_PlayerWeaponsManager>();

        }

        switch (m_Players.Length)
        {
            case 1:
                {
                    m_Players[0].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(0, 0, 1, 1);
                    break;
                }
            case 2:
                {
                    m_Players[0].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(0, 0.5f, 1, 0.5f);
                    m_Players[1].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(0, 0, 1, 0.5f);
                    m_Players[0].GetComponent<IS_PlayerInputHandler>().bIsHost = true;
                    m_Players[1].GetComponent<IS_PlayerInputHandler>().bIsHost = false;


                    break;
                }
            case 3:
                {
                    m_Players[2].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(-0.5f, 0.5f, 1, 0.5f);
                    m_Players[1].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    m_Players[0].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(0, 0, 1, 0.5f);
                    m_Players[0].GetComponent<IS_PlayerInputHandler>().bIsHost = true;
                    m_Players[1].GetComponent<IS_PlayerInputHandler>().bIsHost = false;
                    m_Players[2].GetComponent<IS_PlayerInputHandler>().bIsHost = false;
                    break;
                }
            case 4:
                {
                    m_Players[3].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    m_Players[2].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(0, 0, 0.5f, 0.5f);
                    m_Players[1].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    m_Players[0].GetComponent<IS_PlayerCharacterController>().playerCamera.rect = new Rect(-0.5f, 0.5f, 1, 0.5f);
                    m_Players[0].GetComponent<IS_PlayerInputHandler>().bIsHost = true;
                    m_Players[1].GetComponent<IS_PlayerInputHandler>().bIsHost = false;
                    m_Players[2].GetComponent<IS_PlayerInputHandler>().bIsHost = false;
                    m_Players[3].GetComponent<IS_PlayerInputHandler>().bIsHost = false;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void OnJoin()
    {
        var p2 = PlayerInput.Instantiate(playerPrefab, -1, "Gamepad", -1, Gamepad.current);
        p2.actions = new RoseInputSettings1().asset;
        FindPlayers();
    }
}
