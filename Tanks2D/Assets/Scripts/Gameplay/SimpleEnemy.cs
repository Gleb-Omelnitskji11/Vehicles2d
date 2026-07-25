using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    [RequireComponent(typeof(TankMovement))]
    public class SimpleEnemy : MonoBehaviour, IUpdatable
    {
        [Header("Direction")]
        [SerializeField] private float _minDirectionChangeTime = 2f;
        [SerializeField] private float _maxDirectionChangeTime = 5f;

        [Header("Obstacle")]
        [SerializeField] private float _forwardCheckDistance = 2f;
        [SerializeField] private float _sideCheckDistance = 1.2f;
        [SerializeField] private float _length = 0.37f;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private LayerMask _findMask;

        [Header("Turning")]
        [SerializeField] private float _turnDecisionAngle = 60f;
        [SerializeField] private float _reverseUntilAngle = 60f;

        [Header("Chasing")]
        [SerializeField] private float _viewDistance = 8f;

        private Transform _player;
        private TankMovement _movement;

        private Vector2 _targetDirection;

        private float _timer;
        private bool _avoiding;
        private bool _backward;
        private bool _chasingPlayer;

        private void Awake()
        {
            _obstacleMask = LayerMask.GetMask("Wall", "Enemy");
            _findMask = LayerMask.GetMask("Player");
            _movement = GetComponent<TankMovement>();
        }

        private void Start()
        {
            SetEvents();
            PickRandomDirection();
        }

        private void SetEvents()
        {
            Vehicle tank = GetComponent<Vehicle>();
            tank.OnCollision += OnCollision;
        }

        public void OnUpdate()
        {
            FindPlayer();

            if (_chasingPlayer)
            {
                _targetDirection = ((Vector2)transform.position - (Vector2)_player.position).normalized;
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
            bool front = Physics2D.Raycast(transform.position, transform.up, _forwardCheckDistance, _obstacleMask);

            if (!front)
            {
                _avoiding = false;
                return;
            }

            _avoiding = true;

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

            _targetDirection = -transform.up;
        }

        private void FindPlayer()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, _viewDistance, _findMask);

            if (hit.collider == null)
            {
                LoseTarget();
            }
            else
            {
                _player = hit.transform;
                _chasingPlayer = true;
                _avoiding = false;
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
                if (_backward || (absAngle > _reverseUntilAngle && hit.distance <= _length))
                {
                    _backward = true;
                    move = -0.4f;
                }
            }

            _movement.Move(move, turn);
        }

        private void OnCollision(Collision2D collision)
        {
            GameObject obj = collision.gameObject;
            if (obj.CompareTag("Player"))
            {
                if (collision.gameObject.TryGetComponent<Vehicle>(out var player))
                {
                    player.BasicUnitHealth.TakeLethalDamage();
                    LoseTarget();
                }
            }

            PickRandomDirection();
        }

        private void PickRandomDirection()
        {
            _targetDirection = Random.insideUnitCircle.normalized;

            _timer = Random.Range(_minDirectionChangeTime, _maxDirectionChangeTime);
            _backward = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.up * _forwardCheckDistance);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, 90) * transform.up * _sideCheckDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -90) * transform.up * _sideCheckDistance);
        }
#endif
    }
}