using UnityEngine;

using System;
using System.IO;
using System.Collections.Generic;

namespace DataManagement
{
    public abstract class Constructor<T> : ScriptableObject
    {
        public Constructor(T id)
        {

        }
    }

    public class DataElement : Constructor<string>
    {
        public string ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
        [Header("Element's ID:"), SerializeField]
        private string _id;

        public DataReferences.SavedElement SaveData
        {
            get
            {
                return _saveData;
            }

            set
            {
                _saveData = value;
            }
        }
        [Header("Element's SaveData:"), SerializeField]
        private DataReferences.SavedElement _saveData;

        public DataElement(string p_id) : base(p_id)
        {
            _id = ID;
        }

        public void AddElement<T>(string p_ID) where T : DataElement
        {
            if (DataManager.Instance == null) return;

            for (int i = 0; i < _saveData.ids.Count; i++)
                if (p_ID == _saveData.ids[i]) return;

            T t_info = (T)DataParser.CreateAsset<T>(p_ID);
            t_info.ID = p_ID;

            DataParser.SaveJSON(p_ID, JsonUtility.ToJson(t_info, true));
            JsonUtility.FromJsonOverwrite(DataBuilder.Decrypt(File.ReadAllText(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + p_ID + ".json")), t_info);

            _saveData.ids.Add(p_ID);
            _saveData.info.Add(t_info);
            _saveData.types.Add(t_info.GetType().ToString());

            Save();
        }

        public void ReplaceElement<T>(string p_ID, int p_index) where T : DataElement
        {
            if (DataManager.Instance == null) return;

            #pragma warning disable CS0162 // Unreachable code detected
            for (int i = 0; i < _saveData.ids.Count; i++)
            #pragma warning restore CS0162 // Unreachable code detected
            {
                if (p_ID == _saveData.ids[p_index]) break;
                else throw new ArgumentException("Argument does not exists.");
            }
             
            T t_info = (T)DataParser.CreateAsset<T>(p_ID);
            t_info.ID = p_ID;

            File.Delete(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + p_ID + ".json");

            DataParser.SaveJSON(p_ID, JsonUtility.ToJson(t_info, true));
            JsonUtility.FromJsonOverwrite(DataBuilder.Decrypt(File.ReadAllText(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + p_ID + ".json")), t_info as T);

            _saveData.ids[p_index] = p_ID;
            _saveData.info[p_index] = t_info;
            _saveData.types[p_index] = t_info.GetType().ToString();

            Save();
        }

        public void RemoveElement<T>(string t_id) where T : DataElement
        {
            for (int i = 0; i < _saveData.ids.Count; i++)
            {
                if (_saveData.ids[i] == t_id && _saveData.types[i] == typeof(T).Name)
                {
                    Debug.Log("Removing " + typeof(T).Name + ": " + t_id);

                    _saveData.info.Remove(_saveData.info[i]);
                    _saveData.ids.Remove(_saveData.ids[i]);
                    _saveData.types.Remove(_saveData.types[i]);

                    File.Delete(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + t_id + ".json");
                    Save();
                }
            }
        }

        public T FindElement<T>(string p_id) where T : DataElement
        {
            for (int i = 0; i < _saveData.ids.Count; i++)
            {
                if (_saveData.ids[i] == p_id)
                    return _saveData.info[i] as T;
            }
            return null;
        }

        public T FindElement<T>(int p_index) where T : DataElement
        {
            if (_saveData.types[p_index] == typeof(T).Name)
                return _saveData.info[p_index] as T;

            return null;
        }

        public List<T> FindElementsOfType<T>() where T : DataElement
        {
            List<T> t_temp = new List<T>();
            for (int i = 0; i < SaveData.ids.Count; i++)
            {
                if (SaveData.types[i] == typeof(T).Name)
                    t_temp.Add(SaveData.info[i] as T);
            }
            t_temp.Reverse();
            return t_temp;
        }

        public void Build<T>() where T : DataElement
        {
            for (int i = 0; i < _saveData.ids.Count; i++)
            {
                if (_saveData.types[i] == typeof(T).Name)
                    DataBuilder.BuildElementOfType<T>(_saveData, i);

                if (_saveData.info[i] != null)
                {
                    for (int a = 0; a < _saveData.info[i].SaveData.ids.Count; a++)
                    {
                        if (_saveData.info[i].SaveData.types[a] == typeof(T).Name)
                            _saveData.info[i].Build<T>();
                    }
                }
            }
        }

        public void Save()
        {
            DataParser.SaveJSON(_id.ToString(), JsonUtility.ToJson(this, true));
            JsonUtility.FromJsonOverwrite(DataBuilder.Decrypt(File.ReadAllText(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + _id.ToString() + ".json")), this);
            Debug.Log("Saving Data to: " + Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID);
        }

        public void Destroy()
        {
            _saveData.ids.Clear();
            _saveData.info.Clear();
            _saveData.types.Clear();
        }
    }
}

