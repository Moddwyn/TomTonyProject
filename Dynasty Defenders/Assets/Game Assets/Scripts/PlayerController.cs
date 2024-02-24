using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public KeyCode runKey = KeyCode.LeftShift;

    [Space(20)]
    public float jumpStrength = 5;
    public float gravity = -20;
    public KeyCode jumpKey = KeyCode.Space;

    [Space(20)]
    public Transform cameraTransform;
    public float camSensitivity = 1;
    public float camSmoothing = 2;

    [Space(20)]
    public bool allowMove = true;
    public bool allowJump = true;
    public bool allowLook = true;

    CharacterController cc;
    bool isGrounded = true;
    Vector2 currentMouseLook;
    Vector2 appliedMouseDelta;
    Vector3 moveDirection = Vector3.zero;
    float currSpeed;
    float yVel;

    public static PlayerController Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();

        AllowControl(true);
    }

    void Update()
    {
        if (allowJump)
            Jump();
        if (allowLook)
            Look();
        if (allowMove)
            Move();
        
    }

    void Move()
    {
        currSpeed = Input.GetKey(runKey) ? runSpeed : walkSpeed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = currSpeed * Input.GetAxis("Vertical");
        float curSpeedY = currSpeed * Input.GetAxis("Horizontal");
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.y = yVel;

        // Move the controller
        cc.Move(moveDirection * Time.deltaTime);
    }

    void Jump()
    {
        Gravity();


        isGrounded = cc.isGrounded;
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            yVel = Mathf.Sqrt(jumpStrength * -2f * (gravity));
        }
    }

    void Gravity()
    {
        if (yVel > gravity + 1)
            yVel += gravity * Time.deltaTime;
        else
            yVel = gravity;
    }

    void Look()
    {
        Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * camSensitivity * camSmoothing);
        appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / camSmoothing);
        currentMouseLook += appliedMouseDelta;
        currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90, 90);

        cameraTransform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);
    }

    public void AllowLook(bool allow)
    {
        Cursor.lockState = allow ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !allow;

        allowLook = allow;
    }

    public void AllowControl(bool allow)
    {
        allowJump = allow;
        allowMove = allow;

        AllowLook(allow);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}