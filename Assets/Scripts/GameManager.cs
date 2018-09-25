using DataManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        GenerateSave();
    }

    private void GenerateSave()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.GenerateSave("Autosave");
    }
}
