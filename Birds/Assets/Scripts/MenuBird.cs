using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBird : MonoBehaviour
{

    [SerializeField] float _launchForce = 500f;
    [SerializeField] float _maxDragDistance = 5f;

    Vector2 _startPosition;
    Rigidbody2D _rigidbody2D;
    Animator _animator;

    [SerializeField] float _maxYHeight = 0.9f;
    [SerializeField] float _minYHeight = -1.5f;

    [SerializeField] float _dragSpeed;
    private bool _canDrag;


    Vector2 _desiredPosition;
    Vector2 _randomDragPosition;
    Vector2 _direction;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;
        StartCoroutine(StartMenuDrag());
    }

    IEnumerator StartMenuDrag()
    {
        yield return new WaitForSeconds(1);

        _animator.SetBool("_isGrabbed", true);
        AIDrag();
        StartCoroutine(MoveToLaunchPostition());

        yield return new WaitForSeconds(2);
        LetGo();
    }

    void AIDrag()
    {
        _randomDragPosition = new Vector2(-8f, Random.Range(_minYHeight, _maxYHeight));

        _desiredPosition = _randomDragPosition;

        _direction = _desiredPosition - _startPosition;
        _direction.Normalize();

        float distance = Vector2.Distance(_desiredPosition, _startPosition);

        if (distance > _maxDragDistance)
        {
            _desiredPosition = _startPosition + (_direction * _maxDragDistance);
        }

        if (_desiredPosition.x > _startPosition.x)
            _desiredPosition.x = _startPosition.x;
    }

    IEnumerator MoveToLaunchPostition()
    {
        _canDrag = true;
        yield return new WaitForSeconds(2);
        _canDrag = false;

    }

    void FixedUpdate()
    {
        if (_canDrag)
        {
            _rigidbody2D.position = Vector2.MoveTowards(_rigidbody2D.position, _desiredPosition, _dragSpeed * Time.deltaTime);
        }
    }

    void LetGo()
    {
        Vector2 currentPosition = _rigidbody2D.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(direction * _launchForce);

        _animator.SetBool("_isGrabbed", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetAfterDelay());
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
