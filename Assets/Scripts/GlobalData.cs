using DataManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : DataElement
{
    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }

        set
        {
            _currentLevel = value;
        }
    }
    [SerializeField] private int _currentLevel;

    public float Timer
    {
        get
        {
            return _timer;
        }

        set
        {
            _timer = value;
        }
    }
    [SerializeField] private float _timer;

    [Serializable]
    public class Highscore
    {
        [SerializeField]
        private float _score;
        public float Score
        {
            get
            {
                return _score;
            }

            set
            {
                _score = value;
            }
        }
    }
    public List<Highscore> Highscores
    {
        get
        {
            return _highscores;
        }

        set
        {
            _highscores = value;
        }
    }
    [SerializeField] private List<Highscore> _highscores = new List<Highscore>();

    public GlobalData(string p_id, int p_currentLevel, float p_timer) : base(p_id)
    {
        ID = p_id;
        CurrentLevel = p_currentLevel;
        Timer = p_timer;
    }
}
