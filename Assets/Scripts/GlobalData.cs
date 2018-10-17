using DataManagement;
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

    public GlobalData(string p_id, int p_currentLevel, float p_timer) : base(p_id)
    {
        ID = p_id;
        CurrentLevel = p_currentLevel;
        Timer = p_timer;
    }
}
