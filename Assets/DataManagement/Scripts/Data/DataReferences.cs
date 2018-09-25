using UnityEngine;

using System;
using System.Collections.Generic;

namespace DataManagement
{
    [CreateAssetMenu]
    public class DataReferences : DataElement
    {
        public List<T> FindDataOfType<T>() where T : DataElement
        {
            List<T> t_temp = new List<T>();
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].GetType() == typeof(T))
                    t_temp.Add(_data[i] as T);
            }
            t_temp.Reverse();
            return t_temp;
        }

        public T FindDataElement<T>(string p_id) where T : DataElement
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].ID == p_id)
                    return _data[i] as T;
            }
            return null;
        }

        public T FindDataElement<T>(int p_index) where T : DataElement
        {
            if (_data[p_index].GetType() == typeof(T))
                return _data[p_index] as T;

            return null;
        }

        [Header("Reference Data:"), SerializeField]
        private List<DataElement> _data = new List<DataElement>();

        public static byte[] key = new byte[8] { 14, 43, 26, 54, 78, 107, 31, 65 };
        public static byte[] iv = new byte[8] { 10, 28, 20, 35, 88, 11, 7, 107 };

        [Serializable]
        public class SavedElement
        {
            public List<string> ids = new List<string>();
            public List<string> types = new List<string>();
            public List<DataElement> info = new List<DataElement>();
        }

        public DataReferences(string p_id) : base(p_id)
        {
            ID = p_id;
        }
    }
}




