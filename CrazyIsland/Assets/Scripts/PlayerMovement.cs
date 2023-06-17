using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private float jumpHorizontalSpeed;

    CharacterController controller;
    //AudioSource audioSrc;
    public float playerSpeed = 5;

    public float mouseSensivity = 3; 
    Vector2 look;
    Vector3 velocity;
    float mass = 1f;
    public float jumpSpeed = 5f;

    private Animator animator;
    private bool isJumping;
    private bool isGrounded;

    private float? jumpBtnPressedTime;
    private float? lastGroundedTime;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        //audioSrc = GetComponent<AudioSource>();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        UpdateLook(); 
        UpdateMovement();
        UpdateGravity(); 
    }

    void UpdateLook()
    {  
        look.x += Input.GetAxis("Mouse X") * mouseSensivity; 
        look.y += Input.GetAxis("Mouse Y") * mouseSensivity; 
        look.y = Mathf.Clamp(look.y, -90, 90);
        transform.localRotation = Quaternion.Euler(0, look.x, 0); 
    }
    void UpdateMovement()
    {   
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var input = new Vector3();
        input += transform.forward * z;
        input += transform.right * x;
        input = Vector3.ClampMagnitude(input, 1f);
            // the character is moving so change the state of the animation to running, sneaking or idle
            if (input != Vector3.zero)
        {
            if (Input.GetButton("Sneaking")) {
                animator.SetInteger("playerState", 2);
            } else
            {
                animator.SetInteger("playerState", 1);
            }
          
        }  else
        {
            animator.SetInteger("playerState", 0);
        }

            //// Play step sounds
            //if (input != Vector3.zero)
            //{
            //    if (!audioSrc.isPlaying)
            //    {
            //        audioSrc.Play();
            //    }  
            //} else
            //{
            //    audioSrc.Stop();
            //}

        if (controller.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBtnPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            // the character is jumping so set the animation and velocity
            velocity.y = -0.5f;
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);
            if (Time.time - jumpBtnPressedTime <= jumpButtonGracePeriod)
            {
            velocity.y += jumpSpeed;
            animator.SetBool("isJumping", true);
            isJumping = true;
            jumpBtnPressedTime = null;
            lastGroundedTime = null;
            }
        } else
        {
            // the character is not jumping 
            animator.SetBool("isGrounded", false);
            isGrounded = false;

            if ((isJumping && velocity.y < 0) || velocity.y  < -2)
            {
                animator.SetBool("isFalling", true);
            }
        }

        // move the charactor - if jumping by the speed of jumping
        if (isGrounded)
        {
            controller.Move((input * playerSpeed + velocity) * Time.deltaTime);
        } else
        {
            controller.Move((input * jumpHorizontalSpeed + velocity) * Time.deltaTime);
        }
       
    }
    private void UpdateGravity()
    {      
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1 : velocity.y + gravity.y;
    }
}

