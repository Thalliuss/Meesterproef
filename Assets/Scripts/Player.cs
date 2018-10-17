using DataManagement;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SceneManager _sceneManager;

    [HideInInspector]
    public PlayerData playerData;

    private const string _playerDataID = "PlayerDataID";

    private void Start()
    {
        Setup();

        transform.position = LoadPosition();
        StartCoroutine(SavePosition(1));
    }

    private void Setup()
    {
        _sceneManager = SceneManager.Instance;

        playerData = _sceneManager.DataReferences.FindElement<PlayerData>(_playerDataID);
        if (playerData == null)
        {
            _sceneManager.DataReferences.AddElement<PlayerData>(_playerDataID);
            playerData = _sceneManager.DataReferences.FindElement<PlayerData>(_playerDataID);
        }
    }

    private IEnumerator SavePosition(float p_input)
    {
        while (true && playerData != null)
        {
            yield return new WaitForSeconds(p_input);

            playerData.Position = transform.position;
            playerData.Save();
        }
    }

    private Vector3 LoadPosition()
    {
        if (playerData == null) return transform.position;

        return (playerData.Position != Vector3.zero) ? playerData.Position : transform.position;
    }
}
