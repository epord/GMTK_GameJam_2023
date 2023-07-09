using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class FurretController : MonoBehaviour
{
    // Constants that can be edited from UI
    public float NormalSpeed = 1f;
    public float ChangeDirectionTolerance = 0.1f;
    private float _currentSpeed;
    private PlayerControls _playerControls;
    private Animator _animator;

    private MovementDirection _movementDirection = MovementDirection.RIGHT;
    private Vector2 _lastMovement = Vector2.zero;
    private Queue<Vector2> _movementQueue;

    private void Awake()
    {
        _movementQueue = new Queue<Vector2>();
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerControls.Overworld.Move.performed += MoveCharacter;
        _currentSpeed = NormalSpeed;
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputMove = _playerControls.Overworld.Move.ReadValue<Vector2>();
        Debug.Log(inputMove);

        // Get the proper direction
        MovementDirection newMoveDirection = MovementDirection.RIGHT;
        if (Mathf.Abs(inputMove.y) * 2 <= Mathf.Abs(inputMove.x))
        {
            if (inputMove.x >= 0)
            {
                newMoveDirection = MovementDirection.RIGHT;
            }
            else
            {
                newMoveDirection = MovementDirection.LEFT;
            }
        }
        else if (Mathf.Abs(inputMove.x) * 2 <= Mathf.Abs(inputMove.y))
        {
            if (inputMove.y >= 0)
            {
                newMoveDirection = MovementDirection.UP;
            }
            else
            {
                newMoveDirection = MovementDirection.DOWN;
            }
        }
        else if (inputMove.x >= 0 && inputMove.y >= 0)
        {
            newMoveDirection = MovementDirection.UP_RIGHT;
        }
        else if (inputMove.x >= 0 && inputMove.y <= 0)
        {
            newMoveDirection = MovementDirection.DOWN_RIGHT;
        }
        else if (inputMove.x <= 0 && inputMove.y >= 0)
        {
            newMoveDirection = MovementDirection.UP_LEFT;
        }
        else if (inputMove.x <= 0 && inputMove.y <= 0)
        {
            newMoveDirection = MovementDirection.DOWN_LEFT;
        }
        // Do not update the direction animation if the movement is too small
        if (Mathf.Abs(inputMove.x) >= ChangeDirectionTolerance || Mathf.Abs(inputMove.y) >= ChangeDirectionTolerance)
        {
            _movementDirection = newMoveDirection;
        }
        _animator.SetInteger("direction", (int)_movementDirection);
        // Turn off the animator when standing still
        _animator.SetBool("moving", inputMove.x != 0 || inputMove.y != 0);
        _movementQueue.Enqueue(inputMove * _currentSpeed);
    }

    void FixedUpdate()
    {
        while (_movementQueue.Count > 0)
        {
            Vector2 qMove = _movementQueue.Dequeue();
            transform.position += new Vector3(qMove.x, qMove.y);
        }
    }
}

   

public enum MovementDirection
{
    UP=0,
    UP_RIGHT=1,
    RIGHT=2,
    DOWN_RIGHT=3,
    DOWN=4,
    DOWN_LEFT=5,
    LEFT=6,
    UP_LEFT=7,
}