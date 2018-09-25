using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;
using System.IO;

namespace DataManagement
{
    [Serializable]
    public class SaveReferences
    {
        public Dropdown load;
        public Button save;
        public List<string> saveData = new List<string>();

        public void Init()
        {
            if (saveData != null) saveData.Clear();
            if (load.options != null) load.options.Clear();

            string t_path = Application.persistentDataPath + "/";
            string[] t_data = Directory.GetDirectories(t_path);
            for (uint i = 0; i < t_data.Length; i++)
            {
                t_data[i] = t_data[i].Replace(t_path, "");

                if (!t_data[i].Contains("Unity") && !t_data[i].Contains("temp"))
                {
                    saveData.Add(t_data[i]);

                    if (t_data[i] != DataManager.Instance.ID)
                        load.options.Add(new Dropdown.OptionData(t_data[i]));
                    
                    else load.options.Add(new Dropdown.OptionData(DataManager.Instance.ID));
                }
            }
        }
    }
}