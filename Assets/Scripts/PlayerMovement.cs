using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private CharacterController characterController;
    private Animator animator;
    private float rotationSpeed = 4f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        //var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var mouseHorizontal = Input.GetAxis("Mouse X");

        animator.SetFloat("Speed", vertical);
        characterController.SimpleMove(transform.forward * vertical * moveSpeed);
        if (Input.GetMouseButton(1) == false)
        {
            transform.Rotate(Vector3.up * mouseHorizontal * rotationSpeed);
        }
    }
}