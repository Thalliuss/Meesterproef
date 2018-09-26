using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingscreenManager : MonoBehaviour
{
    public static LoadingscreenManager Instance { get; set; }
    public bool IsLoading { get; set; }
    public bool IsSetupScene
    {
        get
        {
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                return true;
            } else return false;

        }
    }

    [SerializeField] private bool _loadOnStart;
    [SerializeField] private string _levelToLoad;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (_loadOnStart) LoadScene(_levelToLoad);
    }

    private void Update()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void LoadScene(string p_input)
    {
        SceneManager.LoadScene(p_input);
    }

    public void OpenLoadingscreen(float p_current, float p_max, string p_text)
    {
        UIManager t_uiManager = UIManager.Instance;

        t_uiManager.OpenLoading(p_text);
        IsLoading = true;

        t_uiManager.LoadingBar.value = p_current / p_max;
    }

    public void CloseLoadingscreen()
    {
        UIManager t_uiManager = UIManager.Instance;

        t_uiManager.CloseLoading();
        IsLoading = false;
    }
}

