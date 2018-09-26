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

    public PlayerData(string p_id, Vector3 p_position) : base(p_id)
    {
        ID = p_id;
        Position = p_position;
    }
}
