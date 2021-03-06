﻿using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private List<Vector2> _positions;
    [SerializeField] private GameObject _patrolling;
    [SerializeField] private GameObject _following;
    [SerializeField] private AudioSource _audio;

    private enum EnemyType
    {
        Patrolling,
        Following
    }
    [SerializeField] private EnemyType _enemyType;

    private GameObject _target;

    private Vector2 _startPosition;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _startPosition = transform.position;
    }

    /// Private variable used only in OnTriggerEnter2D(); & OnTriggerExit2D();
    private bool _colliding = false;
    /// =====================================================================
    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.name == "Player")
            _colliding = true;
    }
    private void OnTriggerExit2D(Collider2D p_other)
    {
        if (p_other.name == "Player")
            _colliding = false;
    }

    /// Private variables used only in Update();
    private int _current = 0;
    private int _scale = 1;
    /// ========================================
    private void Update()
    {
        if (_enemyType == EnemyType.Patrolling)
        {
            _patrolling.SetActive(true);

            transform.position = Vector2.MoveTowards(transform.position, _startPosition + _positions[_current], 2f * Time.deltaTime);
            if ((Vector2)transform.position == _startPosition + _positions[_current])
            {
                _current++;
                if (_current > _positions.Count - 1) _current = 0;

                _scale = (_scale == 1) ? _scale = -1 : _scale = 1;

                transform.localScale = new Vector3(_scale, transform.localScale.y, transform.localScale.z);
            }
        }
        if (_enemyType == EnemyType.Following)
        {
            _following.SetActive(true);

            if (_colliding)
            {
                transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, 2f * Time.deltaTime);

                if (!_audio.isPlaying)
                    _audio.Play();
            }
            else
            {
                if (_audio.isPlaying)
                    _audio.Stop();
            }
        }
    }
}

