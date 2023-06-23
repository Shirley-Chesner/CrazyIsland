using System.Collections;
using System.Collections.Generic;
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

    public bool shouldAllowMovment;
    public bool isFighting;
    public bool isSneaking;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        //audioSrc = GetComponent<AudioSource>();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        shouldAllowMovment = true;
    }
    void Update()
    { 
        if (shouldAllowMovment && !isFighting)
        {
            UpdateLook();
            UpdateMovement();
            UpdateGravity(); 
        }
    }

    public void changeAllowMovement(bool isMoveAllowed)
    {
        shouldAllowMovment = isMoveAllowed;
    }

    public void notAllowMovement()
    {
        isFighting = true;
    }

    public void allowMovement()
    {
        isFighting = false;
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
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetInteger("playerState", 3);
        }
        else
        {
           if (input != Vector3.zero)
                {
                    if (Input.GetKey(KeyCode.LeftControl)) {
                        animator.SetInteger("playerState", 2);
                         isSneaking = true;
                        playerSpeed = 2;
                    } else
                    {
                        animator.SetInteger("playerState", 1);
                        isSneaking = false;
                        playerSpeed = 5;
                    }
                }  else
                {
                    animator.SetInteger("playerState", 0);
                    isSneaking = false;
                    playerSpeed = 5;
                }
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
            animator.SetInteger("playerState", 0);
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

