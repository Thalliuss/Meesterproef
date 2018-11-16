using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [HideInInspector]
    public GlobalData globalData;

    private DataManagement.SceneManager _sceneManager;
    private const string _globalDataID = "GlobalDataID";

    [SerializeField]
    private Text _timerVisual;
    [SerializeField]
    private float _timer;

    [SerializeField]
    private Text _currentLevelVisual;
    [SerializeField]
    private int _currentLevel;

    [SerializeField]
    private Text _highscoreVisual;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Setup();

        _currentLevel = LoadCurrentLevel();
        SceneManager.activeSceneChanged += SaveCurrentLevel;

        _timer = LoadTimer();
        StartCoroutine(SaveTimer(1));

        if (globalData == null)
            return;

        for (int i = globalData.Highscores.Count; i > 0; i--)
            _highscoreVisual.text += globalData.Highscores.Count - i + 1 + ". " + globalData.Highscores[i -1].Score + "\r\n";
    }

    private IEnumerator SaveTimer(float p_input)
    {
        while (true && globalData != null)  
        {
            yield return new WaitForSeconds(p_input);

            globalData.Timer = _timer;
            globalData.Save();
        }
    }

    private float LoadTimer()
    {
        if (globalData == null) return 0f;
        return globalData.Timer;
    }

    private int LoadCurrentLevel()
    {
        if (globalData == null) return SceneManager.GetActiveScene().buildIndex -1;
        return (globalData.CurrentLevel != 0) ? globalData.CurrentLevel : SceneManager.GetActiveScene().buildIndex -1;
    }

    private void SaveCurrentLevel(Scene p_previous, Scene p_next)
    {
        if (globalData == null)
            return;

        _currentLevel = p_next.buildIndex -1;

        globalData.CurrentLevel = _currentLevel;
        globalData.Save();
    }

    private void Setup()
    {
        if (DataManagement.DataManager.Instance != null)
            DataManagement.DataManager.Instance.GenerateSave("Autosave");

        _sceneManager = DataManagement.SceneManager.Instance;

        globalData = _sceneManager.DataReferences.FindElement<GlobalData>(_globalDataID);
        if (globalData == null)
        {
            _sceneManager.DataReferences.AddElement<GlobalData>(_globalDataID);
            globalData = _sceneManager.DataReferences.FindElement<GlobalData>(_globalDataID);
        }
    }

    private IEnumerator Loadingscreen(int p_input, string p_msg)
    {
        float t_temp = 0;

        while (t_temp != p_input)
        {
            yield return new WaitForSeconds(.01f);

            LoadingscreenManager.Instance.OpenLoadingscreen(t_temp, p_input, p_msg);

            t_temp += .1f;
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene(_currentLevel + 1);
    }

    [ContextMenu("Next Level.")]
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            List<GlobalData.Highscore> t_highscores = globalData.Highscores;

            if (t_highscores.Count == 0)
            {
                GlobalData.Highscore t_highscore = new GlobalData.Highscore()
                {
                    Score = globalData.Timer
                };

                globalData.Highscores.Add(t_highscore);
                globalData.Save();
            }
            else
            {
                if (globalData.Timer < t_highscores[t_highscores.Count - 1].Score)
                {
                    GlobalData.Highscore t_highscore = new GlobalData.Highscore()
                    {
                        Score = globalData.Timer
                    };

                    globalData.Highscores.Add(t_highscore);
                    globalData.Save();
                }
            }
            return;
        }

        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
            return;

        //StartCoroutine(Loadingscreen(2, "Level " + (SceneManager.GetActiveScene().buildIndex + 1)));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        if (SceneManager.GetActiveScene().buildIndex == 1)
            return;

        Player t_player = FindObjectOfType<Player>();

        t_player.playerData.Position = Vector3.zero;
        t_player.playerData.Save();
    }

    [ContextMenu("Previous Level.")]
    public void PreviousLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex - 1 < 1)
            return;

        //StartCoroutine(Loadingscreen(2, "Level " + (SceneManager.GetActiveScene().buildIndex - 1)));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        if (SceneManager.GetActiveScene().buildIndex == 1)
            return;

        Player t_player = FindObjectOfType<Player>();

        t_player.playerData.Position = Vector3.zero;
        t_player.playerData.Save();
    }

    public void Restart()
    {
        if (DataManagement.DataManager.Instance == null)
            return;

        DataManagement.DataManager.Instance.ClearData("ListedEnemyData");
        DataManagement.DataManager.Instance.ClearData("PlayerDataID");
        DataManagement.DataManager.Instance.ClearData("GameData");

        _timer = 0;
        _currentLevel = 0;

        NextLevel();
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            return;
        
        _timer += 1f * Time.deltaTime;

        _timerVisual.text = "Timer: " + _timer.ToString();
        _currentLevelVisual.text = "Level: " + _currentLevel.ToString();
    }
}
