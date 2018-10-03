using System.IO;
using UnityEngine;

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

        public DataElement(string p_id) : base(p_id)
        {
            _id = ID;
        }

        public void Save()
        {
            DataParser.SaveJSON(_id.ToString(), JsonUtility.ToJson(this, true));
            JsonUtility.FromJsonOverwrite(DataBuilder.Decrypt(File.ReadAllText(Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + _id.ToString() + ".json")), this);
        }
    }
}

