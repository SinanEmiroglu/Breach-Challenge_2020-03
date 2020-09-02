using UnityEngine;

namespace Breach
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float jumpForce = 4f;
        [SerializeField] private float _rotationSpeed = 50f;

        [Header("Foot Sphere Cast Fields")]
        [SerializeField] Transform foot;
        [SerializeField] private float radius;
        [SerializeField] private LayerMask layerMask;

        private float _verticalAxis;
        private float _horizontalAxis;
        private float _horizontalMouseAxis;
        private Transform _transform;
        private Rigidbody _rigidbody;
        private Vector3 _directionBeforeJump;
        private RaycastHit[] _raycastHits;

        /// <summary>
        /// Bounce player after they jump on dudes
        /// </summary>
        public void Bounce()
        {
            _rigidbody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        }

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _raycastHits = new RaycastHit[3];
        }

        private void FixedUpdate()
        {
            _verticalAxis = Input.GetAxis("Vertical");
            _horizontalAxis = Input.GetAxis("Horizontal");
            _horizontalMouseAxis = Input.GetAxis("Mouse X");

            Vector3 direction = new Vector3(_horizontalAxis, 0, _verticalAxis);

            if (IsGrounded())
            {
                MoveOnGround(direction);
            }

            if (IsGrounded() && Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
            else
            {
                Fall();
            }

            if (Input.GetMouseButton(1))
            {
                RotatePlayer();
            }
            else
            {
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        private void Fall()
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * 5f * Time.deltaTime;
        }

        private void Jump()
        {
            _rigidbody.AddForce(0.5f * jumpForce * (2 * Vector3.up + _directionBeforeJump), ForceMode.Impulse);
        }

        private void MoveOnGround(Vector3 direction)
        {
            _directionBeforeJump = _transform.TransformDirection(direction);
            Vector3 motion = _rigidbody.position + _transform.TransformDirection(moveSpeed * Time.deltaTime * direction);
            _rigidbody.MovePosition(motion);
        }

        private void RotatePlayer()
        {
            Quaternion mouseRotation = _rigidbody.rotation * Quaternion.Euler(Vector3.up * _horizontalMouseAxis * _rotationSpeed);
            _rigidbody.MoveRotation(mouseRotation);
        }

        /// <summary>
        /// NonAlloc casts are more cheaper than dynamic casts.
        /// </summary>
        private bool IsGrounded() => Physics.SphereCastNonAlloc(foot.position, radius, Vector2.down, _raycastHits, 0, layerMask) > 0;

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(foot.position, radius);
        }

#endif
    }
}