using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBird : MonoBehaviour
{

    [SerializeField] float _launchForce = 1000f;
    [SerializeField] float _maxDragDistance = 3f;
    [SerializeField] float _maxYHeight = 0.9f;
    [SerializeField] float _minYHeight = -1.5f;
    [SerializeField] float _dragSpeed = 10f;

    Rigidbody2D _rigidbody2D;
    Animator _animator;

    Vector2 _startPosition;
    Vector2 _randomDragPosition;
    Vector2 _direction;

    bool _canDrag;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Cache _startPosition and set RigidBody2D to Kinematic
        _startPosition = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;

        StartCoroutine(StartMenuDrag());
    }

    // Starting Co-routine to handle bird launching
    IEnumerator StartMenuDrag()
    {
        yield return new WaitForSeconds(1);
        _animator.SetBool("_isGrabbed", true);

        AIDrag();
        StartCoroutine(MoveToLaunchPostition());

        yield return new WaitForSeconds(2);
        LetGo();

        yield return new WaitForSeconds(1);
        StartCoroutine(ResetAfterDelay());
    }

    // Creates a random position for the bird to eventually move towards.
    void AIDrag()
    {
        _randomDragPosition = new Vector2(-8f, Random.Range(_minYHeight, _maxYHeight));


        _direction = _randomDragPosition - _startPosition;
        _direction.Normalize();

        float distance = Vector2.Distance(_randomDragPosition, _startPosition);

        if (distance > _maxDragDistance)
        {
            _randomDragPosition = _startPosition + (_direction * _maxDragDistance);
        }

        if (_randomDragPosition.x > _startPosition.x)
            _randomDragPosition.x = _startPosition.x;
    }

    // Turns bool on, then back off to allow movement in FixedUpdate
    IEnumerator MoveToLaunchPostition()
    {
        _canDrag = true;
        yield return new WaitForSeconds(2);
        _canDrag = false;
    }

    // Moving Rigidbody2D position towards the random position
    void FixedUpdate()
    {
        if (_canDrag)
        {
            _rigidbody2D.position = Vector2.MoveTowards(_rigidbody2D.position, _randomDragPosition, _dragSpeed * Time.deltaTime);
        }
    }

    // Launches Bird by finding the direction, then adding a force
    void LetGo()
    {
        Vector2 currentPosition = _rigidbody2D.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(direction * _launchForce);

        _animator.SetBool("_isGrabbed", false);
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(3);
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
        StartCoroutine(StartMenuDrag());
    }
}
