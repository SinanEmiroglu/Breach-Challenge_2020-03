using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 3f;

    private Transform _transform;
    //private Rigidbody _rigidbody;
    private CharacterController _characterController;
    private float _rotationSpeed = 4f;

    private void Awake()
    {
        _transform = transform;
        // _rigidbody = GetComponent<Rigidbody>();
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        //var mouseHorizontal = Input.GetAxis("Mouse X");

        _characterController.SimpleMove(_transform.forward * vertical * moveSpeed);

        //if (Input.GetButtonDown("Jump") && _characterController.isGrounded)
        //    Jump();
        //else
        //    _rigidbody.useGravity = false;

        //if (Input.GetMouseButton(1) == false)
        _transform.Rotate(Vector3.up * horizontal * _rotationSpeed);
    }

    private void Jump()
    {
        //_rigidbody.useGravity = true;
        //_rigidbody.velocity = new Vector3(0, jumpForce, 0);
        //_rigidbody.AddForce(Vector2.up * jumpForce);
    }
}