using UnityEngine;
using UnityEngine.InputSystem;

public class FurretController : MonoBehaviour
{
    // Constants that can be edited from UI
    public float NormalSpeed = 1f;
    private float _currentSpeed;
    private PlayerControls _playerControls;
    private Animator _animator;

    private void Awake()
    {
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
        Vector2 move = _playerControls.Overworld.Move.ReadValue<Vector2>();
        move *= _currentSpeed;
        transform.position += new Vector3(move.x, move.y);

        this.ApplyAnimation();
    }
    
    private void ApplyAnimation()
    {
        Vector2 moveDirection = _playerControls.Overworld.Move.ReadValue<Vector2>();
        float tolerance = 0.25f;

        bool moving = true;

        if (Mathf.Abs(moveDirection.y - 0.5f) < tolerance && Mathf.Abs(moveDirection.x - 0.5f) < tolerance)
        {
            _animator.SetInteger("direction", (int)MovementDirection.UP_LEFT);
        }
        if (Mathf.Abs(moveDirection.y - 0.5f) < tolerance && Mathf.Abs(moveDirection.x + 0.5f) < tolerance)
        {
            _animator.SetInteger("direction", (int)MovementDirection.UP_RIGHT);
        }
        else if (Mathf.Abs(moveDirection.y - 1f) < tolerance && Mathf.Abs(moveDirection.x) < tolerance)
        {
            _animator.SetInteger("direction", (int)MovementDirection.UP);
        }
        if (Mathf.Abs(moveDirection.y + 0.5f) < tolerance && Mathf.Abs(moveDirection.x - 0.5f) < tolerance)
        {
            _animator.SetInteger("direction", (int)MovementDirection.DOWN_LEFT);
        }
        if (Mathf.Abs(moveDirection.y + 0.5f) < tolerance && Mathf.Abs(moveDirection.x + 0.5f) < tolerance)
        {
            _animator.SetInteger("direction", (int)MovementDirection.DOWN_RIGHT);
        }
        else if (Mathf.Abs(moveDirection.y + 1f) < tolerance && Mathf.Abs(moveDirection.x) < tolerance)
        {
            _animator.SetInteger("direction", (int)MovementDirection.DOWN);
        }
        else if (Mathf.Abs(moveDirection.y) < tolerance && Mathf.Abs(moveDirection.x - 1f) < tolerance)
        {
            _animator.SetInteger("direction", (int)MovementDirection.LEFT);
        }
        else if (Mathf.Abs(moveDirection.y) < tolerance && Mathf.Abs(moveDirection.x + 1f) < tolerance)
        {
            _animator.SetInteger("direction", (int)MovementDirection.RIGHT);
        }
        else if (Mathf.Abs(moveDirection.y) < tolerance && Mathf.Abs(moveDirection.x) < tolerance)
        {
            moving = false;
        }

        _animator.SetBool("moving", moving);
    }
}

public enum MovementDirection
{
    UP,
    UP_LEFT,
    LEFT,
    DOWN_LEFT,
    DOWN,
    DOWN_RIGHT,
    RIGHT,
    UP_RIGHT,
}