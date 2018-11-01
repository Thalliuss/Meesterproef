using DataManagement;
using UnityEngine;

public class PlayerData : DataElement
{
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

    public PlayerData(string p_id, Vector3 p_position, int p_health) : base(p_id)
    {
        Position = p_position;
        Health = p_health;
    }
}
