using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private DataManagement.SceneManager _sceneManager;
    private const string _globalDataID = "GlobalDataID";

    [SerializeField]
    private GlobalData _globalData;

    [SerializeField]
    private float _timer;

    [SerializeField]
    private int _currentLevel;

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
    }

    private IEnumerator SaveTimer(float p_input)
    {
        while (true)  
        {
            yield return new WaitForSeconds(p_input);

            _globalData.Timer = _timer;
            _globalData.Save();
        }
    }

    private float LoadTimer()
    {
        return _globalData.Timer;
    }

    private int LoadCurrentLevel()
    {
        return (_globalData.CurrentLevel != 0) ? _globalData.CurrentLevel : SceneManager.GetActiveScene().buildIndex;
    }

    private void SaveCurrentLevel(Scene p_previous, Scene p_next)
    {
        _currentLevel = p_next.buildIndex;

        _globalData.CurrentLevel = _currentLevel;
        _globalData.Save();
    }

    private void Setup()
    {
        if (DataManagement.DataManager.Instance != null)
            DataManagement.DataManager.Instance.GenerateSave("Autosave");

        _sceneManager = DataManagement.SceneManager.Instance;

        _globalData = _sceneManager.DataReferences.FindElement<GlobalData>(_globalDataID);
        if (_globalData == null)
        {
            _sceneManager.DataReferences.AddElement<GlobalData>(_globalDataID);
            _globalData = _sceneManager.DataReferences.FindElement<GlobalData>(_globalDataID);
        }
    }

    private void Update()
    {
        _timer += 1f * Time.deltaTime;
    }
}
