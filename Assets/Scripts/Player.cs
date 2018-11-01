using DataManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Save Implementation

    [HideInInspector]
    public PlayerData playerData;

    private SceneManager _sceneManager;
    private const string _playerDataID = "PlayerDataID";

    private void Setup()
    {
        _sceneManager = SceneManager.Instance;

        playerData = _sceneManager.DataReferences.FindElement<PlayerData>(_playerDataID);
        if (playerData == null)
        {
            _sceneManager.DataReferences.AddElement<PlayerData>(_playerDataID);
            playerData = _sceneManager.DataReferences.FindElement<PlayerData>(_playerDataID);

            playerData.Health = _health;
            playerData.Position = transform.position;
            playerData.Save();
        }

        _startPosition = transform.position;

        transform.position = LoadPosition();
        StartCoroutine(SavePosition(1));

        _health = LoadHealth();
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

    private void SaveHealth()
    {
        if (playerData == null) return;

        playerData.Health = _health;
        playerData.Save();
    }

    private Vector3 LoadPosition()
    {
        return (playerData != null && playerData.Position != Vector3.zero) ? playerData.Position : transform.position;
    }

    private int LoadHealth()
    {
        return (playerData != null) ? playerData.Health : _health;
    }

    #endregion

    [SerializeField]
    private Vector2 _startPosition;
    [SerializeField]
    private GameObject _sword;
    [SerializeField]
    private Animator _animator;

    private Slider _healthVisual;
    private int _health = 3;

    private void Start()
    {
        if (DataManager.Instance != null) Setup();

        _healthVisual = GameObject.FindGameObjectWithTag("Healthbar").GetComponent<Slider>();
    }

    private void OnTriggerStay2D(Collider2D p_other)
    {
        if (p_other.CompareTag("Box") && Input.GetKey(KeyCode.E))
            p_other.GetComponent<Rigidbody2D>().mass = 4;
        else p_other.GetComponent<Rigidbody2D>().mass = 400;
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.CompareTag("Enemy"))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
            float speed = 150;
            Vector2 force = transform.forward;
            force = new Vector2(force.x, 1);
            GetComponent<Rigidbody2D>().AddForce(force * speed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);

            StartCoroutine(InvincibilityFrames());
        }
    }

    private bool _invincible = false;
    private IEnumerator InvincibilityFrames()
    {
        if (_invincible == false)
        {
            _invincible = true;
            TakeDamage();

            yield return new WaitForSeconds(2f);
            _invincible = false;
        }
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
        _health = 3;
        SaveHealth();

        transform.position = _startPosition;
    }

    private void Update()
    {
        _healthVisual.value = _health;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_sword.GetComponent<SpriteRenderer>().flipX == false)
                _animator.Play("HitRight");
            else _animator.Play("HitLeft");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _sword.GetComponent<SpriteRenderer>().flipX = true;
            _sword.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _sword.GetComponent<SpriteRenderer>().flipX = false;
            _sword.GetComponent<SpriteRenderer>().sortingOrder = 6;
        }
    }
}
