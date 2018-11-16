using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    [Header("Ground layer:")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Movement settings:")]
    [SerializeField] private float _moveForce;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxVelocity;

    [Header("Camera settings:")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _followSpeed;

    [Header("Animator")]
    [SerializeField] private Animator _animator;

    private void Start ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _camera.transform.position = (Vector2)transform.position + _offset;
    }

    private enum Direction
    {
        Right,
        Left
    }
    private void Move(Direction p_input)
    {
        if (p_input == Direction.Right)
        {
            _animator.Play("Walk");

            if (Mathf.Abs(_rigidbody.velocity.x) < _maxVelocity)
                _rigidbody.AddForce(Vector2.right * 1 * _moveForce, ForceMode2D.Impulse);

            if (!IsGrounded())
            {
                _rigidbody.transform.Translate(new Vector2(.1f, 0));
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 10, _rigidbody.velocity.y);
            }

            if (_rigidbody.velocity.x > 0)
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

            //_spriteRenderer.flipX = false;
        }
        if (p_input == Direction.Left)
        {
            _animator.Play("Walk");

            if (Mathf.Abs(_rigidbody.velocity.x) < _maxVelocity)
                _rigidbody.AddForce(Vector2.left * 1 * _moveForce, ForceMode2D.Impulse);

            if (!IsGrounded())
            {
                _rigidbody.transform.Translate(new Vector2(-.1f, 0));
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 10, _rigidbody.velocity.y);
            }

            if (_rigidbody.velocity.x < 0)
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                
            //_spriteRenderer.flipX = true;
        }
    }

    private void Slash()
    {
        _animator.Play("Slash");
    }

    private bool IsGrounded()
    {
        var t_spriteRotation = (!_spriteRenderer.flipX) ? .2f : -.2f;

        Vector2 t_position = new Vector3(transform.position.x - t_spriteRotation, transform.position.y, transform.position.z); ;
        Vector2 t_direction = Vector2.down;
        float distance = .8f;

        Debug.DrawRay(t_position, t_direction.normalized * distance, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(t_position, t_direction * distance, distance, _groundLayer);
        if (hit.collider != null)
            return true;

        return false;
    }

    private void FixedUpdate()
    {
        _camera.transform.position = Vector2.Lerp(_camera.transform.position, (Vector2)transform.position + _offset, _followSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
        {
            Move(Direction.Left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(Direction.Right);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Slash();
            }
        }

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
            _rigidbody.AddForce(Vector2.up * 1 * _jumpForce, ForceMode2D.Impulse);


    }
}
