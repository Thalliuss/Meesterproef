using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject _target;

    [SerializeField]
    private List<Vector2> _positions;

    private enum EnemyType
    {
        Patrolling,
        Following
    }

    [SerializeField]
    private EnemyType _enemyType;

    private Vector2 _startPosition;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");

        _startPosition = transform.position;
    }

    private int _current = 0;
    private void Update()
    {
        if (_enemyType == EnemyType.Patrolling)
        {
            transform.position = Vector2.MoveTowards(transform.position, _startPosition + _positions[_current], 2f * Time.deltaTime);
            if ((Vector2)transform.position == _startPosition + _positions[_current])
            {
                _current++;
                if (_current > _positions.Count - 1) _current = 0;
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            }
        }
        if (_enemyType == EnemyType.Following)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, 2f * Time.deltaTime);
        }
    }
}
