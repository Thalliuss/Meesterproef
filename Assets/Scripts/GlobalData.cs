using DataManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : DataElement
{
    [SerializeField]
    private int _currentLevel;
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

    [SerializeField]
    private float _timer;
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
    [SerializeField]
    private List<Highscore> _highscores = new List<Highscore>();
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

    public GlobalData(string p_id, int p_currentLevel, float p_timer) : base(p_id)
    {
        ID = p_id;
        CurrentLevel = p_currentLevel;
        Timer = p_timer;
    }
}
