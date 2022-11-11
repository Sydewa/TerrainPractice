using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCharacterController_Oogie : MonoBehaviour
{
#region Variables
    public Transform cam;
    private Animator anim;
    [SerializeField]private CharacterController controller;

    [Header("----Movimiento----")]
    public float speed = 5;
    public float jumpHeight = 1;
    public float gravity = -9.81f;
    
    //Rotacion del personaje
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    [Header("-----GroundSensor-----")]
    public bool isGrounded;
    public Transform groundSensor;
    public float sensorRadius = 0.1f;
    public LayerMask ground;
    private Vector3 playerVelocity;


#endregion
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    
    void Update()
    {
        MovementTPS();
        Jump();
    }

    void MovementTPS()
    {
        float z = Input.GetAxis("Vertical");
        anim.SetFloat("Dir_Z", z);
        float x = Input.GetAxis("Horizontal");
        anim.SetFloat("Dir_X", x);
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if(move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
    }

    void Jump()
    {
        if(Physics.Raycast(groundSensor.position, Vector3.down, sensorRadius, ground))
        {
            isGrounded = true;
            Debug.DrawRay(groundSensor.position, Vector3.down * sensorRadius, Color.green);
        }
        else
        {
            isGrounded = false;
            Debug.DrawRay(groundSensor.position, Vector3.down * sensorRadius, Color.red);
        }

        anim.SetBool("Jump", false);

        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }
        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity); 
            anim.SetBool("Jump", true);
        }
        
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 20f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundSensor.position, sensorRadius);
    }
}
