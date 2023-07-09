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

    private Vector3 _lastPlayerPosition;
    private Queue<Vector2> _movementQueue;
    private float _currentSpeed = 0f;
    private bool _knowsPlayer = false;
    private Animator _animator;
    private MovementDirection _movementDirection = MovementDirection.RIGHT;



    // Start is called before the first frame update
    void Start()
    {
        _movementQueue = new Queue<Vector2>();
        _currentSpeed = TrainerBaseSpeed;
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 origin = transform.position;
        Vector2 direction = Player.transform.position - transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, SightDistance);
        bool viewBlocked = false;
        foreach (RaycastHit2D hit in hits )
        {
            if (hit.collider.gameObject.GetComponent<Trainer>() != null && hit.collider.gameObject.GetComponent<FurretController>())
            {
                viewBlocked = true; break;
            }
        }
        if (!viewBlocked)
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
            transform.position += new Vector3(finalMovement.x, finalMovement.y);
            if (Mathf.Abs(finalMovement.x) >= Mathf.Abs(finalMovement.y))
            {
                if (finalMovement.x > 0)
                {
                    _movementDirection = MovementDirection.RIGHT;
                }
                else if (finalMovement.x < 0)
                {
                    _movementDirection = MovementDirection.LEFT;
                }
            }
            else
            {
                if (finalMovement.y > 0)
                {
                    _movementDirection = MovementDirection.UP;
                }
                else if (finalMovement.y < 0)
                {
                    _movementDirection = MovementDirection.DOWN;
                }
            }
            if (finalMovement.magnitude > 0)
            {
                _animator.SetInteger("direction", (int)_movementDirection);
            }
        }
    }
}