using UnityEngine;
using UnityEngine.InputSystem;

public class FurretController : MonoBehaviour
{
    // Constants that can be edited from UI
    public float NormalSpeed = 1f;
    private float _currentSpeed;
    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
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
    }
}