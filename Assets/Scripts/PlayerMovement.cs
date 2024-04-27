using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float lookSpeed;
    [SerializeField] private float lookXLimit;
    [SerializeField] private WFC wfc;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    public bool gameStarted = false;
    public bool gameEnded = false;

    [SerializeField] UIManager uiManager;
    [SerializeField] AudioSource floorStepsSound;
    [SerializeField] AudioSource stairsStepsSound;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void SetPlayerPosition()
    {
        transform.position = wfc.startingCell + new Vector3(-4, -0.25f, 0);
    }

    void Update()
    {
        // Finish the game if the player reaches the end
        if (gameEnded)
        {
            uiManager.SetEndGameText();
        }

        if (wfc.isCollapsing)
        {
            uiManager.UpdateLoadingProgress((int)wfc.progress);
        }

        if (gameStarted)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            if (!gameEnded)
            {
                uiManager.UpdateTimer();
            }

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            moveDirection.y = movementDirectionY;

            if (curSpeedX != 0 || curSpeedY != 0)
            {
                if (!floorStepsSound.isPlaying)
                {
                    floorStepsSound.Play();
                }
            }
            else
            {
                floorStepsSound.Stop();
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.R))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


}