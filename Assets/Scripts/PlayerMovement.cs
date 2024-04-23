using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 0.75f;
    public float runSpeed = 1.5f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    [SerializeField] private WFC wfc;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    public bool gameStarted = false;
    private bool gameEnded = false;
    private float time = 0f;


    // [SerializeField] private GameObject textObj;
    [SerializeField] private TextMeshProUGUI timeText;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        transform.position = wfc.startingCell + new Vector3(-4, 0, 0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // timeText = textObj.GetComponent<TextMeshPro>();
    }

    void Update()
    {
        // Finish the game if the player reaches the end
        if (transform.position.y >= wfc.dimY - 1.5)
        {
            gameEnded = true;
            Debug.Log("You Won in " + time + " seconds! Press R to restart");
            timeText.text = "You Won in " + time + " seconds! Press R to restart";
        }

        if (gameStarted)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            if (!gameEnded)
            {
                time += Time.deltaTime;
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                // int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);
                string formattedTime = $"{minutes:00}:{seconds:00}";
                timeText.text = formattedTime;

            }

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            moveDirection.y = movementDirectionY;

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.R))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }

            else
            {
                characterController.height = defaultHeight;
                walkSpeed = 6f;
                runSpeed = 12f;
            }

            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
    }
}