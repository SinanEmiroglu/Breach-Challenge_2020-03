using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const float GRAVITY = 9.81f;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float _rotationSpeed = 5f;

    private float _directionY;
    private float _verticalAxis;
    private float _horizontalAxis;
    private float _horizontalMouseAxis;
    private Transform _transform;
    private CharacterController _characterController;

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        _verticalAxis = Input.GetAxis("Vertical");
        _horizontalAxis = Input.GetAxis("Horizontal");
        _horizontalMouseAxis = Input.GetAxis("Mouse X");

        var direction = new Vector3(_horizontalAxis, 0, _verticalAxis);

        if (_characterController.isGrounded && Input.GetButtonDown("Jump"))
            _directionY = jumpForce;

        _directionY -= GRAVITY * Time.deltaTime;
        direction.y = _directionY;

        _transform.Rotate(_transform.up * _horizontalMouseAxis * _rotationSpeed);
        _characterController.Move(_transform.TransformDirection(direction) * Time.deltaTime * moveSpeed);
    }
}