using UnityEngine;

public class characterController : MonoBehaviour
{
    public CharacterController cc;
    public GameObject player;
    public Camera cam;
    public GameObject camobj;
    public Animator animator;

    [SerializeField] private float Sensitivity;
    [SerializeField] private float cameraSmoothness = 0.1f;
    [SerializeField] private float offsetY;
    [SerializeField] private float walkSpeed, runSpeed, crouchSpeed;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.4f;
    private float speed;
    public bool isMoving;
    public bool isCloseToGround;
    public float groundClose;


    private Vector3 crouchScale, normalScale;
    private float X, Y;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        speed = walkSpeed;
        crouchScale = new Vector3(1, .75f, 1);
        normalScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        UpdateCameraRotation();
        UpdateMovement();
        UpdateJump();
        UpdateGravity();
        UpdateAnimation();
    }

    private void UpdateCameraRotation()
    {
        const float MIN_Y = -60.0f;
        const float MAX_Y = 70.0f;

        X += Input.GetAxis("Mouse X") * (Sensitivity * Time.deltaTime);
        Y -= Input.GetAxis("Mouse Y") * (Sensitivity * Time.deltaTime);

        Y = Mathf.Clamp(Y, MIN_Y, MAX_Y);

        camobj.transform.localRotation = Quaternion.Euler(Y, X, 0.0f);
    }

    private void UpdateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        Vector3 moveDirection = (camobj.transform.forward * vertical + camobj.transform.right * horizontal).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(camobj.transform.forward, Vector3.up);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        }

        Vector3 targetCamPosition = transform.position + new Vector3(0, offsetY, 0);
        camobj.transform.position = Vector3.Lerp(camobj.transform.position, targetCamPosition, cameraSmoothness);

        cc.SimpleMove(moveDirection * speed);

        if (Input.GetKey(KeyCode.LeftShift) && (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f))
        {
            speed = runSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = crouchSpeed;
            player.transform.localScale = crouchScale;
        }
        else
        {
            speed = walkSpeed;
            player.transform.localScale = normalScale;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
    private void UpdateJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            
            
            
            

        }
        animator.SetBool("jumpUp", isGrounded);
    }

    private void UpdateGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        //isMoving = cc.velocity.sqrMagnitude > 0.0f;
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isCloseToGround", Physics.CheckSphere(groundCheck.position, groundClose, groundMask));
        
    }
}
