using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _moveStartForce;
    private float _startTorque;
    private float _startBoostTimer;
    private float _startRevengeTimer;
    private int _startHealth;
    private Sprite _startSprite;

    [SerializeField] private Transform _directionObject;

    [SerializeField]
    private GameObject _boostObject, _revengeObject, _shieldObject,
        _magnetObject, _explosionPrefabs;

    [SerializeField] private TrailRenderer[] _trails;
    [SerializeField] private AudioClip _bombClip;

    private float _input;
    private Rigidbody2D _rb;
    private PlayerMovement _movement;
    private SpriteRenderer _spriteRenderer;
    private bool _canMove;
    private bool _isRevengeMode;

    private int _missCount;
    private int _currentHealth;
    private float _boostTimer;
    private float _revengeTimer;
    private float _powerUpTimer;
    private float _boostForce;
    private float _appliedForce;
    private float _appliedTorque;

    public bool _isMagnetOn;
    public bool _isShieldOn;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponent<PlayerMovement>();

        _isRevengeMode = false;
        _isMagnetOn = false;
        _isShieldOn = false;
        _canMove = false;

        _boostObject.SetActive(false);
        _revengeObject.SetActive(false);
        _shieldObject.SetActive(false);
        _magnetObject.SetActive(false);
    }

    private void Start()
    {
        _moveStartForce = 24f;
        _startTorque = _moveStartForce * 1200 / 24f;
        _appliedForce = _moveStartForce;
        _appliedTorque = _startTorque;
        _boostForce = 1.5f * _moveStartForce;

        _missCount = 0;
        _startBoostTimer = 2f;
        _startRevengeTimer = 4f;
        _boostTimer = _startBoostTimer;
        _revengeTimer = _startRevengeTimer;
        _powerUpTimer = 4f;
        _startHealth = 1;
        _currentHealth = _startHealth;

        _startSprite = _spriteRenderer.sprite;
        _spriteRenderer.sprite = _startSprite;
    }

    private void OnEnable()
    {
        EventManager.StartListening(Constants.EventNames.GAME_START, StartGame);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Constants.EventNames.GAME_START, StartGame);
    }

    private void Update()
    {
        _input = _movement._horizontal;
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;
        _rb.velocity = _rb.velocity * 0.85f;
        Vector3 moveDirection = _directionObject.position - transform.position;
        _rb.AddForce(moveDirection * _appliedForce);
        _rb.angularVelocity = _rb.angularVelocity * 0.85f;

        if (Mathf.Abs(_input) > 0.05f)
        {
            _rb.AddTorque(-_appliedTorque * Time.deltaTime * _input);

            foreach (var item in _trails)
            {
                item.emitting = true;
            }
        }
        else
        {
            foreach (var item in _trails)
            {
                item.emitting = false;
            }
        }
    }

    private void StartGame(Dictionary<string, object> message)
    {
        _canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Constants.Tags.ENEMY))
        {
            if (_isRevengeMode) return;

            if (_isShieldOn) return;

            _currentHealth--;

            if (_currentHealth == 0)
            {

            }
            else
            {
                _canMove = false;
                GameManager.Instance.GameOver();
                Destroy(gameObject, 2f);
            }

            EventManager.TriggerEvent(Constants.EventNames.UPDATE_HEALTH, new Dictionary<string, object>()
            {
                {Constants.ScoreMessage.AMOUNT, _currentHealth }
            });
        }
    }
}
