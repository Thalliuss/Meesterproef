using DataManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ListedEnemyData : DataElement
{
    [Serializable]
    public class EnemyData
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
        [SerializeField]
        private string _id;

        public Vector3 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }
        [SerializeField]
        private Vector3 _position;

        public int Health
        {
            get
            {
                return _health;
            }

            set
            {
                _health = value;
            }
        }
        [SerializeField]
        private int _health;

        public EnemyData(string p_id)
        {
            ID = p_id;
        }
    }
    public List<EnemyData> Enemies
    {
        get
        {
            return _enemies;
        }

        set
        {
            _enemies = value;
        }
    }
    [SerializeField]
    private List<EnemyData> _enemies = new List<EnemyData>();

    public EnemyData FindEnemyDataByID(string p_ID)
    {
        if (_enemies == null || _enemies.Count <= 0) return null;

        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i].ID == p_ID)
                return _enemies[i];
        }
        return null;
    }

    public ListedEnemyData(string p_id, List<EnemyData> p_enemies) : base(p_id)
    {
        Enemies = p_enemies;
    }
}
