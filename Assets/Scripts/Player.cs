using DataManagement;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SceneManager _sceneManager;

    [SerializeField]
    private PlayerData _playerData;

    private const string _playerDataID = "PlayerDataID";

    private void Start()
    {
        Setup();

        transform.position = LoadPosition();
        StartCoroutine(SavePosition(1));
    }

    private IEnumerator SavePosition(float p_input)
    {
        while (true)
        {
            yield return new WaitForSeconds(p_input);

            _playerData.Position = transform.position;
            _playerData.Save();
        }
    }

    private Vector3 LoadPosition()
    {
        return (_playerData.Position != Vector3.zero) ? _playerData.Position : transform.position;
    }

    private void Setup()
    {
        _sceneManager = SceneManager.Instance;

        _playerData = _sceneManager.DataReferences.FindElement<PlayerData>(_playerDataID);
        if (_playerData == null)
        {
            _sceneManager.DataReferences.AddElement<PlayerData>(_playerDataID);
            _playerData = _sceneManager.DataReferences.FindElement<PlayerData>(_playerDataID);
        }
    }
}
