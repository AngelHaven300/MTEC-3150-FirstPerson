using UnityEngine;

public class FPController : MonoBehaviour
{
    private CharacterController characterController;
    public float walkSpeed = 5;
    public float mouseSensitivity = 2;
    float verticalRotation;
    public float upDownRange = 80;

    private Camera cam;
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
    }


    void Movement()
    {
        float vertInput = Input.GetAxis("Vertical");
        float horInput = Input.GetAxis("Horizontal");
        float verSpeed = vertInput * walkSpeed;
        float horSpeed = horInput * walkSpeed;

        Vector3 horizontalMovement = new Vector3(horSpeed, 0, verSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;
        characterController.Move(horizontalMovement * Time.deltaTime);
    }

    void MouseLook()
    {
        float mouseXRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

}
