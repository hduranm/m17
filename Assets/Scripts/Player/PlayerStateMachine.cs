using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static UnityEditor.VersionControl.Asset;

[RequireComponent(typeof(Animator))]
public class PlayerStateMachine : MonoBehaviour
{
    private Animator _Animator;
    [SerializeField] private InputActionAsset _ActionAsset;
    private InputActionAsset _InputAction;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float movementSpeed = 5f;

    private InputAction _MovementAction;

    [SerializeField] private AnimationClip _AttackClip;
    [SerializeField] private AnimationClip _Attack2Clip;
    [SerializeField] private AnimationClip _Move;

    private void Awake()
    {
        _Animator = GetComponent<Animator>();
        Assert.IsNotNull(_ActionAsset, $"No has seleccionat input asset.");
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _InputAction = Instantiate(_ActionAsset);

        _InputAction.FindActionMap("Player").FindAction("Attack").performed += OnAttack;
        _InputAction.FindActionMap("Player").FindAction("Attack2").performed += OnAttackStrong;
        _MovementAction = _InputAction.FindActionMap("Player").FindAction("Move");


        _InputAction.FindActionMap("Player").Enable();

    }

    private enum PlayerStates { NULL, IDLE, KICK, PUNCH, MOVE }
    [SerializeField] private PlayerStates _CurrentState;
    [SerializeField] private float _StateTime;
    private bool _ComboAvailable;

    public void StartCombo()
    {
        _ComboAvailable = true;
    }

    public void EndCombo()
    {
        _ComboAvailable = false;
    }

    private void Start()
    {
        ChangeState(PlayerStates.IDLE);
    }

    private void ChangeState(PlayerStates newState)
    {
        if (newState == _CurrentState)
            return;

        ExitState(_CurrentState);
        InitState(newState);
    }

    private void InitState(PlayerStates initState)
    {
        _CurrentState = initState;
        _StateTime = 0f;

        switch (_CurrentState)
        {
            case PlayerStates.IDLE:
                _Animator.Play("Idle");
                break;
            case PlayerStates.KICK:
                _Animator.Play("Attack1");
                break;
            case PlayerStates.PUNCH:
                _Animator.Play("Attack2");
                break;
            case PlayerStates.MOVE:
                _Animator.Play("Move");
                break;
            default:
                break;
        }
    }


    private void UpdateState()
    {
        _StateTime += Time.deltaTime;

        switch (_CurrentState)
        {
            case PlayerStates.IDLE:
            case PlayerStates.MOVE:
                Vector2 movement = _MovementAction.ReadValue<Vector2>();
                _rigidbody2D.velocity = movement * movementSpeed;

                if (movement.x < 0)
                {
                    transform.eulerAngles = Vector3.up * 180;
                }
                else if (movement.x > 0)
                {
                    transform.eulerAngles = Vector3.zero;
                }

                if (movement != Vector2.zero && _CurrentState != PlayerStates.MOVE)
                {
                    ChangeState(PlayerStates.MOVE);
                }
                else if (movement == Vector2.zero && _CurrentState != PlayerStates.IDLE)
                {
                    ChangeState(PlayerStates.IDLE);
                }
                break;
            case PlayerStates.KICK:
                if (_StateTime >= _AttackClip.length)
                    ChangeState(PlayerStates.IDLE);
                break;

            case PlayerStates.PUNCH:
                if (_StateTime >= _Attack2Clip.length)
                    ChangeState(PlayerStates.IDLE);
                break;
            default:
                break;
        }
    }

    private void ExitState(PlayerStates exitState)
    {
        switch (exitState)
        {
            case PlayerStates.IDLE:
                break;
            case PlayerStates.KICK:
                _ComboAvailable = false;
                break;
            case PlayerStates.PUNCH:
                _ComboAvailable = false;
                break;
            default:
                break;
        }
    }
    private void OnAttack(InputAction.CallbackContext context)
    {
        switch (_CurrentState)
        {
            case PlayerStates.IDLE:
                ChangeState(PlayerStates.KICK);
                break;
            case PlayerStates.KICK:
                if (_ComboAvailable)
                    ChangeState(PlayerStates.PUNCH);
                break;
            case PlayerStates.PUNCH:
                if (_ComboAvailable)
                    ChangeState(PlayerStates.KICK);
                break;
            default:
                break;
        }
    }
    private void OnAttackStrong(InputAction.CallbackContext context)
    {
        switch (_CurrentState)
        {
            case PlayerStates.IDLE:
                ChangeState(PlayerStates.PUNCH);
                break;
            case PlayerStates.KICK:
                if (_ComboAvailable)
                    ChangeState(PlayerStates.PUNCH);
                break;
            case PlayerStates.PUNCH:
                if (_ComboAvailable)
                    ChangeState(PlayerStates.KICK);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        UpdateState();
    }

}
