using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pokeball : MonoBehaviour
{
    public AudioClip capturingClip;
    public AudioClip escapeClip;
    public AudioClip attack1Clip;
    public AudioClip attack2Clip;

    private const float TIME_TO_ESCAPE = 8000; // 8s based on audio
    private const int ESCAPE_COUNT_NEEDED = 2; // TODO: cahnge this based on how much life is left
    private float timeSinceCapture = 0f;
    private int escapeCount = 0;
    private bool isCapturing = false;
    private bool _isGameOver = false;
    private Coroutine capturingCoroutine;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private PlayerControls _playerControls;
    private GameManager _gameManager;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        _spriteRenderer.enabled = false;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        if (isCapturing)
        {
            this.CheckCapturing();
        }
    }

    private void CheckCapturing()
    {
        bool inputEscape = _playerControls.Overworld.EscapePokeball.WasPerformedThisFrame();
        timeSinceCapture += Time.deltaTime;

        if (inputEscape && timeSinceCapture < TIME_TO_ESCAPE && !_isGameOver)
        {
            escapeCount++;
        }

        if (escapeCount >= ESCAPE_COUNT_NEEDED)
        {
            StartCoroutine(StopCapture());
        }
    }

    public void StartCapture()
    {
        _gameManager.IsCapturing = true;
        capturingCoroutine = StartCoroutine(Tilt());
    }

    private IEnumerator StopCapture()
    {
        StopCoroutine(capturingCoroutine);
        isCapturing = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
        _audioSource.Stop();
        _audioSource.PlayOneShot(escapeClip);

        yield return new WaitForSeconds(1.8f);

        _spriteRenderer.enabled = false;
        _gameManager.SetPlayerActive(true);
        _gameManager.IsCapturing = false;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.3f);
        _audioSource.PlayOneShot(attack1Clip);
        yield return new WaitForSeconds(1.5f);
        _audioSource.PlayOneShot(attack2Clip);
    }

    private IEnumerator Tilt()
    {
        StartCoroutine(Attack());
        yield return new WaitForSeconds(4.6f);

        _spriteRenderer.enabled = true;
        _gameManager.SetPlayerActive(false);
        escapeCount = 0;
        isCapturing = true;

        // Start capturing
        _audioSource.PlayOneShot(capturingClip);

        // First
        transform.eulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.3f);
        transform.eulerAngles = new Vector3(0, 0, 30);
        yield return new WaitForSeconds(1.5f);

        // Second
        transform.eulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.3f);
        transform.eulerAngles = new Vector3(0, 0, -30);
        yield return new WaitForSeconds(1.5f);

        // Third
        transform.eulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.3f);
        transform.eulerAngles = new Vector3(0, 0, 30);
        yield return new WaitForSeconds(1.5f);

        // Fourth
        transform.eulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.3f);
        transform.eulerAngles = new Vector3(0, 0, -30);
        yield return new WaitForSeconds(1.8f);

        // Captured
        _isGameOver = true;
        FindObjectOfType<ScoreManager>().EndGame();
        transform.eulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("GameOver");
    }
}
