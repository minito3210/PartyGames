using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SelectPController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 10f;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // ääÇÁÇ©Ç…Ç∑ÇÈÇΩÇﬂÇÃê›íËÅiîCà”Åj
    }

    void Update()
    {
        HandleInput();
        HandleJump();
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        if (Input.GetKey(KeyCode.W)) moveVertical += 1f;
        if (Input.GetKey(KeyCode.S)) moveVertical -= 1f;
        if (Input.GetKey(KeyCode.A)) moveHorizontal -= 1f;
        if (Input.GetKey(KeyCode.D)) moveHorizontal += 1f;

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        movementInput = moveDirection * moveSpeed;
    }

    void Move()
    {
        Vector3 newPosition = rb.position + movementInput * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
