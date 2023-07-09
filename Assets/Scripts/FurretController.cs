using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FurretController : MonoBehaviour
{
    // Constants that can be edited from UI
    public float NormalSpeed = 0.002f;
    public float FastSpeed = 0.03f;
    public float ChangeDirectionTolerance = 0.1f;
    public int InvulnerabilityFrames = 300;
    public Color level1Color = Color.yellow;
    public Color level2Color = Color.magenta;
    public Color level3Color = Color.red;
    private Color _currentColor = Color.white;
    private float _currentSpeed;
    private PlayerControls _playerControls;
    private Animator _animator;
    private GameManager _gameManager;
    private SpriteRenderer _spriteRenderer;

    private MovementDirection _movementDirection = MovementDirection.RIGHT;
    private Queue<Vector2> _movementQueue;
    private int _remainingInvulnerability = 0;
    private Coroutine _blinkCoroutine;
    private int _accumulatedDamage = 0;

    private void Awake()
    {
        _movementQueue = new Queue<Vector2>();
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = FindObjectOfType<GameManager>();
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
        if (_gameManager.IsCapturing)
        {
            _animator.SetBool("moving", false);
            return;
        }

        Vector2 inputMove = _playerControls.Overworld.Move.ReadValue<Vector2>();
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

        if (_remainingInvulnerability >= 0)
        {
            if (_remainingInvulnerability == InvulnerabilityFrames)
            {
                _blinkCoroutine = StartCoroutine(Blink());
            }

            _remainingInvulnerability--;
        }

        if (_remainingInvulnerability <= 0)
        {
            _currentSpeed = NormalSpeed;
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
                _spriteRenderer.enabled = true;
            }
        }

        Color lerpedColor = Color.Lerp(Color.white, _currentColor, Mathf.PingPong(Time.time, 1));
        _spriteRenderer.color = lerpedColor;
    }

    void FixedUpdate()
    {
        if (_gameManager.IsCapturing)
        {
            return;
        }

        while (_movementQueue.Count > 0)
        {
            Vector2 qMove = _movementQueue.Dequeue();
            transform.position += new Vector3(qMove.x, qMove.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_remainingInvulnerability <= 0 && other.gameObject.GetComponent<Trainer>() != null)
        {
            _gameManager.StartCapture();
            _remainingInvulnerability = InvulnerabilityFrames;
            _currentSpeed = FastSpeed;
            _accumulatedDamage++;
            if (_accumulatedDamage > 3)
            {
                _currentColor = level3Color;
            }
            else if (_accumulatedDamage > 2)
            {
                _currentColor = level2Color;
            }
            else if (_accumulatedDamage > 1)
            {
                _currentColor = level1Color;
            }
        }
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.3f);
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