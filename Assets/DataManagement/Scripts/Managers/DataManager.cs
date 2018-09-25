using UnityEngine;
using System.IO;
using System;
using System.Linq;

namespace DataManagement
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; set; }
        public string ID { get; set; }

        private const string _tempID = "temp";

        [Header("Enable/Disable Encryption.")]
        public bool encrypt;

        [Header("Enable/Disable Multiple Save Files."), SerializeField]
        private bool multipleSaves;

        public SaveReferences SaveReferences
        {
            get
            {
                return _saveReferences;
            }
        }
        [SerializeField]
        private SaveReferences _saveReferences;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            ID = (CheckForLastFile() == null ? _tempID : CheckForLastFile());

            string t_path = Application.persistentDataPath + "/" + ID + "/";
            if (!Directory.Exists(t_path))
                Directory.CreateDirectory(t_path);

            if (Instance != null)
                Destroy(gameObject);

            Instance = this;

            if (!multipleSaves)
            {
                if (_saveReferences.save != null)
                    _saveReferences.save.gameObject.SetActive(false);

                if (_saveReferences.load != null)
                    _saveReferences.load.gameObject.SetActive(false);
            }
            else SaveReferences.Init();
        }

        private string CheckForLastFile()
        {
            string t_path = Application.persistentDataPath + "/";

            var t_root = new DirectoryInfo(t_path);
            var t_dir = t_root.GetDirectories().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

            if (t_dir.Name != "Unity") return t_dir.Name;
            else return null;
        }

        public void Build()
        {
            SceneManager t_sceneManager = SceneManager.Instance;

            DataBuilder.BuildDataReferences();

            //BUILD OBJECTS HERE
            //DataBuilder.BuildElementsOfType<DataElement>(t_sceneManager.DataReferences.SaveData);
        }

        public void GenerateSave()
        {
            if (multipleSaves)
            {
                string t_time = DateTime.Now.ToString();

                t_time = t_time.Replace("/", "-");
                t_time = t_time.Replace(" ", "_");
                t_time = t_time.Replace(":", "-");

                string _path = Application.persistentDataPath + "/";
                if (Directory.Exists(_path + ID + "/"))
                {
                    string t_temp = _path + (ID == _tempID ? "SAVE" : ID);
              
                    t_temp = t_temp.Replace(_path, "");
                    t_temp = t_temp.Replace("-", "");
                    t_temp = t_temp.Replace("_", "");
                    t_temp = t_temp.Replace(":", "");
                    t_temp = t_temp.Replace("PM", "");
                    t_temp = t_temp.Replace("AM", "");

                    for (int i = 0; i < t_temp.Length; i++)
                    {
                        if (char.IsDigit(t_temp[i]))
                            t_temp = t_temp.Replace(t_temp[i], ' ');
                    }
                    t_temp = t_temp.TrimEnd();

                    string t_newPath = t_temp + "_" + t_time;
                    t_newPath = t_newPath.Replace(" ", "_");

                    Directory.CreateDirectory(_path + t_newPath);

                    for (uint i = 0; i < Directory.GetDirectories(_path + ID).Length; i++)
                    {
                        string t_name = Directory.GetDirectories(_path + ID)[i];
                        Directory.CreateDirectory(t_name.Replace(ID, t_newPath));

                        for (uint a = 0; a < Directory.GetFiles(t_name).Length; a++)
                            File.Copy(Directory.GetFiles(t_name)[a], Directory.GetFiles(t_name)[a].Replace(ID, t_newPath));
                    }

                    Debug.Log("Saving Data to: " + t_newPath + "_" + t_time);

                    ID = t_newPath;
                    SaveReferences.Init();
                }
            }
        }
        public void GenerateSave(string p_input)
        {
            string _path = Application.persistentDataPath + "/";
            if (Directory.Exists(_path + ID + "/") && ID == _tempID)
            {
                Directory.CreateDirectory(_path + p_input);

                for (uint i = 0; i < Directory.GetDirectories(_path + ID).Length; i++)
                {
                    string t_name = Directory.GetDirectories(_path + ID)[i];
                    Directory.CreateDirectory(t_name.Replace(ID, p_input));

                    for (uint a = 0; a < Directory.GetFiles(t_name).Length; a++)
                        File.Copy(Directory.GetFiles(t_name)[a], Directory.GetFiles(t_name)[a].Replace(ID, p_input), true);
                }

                Debug.Log("Saving Data to: " + _path + p_input);

                ID = p_input;
                SaveReferences.Init();
            }
        }

        public void Load()
        {
            if (multipleSaves)
            {
                ID = SaveReferences.saveData[SaveReferences.load.value];

                SceneManager t_sceneManager = SceneManager.Instance;

                if (t_sceneManager != null)
                {
                    t_sceneManager.ClearAllData();
                    Build();
                }
                SceneManager.Instance.Reload();
            }
        }

        [ContextMenu("Clear All Data.")]
        public void ClearAllData()
        {
            string t_path = Application.persistentDataPath + "/"; 
            string[] t_data = Directory.GetDirectories(t_path);
            for (uint i = 0; i < t_data.Length; i++)
            {
                if (!t_data[i].Contains("Unity"))
                {
                    Directory.Delete(t_data[i], true);
                    Debug.Log("Cleaning Data from: " + t_data[i]);
                }
            }
        }

        private void OnDestroy()
        {
            string t_temp = Application.persistentDataPath + "/" + _tempID + "/";
            if (Directory.Exists(t_temp))
                Directory.Delete(t_temp, true);
        }
    }
}


