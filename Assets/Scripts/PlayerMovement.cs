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
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
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
        // transform.position = wfc.startingCell + new Vector3(-4, 0, 0);
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