using DataManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Save Implementation 

    [SerializeField] private string _enemyDataID;

    const string _glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    private const string _listedenemyDataID = "ListedEnemyData";

    private SceneManager _sceneManager;
    private ListedEnemyData _listedEnemyData;
    private ListedEnemyData.EnemyData _enemyData;

    private void Setup()
    {
        _sceneManager = SceneManager.Instance;

        _listedEnemyData = _sceneManager.DataReferences.FindElement<ListedEnemyData>(_listedenemyDataID);
        if (_listedEnemyData == null)
        {
            _sceneManager.DataReferences.AddElement<ListedEnemyData>(_listedenemyDataID);
            _listedEnemyData = _sceneManager.DataReferences.FindElement<ListedEnemyData>(_listedenemyDataID);
            return;
        }

        _enemyData = _listedEnemyData.FindEnemyDataByID(_enemyDataID);
        if (_enemyData == null)
        {
            ListedEnemyData.EnemyData t_enemy = new ListedEnemyData.EnemyData(_enemyDataID)
            {
                Health = _health,
                Position = transform.position
            };

            _listedEnemyData.Enemies.Add(t_enemy);
            _listedEnemyData.Save();

            _enemyData = _listedEnemyData.FindEnemyDataByID(_enemyDataID);
        }

        transform.position = LoadPosition();
        StartCoroutine(SavePosition(1));

        _health = LoadHealth();

        if (_health <= 0)
            Die();
    }

    private IEnumerator SavePosition(int p_input)
    {
        while (true && _listedEnemyData != null && _enemyData != null)
        {
            yield return new WaitForSeconds(p_input);

            _enemyData.Position = transform.position;
            _listedEnemyData.Save();
        }
    }

    private void SaveHealth()
    {
        if (_enemyData == null || _listedEnemyData == null) return;

        _enemyData.Health = _health;
        _listedEnemyData.Save();
    }

    private Vector3 LoadPosition()
    {
        return (_enemyData != null) ? _enemyData.Position : transform.position;
    }

    private int LoadHealth()
    {
        return (_enemyData != null) ? _enemyData.Health : _health;
    }

    [ContextMenu("PickRandomID")]
    public void PickRandomID()
    {
        string t_temp = null;
        for (int i = 0; i < 6; i++)
        {
            t_temp += _glyphs[Random.Range(0, _glyphs.Length)];
        }
        _enemyDataID = t_temp;
    }

    #endregion

    [SerializeField] private Slider _healthVisual;

    private int _health = 3;

    private void Start()
    {
        if (DataManager.Instance != null)
            Setup(); 
    }

    public void TakeDamage()
    {
        _health--;
        SaveHealth();

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        _healthVisual.value = _health;
    }
}
