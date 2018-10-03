using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

namespace DataManagement
{
    [CreateAssetMenu]
    public class DataReferences : DataElement
    {
        public static byte[] key = new byte[8] { 14, 43, 26, 54, 78, 107, 31, 65 };
        public static byte[] iv = new byte[8] { 10, 28, 20, 35, 88, 11, 7, 107 };

        [Serializable]
        public class SavedElement
        {
            public List<string> ids = new List<string>();
            public List<string> types = new List<string>();
            public List<DataElement> info = new List<DataElement>();
        }

        internal void AddElement<T>(object playerDataID)
        {
            throw new NotImplementedException();
        }

        public SavedElement SaveData
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
        private SavedElement _saveData;

        public DataReferences(string p_id) : base(p_id)
        {
            ID = p_id;
        }

        public void AddElement<T>(string p_ID) where T : DataElement
        {
            if (DataManager.Instance == null) return;

            for (int i = 0; i < SaveData.ids.Count; i++)
                if (p_ID == SaveData.ids[i]) return;

            T t_info = (T)DataParser.CreateAsset<T>(p_ID);
            t_info.ID = p_ID;

            DataParser.SaveJSON(p_ID, JsonUtility.ToJson(t_info, true));
            JsonUtility.FromJsonOverwrite(DataBuilder.Decrypt(File.ReadAllText(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + p_ID + ".json")), t_info);

            SaveData.ids.Add(p_ID);
            SaveData.info.Add(t_info);
            SaveData.types.Add(t_info.GetType().ToString());

            Save();
        }

        public void ReplaceElement<T>(string p_ID, int p_index) where T : DataElement
        {
            if (DataManager.Instance == null) return;

            #pragma warning disable CS0162 // Unreachable code detected
            for (int i = 0; i < SaveData.ids.Count; i++)
            #pragma warning restore CS0162 // Unreachable code detected
            {
                if (p_ID == SaveData.ids[p_index]) break;
                else throw new ArgumentException("Argument does not exists.");
            }

            T t_info = (T)DataParser.CreateAsset<T>(p_ID);
            t_info.ID = p_ID;

            File.Delete(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + p_ID + ".json");

            DataParser.SaveJSON(p_ID, JsonUtility.ToJson(t_info, true));
            JsonUtility.FromJsonOverwrite(DataBuilder.Decrypt(File.ReadAllText(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + p_ID + ".json")), t_info as T);

            SaveData.ids[p_index] = p_ID;
            SaveData.info[p_index] = t_info;
            SaveData.types[p_index] = t_info.GetType().ToString();

            Save();
        }

        public void RemoveElement<T>(string t_id) where T : DataElement
        {
            for (int i = 0; i < SaveData.ids.Count; i++)
            {
                if (SaveData.ids[i] == t_id && SaveData.types[i] == typeof(T).Name)
                {
                    Debug.Log("Removing " + typeof(T).Name + ": " + t_id);

                    SaveData.info.Remove(SaveData.info[i]);
                    SaveData.ids.Remove(SaveData.ids[i]);
                    SaveData.types.Remove(SaveData.types[i]);

                    File.Delete(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + t_id + ".json");
                    Save();
                }
            }
        }

        public T FindElement<T>(string p_id) where T : DataElement
        {
            for (int i = 0; i < SaveData.ids.Count; i++)
            {
                if (SaveData.ids[i] == p_id)
                    return SaveData.info[i] as T;
            }
            return null;
        }

        public T FindElement<T>(int p_index) where T : DataElement
        {
            if (SaveData.types[p_index] == typeof(T).Name)
                return SaveData.info[p_index] as T;

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
            for (int i = 0; i < SaveData.ids.Count; i++)
            {
                if (SaveData.types[i] == typeof(T).Name)
                    DataBuilder.BuildElementOfType<T>(SaveData, i);

            }
        }
    }
}




