using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class Trainer : MonoBehaviour
{

    public GameObject Player;
    public float SightDistance = 10.0f;
    public float TrainerBaseSpeed = 0.02f;
    public float MinimumMovementTolerance = 0.01f;

    private Collider2D _playerCollider;
    private Vector3 _lastPlayerPosition;
    private Queue<Vector2> _movementQueue;
    private float _currentSpeed = 0f;
    private bool _knowsPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        _movementQueue = new Queue<Vector2>();
        _playerCollider = Player.GetComponent<CircleCollider2D>();
        _currentSpeed = TrainerBaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 origin = transform.position;
        Vector2 direction = Player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, SightDistance);
        if (hit.collider == _playerCollider)
        {
            _lastPlayerPosition = Player.transform.position;
            _knowsPlayer = true;
        }
        Vector2 movementDirection = _lastPlayerPosition - transform.position;
        movementDirection.Normalize();
        Vector2 finalMovement = movementDirection * _currentSpeed;
        Vector2 distance = _lastPlayerPosition - transform.position;

        if (finalMovement.magnitude > distance.magnitude)
        {
            finalMovement = distance;
        }
        if (finalMovement.magnitude < MinimumMovementTolerance)
        {
            _knowsPlayer = false;
        }
        if (_knowsPlayer)
        {
            _movementQueue.Enqueue(finalMovement);
        }
        while (_movementQueue.Count > 0)
        {
            Vector2 qMove = _movementQueue.Dequeue();
            transform.position += new Vector3(qMove.x, qMove.y);
        }
    }
}
