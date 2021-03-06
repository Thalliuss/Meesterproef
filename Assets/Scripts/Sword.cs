﻿using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private BoxCollider2D _collider;

    [SerializeField] private AudioSource _audio;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.CompareTag("Enemy"))
            StartCoroutine(InvincibilityFrames(p_other.transform.parent.GetComponent<Enemy>()));
    }

    /// Private variable used only in InvincibilityFrames();
    private bool _invincible = false;
    /// ===================================================
    private IEnumerator InvincibilityFrames(Enemy p_other)
    {
        if (_invincible == false)
        {
            _invincible = true;
            p_other.TakeDamage();

            yield return new WaitForSeconds(.2f);
            _invincible = false;

            if (!_audio.isPlaying)
                _audio.Play();
        }
    }

    /// Private variable used only in Hit();
    private bool _hitting = false;
    /// ===================================
    private IEnumerator Hit()
    {
        if (_hitting == false)
        {
            _hitting = true;

            yield return new WaitForSeconds(.2f);
            _hitting = false;
        }
    }

    private void Update()
    {
        _collider.enabled = _hitting;

        if (Input.GetKey(KeyCode.Mouse0))
            StartCoroutine(Hit());      
    }
}
