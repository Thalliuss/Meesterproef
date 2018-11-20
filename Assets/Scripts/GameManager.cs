using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Save Implementation 

    private const string _globalDataID = "GlobalDataID";

    private DataManagement.SceneManager _sceneManager;
    private GlobalData _globalData;

    private void Setup()
    {
        SceneManager.activeSceneChanged += SaveCurrentLevel;

        List<GlobalData.Highscore> t_highscores = (_globalData != null) ? _globalData.Highscores : null;
        _globalData = _sceneManager.DataReferences.FindElement<GlobalData>(_globalDataID);

        if (_globalData == null)
        {
            DataManagement.DataManager.Instance.GenerateSave("Autosave");

            _sceneManager.DataReferences.AddElement<GlobalData>(_globalDataID);
            _globalData = _sceneManager.DataReferences.FindElement<GlobalData>(_globalDataID);

            if (t_highscores != null && t_highscores.Count != 0)
            {
                _globalData.Highscores = t_highscores;
                _globalData.Save();
            }
        }
        _currentLevel = LoadCurrentLevel();

        _timer = LoadTimer();
        StartCoroutine(SaveTimer(1));
    }

    private void SaveCurrentLevel(Scene p_previous, Scene p_next)
    {
        if (_globalData == null)
            return;

        _currentLevel = p_next.buildIndex - 1;

        _globalData.CurrentLevel = _currentLevel;
        _globalData.Save();
    }

    private IEnumerator SaveTimer(float p_input)
    {
        while (_globalData != null)
        {
            yield return new WaitForSeconds(p_input);

            _globalData.Timer = _timer;
            _globalData.Save();
        }
    }

    private int LoadCurrentLevel()
    {
        return (_globalData.CurrentLevel != 0 && _globalData != null) ? _globalData.CurrentLevel : 1;
    }

    private float LoadTimer()
    {
        return (_globalData.Timer != 0 && _globalData != null) ? _globalData.Timer : 0;
    }

    #endregion

    #region UI Elements

    [Header("HUD")]
    [SerializeField] private GameObject _hud;

    public Slider HealthVisual
    {
        get
        {
            return _healthVisual;
        }

        set
        {
            _healthVisual = value;
        }
    }
    [SerializeField, Header("UI Visuals")]
    private Slider _healthVisual;

    [SerializeField] private Text _timerVisual;
    [SerializeField] private Text _currentLevelVisual;
    [SerializeField] private Text _highscoreVisual;
    [SerializeField] private Button _start;

    public void Continue()
    {
        SceneManager.LoadScene(_currentLevel + 1);
        if (_currentLevel == 4) Destroy(this.gameObject);
    }

    public void StartGame()
    {
        if(_globalData == null)
            Setup();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        if (DataManagement.DataManager.Instance == null || _sceneManager.DataReferences.FindElement<GlobalData>(_globalDataID) == null)
            return;

        DataManagement.DataManager.Instance.ClearData("PlayerDataID");
        DataManagement.DataManager.Instance.ClearData("ListedEnemyData");
        DataManagement.DataManager.Instance.ClearData("GameData");

        StartCoroutine(WaitUntilNextLevel());

        NextLevel();
    }
    private IEnumerator WaitUntilNextLevel()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex != 1);
        Setup();
    }

    public void Exit()
    {
        Application.Quit();
    }

    #endregion 

    public static GameManager Instance { get; set; }

    private float _timer;
    private int _currentLevel;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance != null)
            Destroy(Instance.gameObject);
        
        Instance = this;
    }

    private void Start()
    {
        _sceneManager = DataManagement.SceneManager.Instance;
        if (_sceneManager.DataReferences.FindElement<GlobalData>(_globalDataID) != null)
        {
            _start.gameObject.SetActive(false);
            Setup();
        }

        if (_globalData == null)
            return;

        for (int i = _globalData.Highscores.Count; i > 0; i--)
            _highscoreVisual.text += _globalData.Highscores.Count - i + 1 + ". " + _globalData.Highscores[i -1].Score + "\r\n";
    }

    [ContextMenu("Next Level.")]
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            List<GlobalData.Highscore> t_highscores = _globalData.Highscores;

            if (t_highscores.Count == 0)
            {
                GlobalData.Highscore t_highscore = new GlobalData.Highscore()
                {
                    Score = _globalData.Timer
                };

                _globalData.Highscores.Add(t_highscore);
                _globalData.Save();
            }
            else
            {
                if (_globalData.Timer < t_highscores[t_highscores.Count - 1].Score)
                {
                    GlobalData.Highscore t_highscore = new GlobalData.Highscore()
                    {
                        Score = _globalData.Timer
                    };

                    _globalData.Highscores.Add(t_highscore);
                    _globalData.Save();
                }
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Destroy(gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
            return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        Player t_player = FindObjectOfType<Player>();

        if (t_player == null)
            return;

        t_player.PlayerData.Position = Vector3.zero;
        t_player.PlayerData.Save();
    }

    [ContextMenu("Previous Level.")]
    public void PreviousLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex - 1 < 1)
            return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        Player t_player = FindObjectOfType<Player>();

        if (t_player == null)
            return;

        t_player.PlayerData.Position = Vector3.zero;
        t_player.PlayerData.Save();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            _hud.SetActive(false);
            return;
        }
        _hud.SetActive(true);

        _timer += 1f * Time.deltaTime;

        _timerVisual.text = "Timer: " + _timer.ToString();
        _currentLevelVisual.text = "Level: " + _currentLevel.ToString();
    }
}

