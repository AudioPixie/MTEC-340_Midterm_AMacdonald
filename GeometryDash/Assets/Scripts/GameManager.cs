using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player Player;
    public CameraMovement CameraMovement;
    public ParticleBGMovement ParticleBGMovement;

    [SerializeField]
    private string _state;
    public string State
    {
        get => _state;
        set
        {
            _state = value;
        }
    }

    public int attemptCount;

    private string tempLevel;

    public KeyCode inputKey;
    public KeyCode pauseKey;

    public GameObject Level1Map;
    public GameObject Level2Map;
    public GameObject Level3Map;

    public CanvasGroup startGUI;
    public CanvasGroup mainMenuGUI;
    public CanvasGroup levelSelectGUI;
    public CanvasGroup pauseGUI;

    public TextMeshProUGUI attemptGUI;
    public TextMeshProUGUI level1CompleteGUI;
    public TextMeshProUGUI level2CompleteGUI;

    private AudioSource m_audioSource;

    public AudioClip clickSound;
    public AudioClip explosionSound;
    public AudioClip victorySound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        m_audioSource = GetComponent<AudioSource>();

        attemptCount = 1;

        Level1Map.SetActive(false);
        Level2Map.SetActive(false);
        Level3Map.SetActive(false);
    }

    private void Start()
    {
        State = "Start";
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        attemptGUI.text = "Attempt Count: " + attemptCount + "\n";

        if (State == "Start")
        {
            startGUI.gameObject.SetActive(true);
            mainMenuGUI.gameObject.SetActive(true);

            levelSelectGUI.gameObject.SetActive(false);
            pauseGUI.gameObject.SetActive(false);

            attemptGUI.enabled = false;
            level1CompleteGUI.enabled = false;
            level2CompleteGUI.enabled = false;

            Level1Map.SetActive(false);
            Level2Map.SetActive(false);
            Level3Map.SetActive(false);
        }

        if (State == "LevelSelect")
        {
            startGUI.gameObject.SetActive(true);
            levelSelectGUI.gameObject.SetActive(true);

            mainMenuGUI.gameObject.SetActive(false);
            pauseGUI.gameObject.SetActive(false);

            attemptGUI.enabled = false;

            if (Player.level1Complete == true)
                level1CompleteGUI.enabled = true;

            if (Player.level2Complete == true)
                level2CompleteGUI.enabled = true;

            Level1Map.SetActive(false);
            Level2Map.SetActive(false);
            Level3Map.SetActive(false);
        }

        if (State == "Level1" || State == "Level2" || State == "Level3")
        {
            startGUI.gameObject.SetActive(false);
            pauseGUI.gameObject.SetActive(false);

            attemptGUI.enabled = true;

            level1CompleteGUI.enabled = false;
            level2CompleteGUI.enabled = false;

            if (Input.GetKeyDown(pauseKey))
            {
                tempLevel = State;
                Player.tempMode = Player.mode;
                Player.mode = "Inactive";
                State = "Pause";
            }

        }

        if (State == "Pause")
        {
            pauseGUI.gameObject.SetActive(true);
        }
    }

    public void OnClickLS()
    {
        Instance.PlaySound(clickSound);
        State = "LevelSelect";
        Player.transform.position = new Vector3(-17, 2, 0); //--turn me back on
        CameraMovement.Reset();
        ParticleBGMovement.Reset();
    }

    public void OnClickL1()
    {
        Instance.PlaySound(clickSound);
        attemptCount = 1;
        Level1Map.SetActive(true);
        Player.Shift("Regular");
        State = "Level1";
        Player.mode = "Regular";

    }

    public void OnClickL2()
    {
        Instance.PlaySound(clickSound);
        attemptCount = 1;
        Level2Map.SetActive(true);
        Player.Shift("Regular");
        State = "Level2";
        Player.mode = "Regular";
    }

    public void OnClickL3()
    {
        Instance.PlaySound(clickSound);
        attemptCount = 1;
        Level3Map.SetActive(true);
        Player.Shift("Regular");
        State = "Level3";
        Player.mode = "Regular";
    }

    public void OnClickLBack()
    {
        Instance.PlaySound(clickSound);
        State = "Start";
    }

    public void OnClickContinue()
    {
        Instance.PlaySound(clickSound);
        State = tempLevel;
        Player.mode = Player.tempMode;
    }

    public void OnClickGameExit()
    {
        Instance.PlaySound(clickSound);
        Application.Quit();
    }

    public void PlaySound(AudioClip clip, float volume = 0.7f)
    {
        m_audioSource.volume = volume;
        m_audioSource.PlayOneShot(clip);
    }
}