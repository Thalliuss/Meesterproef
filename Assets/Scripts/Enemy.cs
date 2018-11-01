using DataManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Save Implementation 

    [HideInInspector]
    public ListedEnemyData.EnemyData enemyData;

    [SerializeField]
    private string _enemyDataID;

    private SceneManager _sceneManager;
    private ListedEnemyData _listedEnemyData;
    private const string _listedenemyDataID = "ListedEnemyData";

    private void Setup()
    {
        _sceneManager = SceneManager.Instance;

        _listedEnemyData = _sceneManager.DataReferences.FindElement<ListedEnemyData>(_listedenemyDataID);
        if (_listedEnemyData == null)
        {
            _sceneManager.DataReferences.AddElement<ListedEnemyData>(_listedenemyDataID);
            _listedEnemyData = _sceneManager.DataReferences.FindElement<ListedEnemyData>(_listedenemyDataID);
        }

        if (_listedEnemyData == null) return;

        enemyData = _listedEnemyData.FindEnemyDataByID(_enemyDataID);
        if (enemyData == null)
        {
            ListedEnemyData.EnemyData t_enemy = new ListedEnemyData.EnemyData(_enemyDataID);
            t_enemy.Health = _health;
            t_enemy.Position = transform.position;

            _listedEnemyData.Enemies.Add(t_enemy);
            _listedEnemyData.Save();

            enemyData = _listedEnemyData.FindEnemyDataByID(_enemyDataID);
        }

        transform.position = LoadPosition();
        StartCoroutine(SavePosition(1));

        _health = LoadHealth();
        if (_health <= 0)
            Die();
    }

    private IEnumerator SavePosition(int p_input)
    {
        while (true && _listedEnemyData != null && enemyData != null)
        {
            yield return new WaitForSeconds(p_input);

            enemyData.Position = transform.position;
            _listedEnemyData.Save();
        }
    }

    private void SaveHealth()
    {
        if (enemyData == null || _listedEnemyData == null) return;

        enemyData.Health = _health;
        _listedEnemyData.Save();
    }

    private Vector3 LoadPosition()
    {
        return (enemyData != null) ? enemyData.Position : transform.position;
    }

    private int LoadHealth()
    {
        return (enemyData != null) ? enemyData.Health : _health;
    }

    #endregion

    [SerializeField]
    private Slider _healthVisual;

    private int _health = 3;

    private void Start()
    {
        if (DataManager.Instance != null) Setup(); 
    }

    [ContextMenu("TakeDamage")]
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
