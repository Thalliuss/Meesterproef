using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

    [HideInInspector]
    public GlobalData globalData;

    public static GameManager Instance { get; set; }

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

        if (SceneManager.GetActiveScene().buildIndex != _currentLevel) {
            ReloadCurrentLevel();
        } //else StartCoroutine(Loadingscreen(2, "Level " + (SceneManager.GetActiveScene().buildIndex)));

    }

    private IEnumerator SaveTimer(float p_input)
    {
        while (true)  
        {
            yield return new WaitForSeconds(p_input);

            globalData.Timer = _timer;
            globalData.Save();
        }
    }

    private float LoadTimer()
    {
        return globalData.Timer;
    }

    private int LoadCurrentLevel()
    {
        return (globalData.CurrentLevel != 0) ? globalData.CurrentLevel : SceneManager.GetActiveScene().buildIndex;
    }

    private void SaveCurrentLevel(Scene p_previous, Scene p_next)
    {
        _currentLevel = p_next.buildIndex;

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

    [ContextMenu("Next Level.")]
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
            return;

        //StartCoroutine(Loadingscreen(2, "Level " + (SceneManager.GetActiveScene().buildIndex + 1)));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        PlayerData t_playerData = FindObjectOfType<Player>().playerData;

        t_playerData.Position = Vector3.zero;
        t_playerData.Save();
    }

    private void ReloadCurrentLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
            return;

        //StartCoroutine(Loadingscreen(2, "Level " + (SceneManager.GetActiveScene().buildIndex + 1)));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    [ContextMenu("Previous Level.")]
    public void PreviousLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex - 1 < 1)
            return;

        //StartCoroutine(Loadingscreen(2, "Level " + (SceneManager.GetActiveScene().buildIndex - 1)));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        PlayerData t_playerData = FindObjectOfType<Player>().playerData;
        t_playerData.Position = Vector3.zero;
        t_playerData.Save();
    }

    private void Update()
    {
        _timer += 1f * Time.deltaTime;

        _timerVisual.text = "Timer: " + _timer.ToString();
        _currentLevelVisual.text = "Level: " + _currentLevel.ToString();
    }
}
