using UnityEngine;
using UnityEngine.Rendering;

public class FPController : MonoBehaviour
{
    private CharacterController characterController;
    public float walkSpeed;
    public float sprintSpeed;
    public float currentSpeed;
    public float jumpForce;
    public float mouseSensitivity = 2;
    float verticalRotation;
    public float upDownRange = 80;
    public float pickUpRange = 2f;
    public Transform holdPoint;
    private Item heldItem;
    public float throwForce = 5;
    //public ParticleSystem impactPS;
    public int particleCount = 20;
    public float gravity = 9.81f;



    private Camera cam;
    private Vector3 hitPoint;
    private Vector3 currentMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;  
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        MouseLook();
        Sprinting();
        Jumping();

        if (heldItem != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                heldItem.Throw(throwForce, cam.transform.forward);
                heldItem = null;
            }
        }
        if (ObjectInFocus() != null)
        {
            float distanceToObject = Vector3.Distance(cam.transform.position, ObjectInFocus().transform.position);

            if (Input.GetMouseButtonDown(0))
            {
                //impactPS.transform.position = hitPoint;
                //impactPS.Emit(particleCount);
            }
            if (distanceToObject <= pickUpRange && ObjectInFocus().GetComponent<Item>() != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    heldItem = ObjectInFocus().GetComponent<Item>();
                    heldItem.PickUp(cam.transform, holdPoint.position);
                }
            }
        }
        
    }

    void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;

        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }
    void Jumping()
    {

        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentMovement.y = jumpForce;

            }
        }
        else
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }

    }

    void Movement()
    {
        float vertInput = Input.GetAxis("Vertical");
        float horInput = Input.GetAxis("Horizontal");
        float verSpeed = vertInput * currentSpeed;
        float horSpeed = horInput * currentSpeed;
        
        

        Vector3 horizontalMovement = new Vector3(horSpeed, 0, verSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;
        currentMovement.x = horizontalMovement.x;
        currentMovement.z = horizontalMovement.z;
        characterController.Move(currentMovement * Time.deltaTime);
    }

    void MouseLook()
    {
        float mouseXRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
    
    public GameObject ObjectInFocus()
    {
        GameObject result = null;
        RaycastHit hit; 
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            result = hit.transform.gameObject;
            hitPoint = hit.point;
        }

        return result;
    }

}
