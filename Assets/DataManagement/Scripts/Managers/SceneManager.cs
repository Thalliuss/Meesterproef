using UnityEngine;
using System.IO;

namespace DataManagement
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; set; }

        [SerializeField]
        private string _sceneID;

        public DataReferences DataReferences
        {
            get
            {
                return _dataReferences;
            }

            set
            {
                _dataReferences = value;
            }
        }
        [SerializeField]
        private DataReferences _dataReferences;

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);

            Instance = this;

            if (DataManager.Instance == null) return;

            _dataReferences.ID = _sceneID;

            string t_path = Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + _dataReferences.ID + "/";
            if (!Directory.Exists(t_path))
                Directory.CreateDirectory(t_path);

            Build();
        }

        public void Reload()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        private void OnDestroy()
        {
            ClearAllData();
        }

        public void ClearAllData()
        {
            _dataReferences.SaveData.ids.Clear();
            _dataReferences.SaveData.info.Clear();
            _dataReferences.SaveData.types.Clear();
        }

        private void Build()
        {
            DataManager t_dataManager = DataManager.Instance;
            if (t_dataManager != null) t_dataManager.Build();
        }
    }
}
