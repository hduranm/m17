using System.Collections;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static UnityEditor.VersionControl.Asset;

[RequireComponent(typeof(Animator))]
public class EnemyStateMachine : MonoBehaviour
{
    private Animator _Animator;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Transform target;
    [SerializeField] private DetectionArea detection;
    [SerializeField] private DetectionArea attackRange;
    [SerializeField] private float movementSpeed = 2f;


    [SerializeField] private AnimationClip _AttackClip;
    [SerializeField] private AnimationClip _Move;

    private enum EnemyStates { NULL, IDLE, ATTACK, MOVE }
    [SerializeField] private EnemyStates _CurrentState;
    [SerializeField] private float _StateTime;
    [SerializeField] private GameObject prefabProyectil;  // Asigna aquí el prefab de proyectil desde el Inspector
    [SerializeField] private Transform puntoDisparo;      // Punto de origen del proyectil
    [SerializeField] private float velocidadProyectil = 5f; // Velocidad del proyectil


    private bool _PlayerAtAttackRange = false;

    private void Awake()
    {
        _Animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        detection.OnEnter += OnPlayerDetectedEnter;
        detection.OnExit += OnPlayerDetectedLeave;
        attackRange.OnEnter += OnPlayerAttackEnter;
        attackRange.OnExit += OnPlayerAttackLeave;
    }

    private void Start()
    {
        ChangeState(EnemyStates.IDLE);
    }

    private void OnPlayerDetectedEnter(GameObject player)
    {
        target = player.transform;
        ChangeState(EnemyStates.MOVE);
    }

    private void OnPlayerDetectedLeave(GameObject player)
    {
        target = null;
    }

    private void OnPlayerAttackEnter(GameObject player)
    {
        _PlayerAtAttackRange = true;
        ChangeState(EnemyStates.ATTACK);
    }

    private void OnPlayerAttackLeave(GameObject player)
    {
        _PlayerAtAttackRange = false;
    }

    private IEnumerator Atacar()
    {
        while (_PlayerAtAttackRange)
        {
            _Animator.Play("Idle");
            yield return new WaitForSeconds(0.4f);
            _Animator.Play("Attack", 0, 0f);
            yield return new WaitForSeconds(0.4f);
            DispararProyectil();
            yield return new WaitForSeconds(_AttackClip.length);
            _Animator.Play("Idle");
            yield return new WaitForSeconds(0.6f);
        }
        ChangeState(EnemyStates.MOVE);
    }

    private void ChangeState(EnemyStates newState)
    {
        ExitState(_CurrentState);
        InitState(newState);
    }

    private void InitState(EnemyStates initState)
    {
        _CurrentState = initState;
        _StateTime = 0f;

        switch (_CurrentState)
        {
            case EnemyStates.IDLE:
                _rigidbody2D.velocity = Vector2.zero;
                _Animator.Play("Idle", 0, 0f);
                break;
            case EnemyStates.ATTACK:
                StartCoroutine(Atacar());
                break;
            case EnemyStates.MOVE:
                _Animator.Play("Move", 0, 0f);
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
            case EnemyStates.IDLE:
                break;

            case EnemyStates.MOVE:
                if (target != null)
                {
                    Vector2 direction = (target.position - transform.position).normalized;
                    _rigidbody2D.velocity = direction * movementSpeed;

                    if (direction.x < 0)
                    {
                        transform.eulerAngles = Vector3.up * 180;
                    }
                    else if (direction.x > 0)
                    {
                        transform.eulerAngles = Vector3.zero;
                    }
                }
                else
                {
                    ChangeState(EnemyStates.IDLE);
                }
                break;
            default:
                break;
        }
    }

    private void ExitState(EnemyStates exitState)
    {
        switch (exitState)
        {
            case EnemyStates.IDLE:
                break;
            case EnemyStates.ATTACK:
                StopAllCoroutines();
                break;
            case EnemyStates.MOVE:
                _rigidbody2D.velocity = Vector2.zero;
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        UpdateState();
    }

    private void DispararProyectil()
    {
        if (target == null || prefabProyectil == null) return;

        GameObject proyectil = Instantiate(prefabProyectil, puntoDisparo.position, Quaternion.identity);

        Vector2 direccion = (target.position - puntoDisparo.position).normalized;

        Rigidbody2D rbProyectil = proyectil.GetComponent<Rigidbody2D>();
        rbProyectil.velocity = direccion * velocidadProyectil;
    }


}
