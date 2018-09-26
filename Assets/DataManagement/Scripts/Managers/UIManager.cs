using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }
    public bool MenuOpened { get; set; }

    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private GameObject _loading;
    [SerializeField]
    private GameObject _commands;

    [SerializeField]
    private Text _loadingText;

    public Slider LoadingBar
    {
        get
        {
            return _loadingBar;
        }

        set
        {
            _loadingBar = value;
        }
    }

    [SerializeField]
    private Slider _loadingBar;

    public bool CommandsOpened
    {
        get
        {
            return _commandsOpened;
        }

        set
        {
            _commandsOpened = value;
        }
    }
    private bool _commandsOpened = false;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        DontDestroyOnLoad(this);
    }

    public void LockCursor(bool p_input)
    {
        if (p_input)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SetTimeScale(int p_input)
    {
        Time.timeScale = p_input;
    }

    private void OpenMenu()
    {
        if (Input.GetKeyDown(KeyCode.E) && !LoadingscreenManager.Instance.IsLoading && !LoadingscreenManager.Instance.IsSetupScene)
        {
            MenuOpened = !MenuOpened;
            _menu.SetActive(MenuOpened);

            if (MenuOpened == true)
            {
                SetTimeScale(0);
            }
            else
            {
                SetTimeScale(1);
            }
        }
    }

    private void OpenPrompt()
    {
        if (Input.GetKeyDown(KeyCode.P) && !LoadingscreenManager.Instance.IsLoading && !LoadingscreenManager.Instance.IsSetupScene)
        {
            _commandsOpened = !_commandsOpened;
            _commands.SetActive(_commandsOpened);
        }
    }

    public void CloseMenu()
    {
        LockCursor(true);
        _menu.SetActive(false);
        MenuOpened = false;
    }

    public void OpenLoading(string p_input)
    {
        _loading.SetActive(true);
        _loadingText.text = p_input;
    }

    public void CloseLoading()
    {
        _loading.SetActive(false);
        _loadingText.text = "";
    }

    public void ClosePrompt()
    {
        _commands.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void Update()
    {
        OpenMenu();
        OpenPrompt();
    }
}
