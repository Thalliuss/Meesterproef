using DataManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Save Implementation

    public PlayerData PlayerData { get; set; }

    private SceneManager _sceneManager;
    private const string _playerDataID = "PlayerDataID";

    private void Setup()
    {
        _sceneManager = SceneManager.Instance;

        PlayerData = _sceneManager.DataReferences.FindElement<PlayerData>(_playerDataID);
        if (PlayerData == null)
        {
            _sceneManager.DataReferences.AddElement<PlayerData>(_playerDataID);
            PlayerData = _sceneManager.DataReferences.FindElement<PlayerData>(_playerDataID);

            PlayerData.Health = _health;
            PlayerData.Position = transform.position;
            PlayerData.Save();
        }

        _startPosition = transform.position;

        transform.position = LoadPosition();
        StartCoroutine(SavePosition(1f));

        _health = LoadHealth();
    }

    private IEnumerator SavePosition(float p_input)
    {
        while (true && PlayerData != null)
        {
            yield return new WaitForSeconds(p_input);

            PlayerData.Position = transform.position;
            PlayerData.Save();
        }
    }

    private void SaveHealth()
    {
        if (PlayerData == null) return;

        PlayerData.Health = _health;
        PlayerData.Save();
    }

    private Vector3 LoadPosition()
    {
        return (PlayerData != null && PlayerData.Position != Vector3.zero) ? PlayerData.Position : transform.position;
    }

    private int LoadHealth()
    {
        return (PlayerData != null) ? PlayerData.Health : _health;
    }

    #endregion

    [SerializeField] private Vector2 _startPosition;
    [SerializeField] private Slider _healthVisual;

    private int _health = 3;

    private void Start()
    {
        if (DataManager.Instance != null)
            Setup();

        if (GameManager.Instance != null)
            _healthVisual = GameManager.Instance.HealthVisual;
    }

    private void OnTriggerStay2D(Collider2D p_other)
    {
        if (p_other.CompareTag("Enemy") && p_other.GetType() == typeof(BoxCollider2D))
            StartCoroutine(InvincibilityFrames(1));

        if (p_other.CompareTag("Flag"))
            GameManager.Instance.NextLevel();

        if (p_other.CompareTag("Box") && Input.GetKey(KeyCode.E)) p_other.GetComponent<Rigidbody2D>().mass = 4;
        else p_other.GetComponent<Rigidbody2D>().mass = 400;

        if (p_other.CompareTag("Spike"))
            StartCoroutine(InvincibilityFrames(1));
    }

    /// Private variable used only in InvincibilityFrames();
    private bool _invincible = false;
    /// ===================================================
    private IEnumerator InvincibilityFrames(float p_input)
    {
        if (_invincible == false)
        {
            _invincible = true;
            TakeDamage();

            yield return new WaitForSeconds(p_input);
            _invincible = false;
        }
    }

    public void TakeDamage()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
        float speed = 500;
        Vector2 force = transform.forward;
        force = new Vector2(force.x, 1);
        GetComponent<Rigidbody2D>().AddForce(force * speed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);

        _health--;
        SaveHealth();

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        _health = 3;
        SaveHealth();

        transform.position = _startPosition;
    }

    private void Update()
    {
        if(_healthVisual != null)
            _healthVisual.value = _health;

        if (transform.position.y <= -15f)
            Die();
    }
}
