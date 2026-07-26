using Core;
using Gameplay.UpdatingInstance;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Enemy
{
    [RequireComponent(typeof(VehicleMovement))]
    public class SimpleEnemy : MonoBehaviour, IUpdatable
    {
        [Header("Direction")]
        [SerializeField] private float _minDirectionChangeTime = 2f;
        [SerializeField] private float _maxDirectionChangeTime = 5f;

        [Header("Obstacle")]
        [SerializeField] private float _forwardCheckDistance = 2f;
        [SerializeField] private float _sideCheckDistance = 1.2f;
        [SerializeField] private float _backwardDistance = 0.37f;
        [SerializeField] private int MaxFindWayAttempts = 10;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private LayerMask _findMask;

        [Header("Turning")]
        [SerializeField] private float _turnDecisionAngle = 60f;
        [SerializeField] private float _reverseUntilAngle = 60f;

        [Header("Chasing")]
        [SerializeField] private float _playerViewDistance = 16f;

        private Transform _player;
        private VehicleMovement _movement;

        private Vector2 _targetDirection;

        private float _timer;
        private bool _avoiding;
        private bool _backward;
        private bool _chasingPlayer;
        private PlayerFinder _playerFinder;

        private void Awake()
        {
            _obstacleMask = LayerMask.GetMask(Constants.Wall, Constants.Player);
            _findMask = LayerMask.GetMask(Constants.Player);
            _movement = GetComponent<VehicleMovement>();
            _playerFinder = new PlayerFinder(transform, _obstacleMask);
        }

        private void Start()
        {
            SetEvents();
            PickRandomDirection();
        }

        private void OnDisable()
        {
            _targetDirection = default;
            _avoiding = false;
            _backward = false;
            _chasingPlayer = false;
            _timer = 0f;
        }

        private void SetEvents()
        {
            Vehicle tank = GetComponent<Vehicle>();
            tank.OnCollision += OnCollision;
        }

        public void OnUpdate()
        {
            _chasingPlayer = _playerFinder.TryFind(ref _player);

            if (_chasingPlayer)
            {
                _targetDirection = ((Vector2)_player.position - (Vector2)transform.position).normalized;
            }
            else
            {
                _timer -= Time.deltaTime;

                if (!_avoiding && _timer <= 0f)
                    PickRandomDirection();
                HandleObstacle();
            }

            Move();
        }

        private void HandleObstacle()
        {
            if (_chasingPlayer) return; // delete this if there are obstacles in field
            bool front = Physics2D.Raycast(transform.position, transform.up, _forwardCheckDistance, _obstacleMask);

            if (!front)
            {
                _avoiding = false;
                if(_backward)
                {
                    _backward = false;
                    PickRandomDirection();
                }
                return;
            }

            //if bot has an obstacle in front, he try to avoid
            _avoiding = true;

            if (_backward)
                return;
            Vector2 left = Quaternion.Euler(0, 0, 90) * transform.up;
            Vector2 right = Quaternion.Euler(0, 0, -90) * transform.up;
            bool leftBlocked = Physics2D.Raycast(transform.position, left, _sideCheckDistance, _obstacleMask);
            bool rightBlocked = Physics2D.Raycast(transform.position, right, _sideCheckDistance, _obstacleMask);

            if (!leftBlocked)
            {
                _targetDirection = left;
                return;
            }

            if (!rightBlocked)
            {
                _targetDirection = right;
                return;
            }
        }

        private void LoseTarget()
        {
            _chasingPlayer = false;
            _player = null;
        }

        private void Move()
        {
            float angle = Vector2.SignedAngle(transform.up, _targetDirection);
            float turn = Mathf.Clamp(angle / _turnDecisionAngle, -1f, 1f);
            float move = 1f;

            if (_avoiding)
            {
                float absAngle = Mathf.Abs(angle);
                var hit = Physics2D.Raycast(transform.position, transform.up, _forwardCheckDistance, _obstacleMask);
                bool isBackwardNeed = absAngle > _reverseUntilAngle && hit.distance <= _backwardDistance;
                if (!_backward && isBackwardNeed)
                {
                    _backward = true;
                }
            }

            if (_backward)
                move = -0.4f;
            
            _movement.Move(move, -turn);
        }

        private void OnCollision(Collision2D collision)
        {
            GameObject obj = collision.gameObject;
            if (obj.CompareTag(Constants.Player))
            {
                if (collision.gameObject.TryGetComponent<Vehicle>(out var player))
                {
                    player.BasicUnitHealth.TakeLethalDamage();
                    LoseTarget();
                }
            }

            if(!_chasingPlayer)
            {
                PickRandomDirection();
            }
        }

        private void PickRandomDirection()
        {
            if (_chasingPlayer) return;
            bool block;
            int attempt = 0;
            do
            {
                float angle = Random.Range(-90f, 90f);
                _targetDirection = (Quaternion.Euler(0, 0, angle) * transform.up).normalized;
                block = Physics2D.Raycast(transform.position, _targetDirection, 2 * _forwardCheckDistance, _obstacleMask);
                attempt++;
            } while (block && attempt < MaxFindWayAttempts);
            
            if(block && attempt == MaxFindWayAttempts)
                _backward = true;

            _timer = Random.Range(_minDirectionChangeTime, _maxDirectionChangeTime);
            _backward = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.up * _forwardCheckDistance);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, 90) * transform.up * _sideCheckDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -90) * transform.up * _sideCheckDistance);

        }
#endif
    }
}