using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Will Auto Add Character Controller To Gameobject If It's Not Already Applied:
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Camera:
    public Camera playerCam;

    public Transform playerTransform;

    // Movement Settings:
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float jumpPower = 0f;
    public float gravity = 10f;

    // Camera Settings:
    public float lookSpeed = 2f;
    public float lookXLimit = 75f;
    public float cameraRotationSmooth = 5f;

    // Ground Sounds:
    public AudioClip[] FootstepSounds;
    public Transform footstepAudioPosition;
    public AudioSource audioSource;
    
    private float MouseLast;

    public bool isMoving = false;

    private bool isWalking = false;
    private bool isFootstepCoroutineRunning = false;
    private AudioClip[] currentFootstepSounds;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    public float curSpeedX, curSpeedY;

    public float Body_Rotation_Trigger_Angle = 35;
    public float Body_Rotation_Speed = 0.1f;

    private float Rotation_Memory;
    private float Body_Rotation_Progress = 0;

    // Can The Player Move?:
    private bool canMove = true;
    private bool Rotate_Body = false;

    CharacterController characterController;

    void Start()
    {
        
        // Ensure We Are Using The Character Controller Component:
        characterController = GetComponent<CharacterController>();

        // Lock And Hide Cursor:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize current footstep sounds to wood sounds by default
        currentFootstepSounds = FootstepSounds;

        
    }

    void Update()
    {
        // Walking/Running In Action:
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;


        

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Jumping In Action:
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // Camera Movement In Action:
        if (canMove)
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            rotationY += Input.GetAxis("Mouse X") * lookSpeed;
            
            if(!isMoving){
                playerCam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
                if(!((playerCam.transform.localRotation.eulerAngles.y <  Body_Rotation_Trigger_Angle && playerCam.transform.localRotation.eulerAngles.y >= 0) || (playerCam.transform.localRotation.eulerAngles.y >  360 - Body_Rotation_Trigger_Angle && playerCam.transform.localRotation.eulerAngles.y <= 360))){
                    Rotate_Body = true;
                }
            } else {
                playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation = Quaternion.Euler(0, rotationY, 0);
            }
        }

        if(Rotate_Body){
            if(Body_Rotation_Progress <= Body_Rotation_Speed){
                Rotation_Memory = playerCam.transform.localRotation.eulerAngles.y;
            }

            if(Rotation_Memory <  180){
                transform.Rotate(Vector3.up, Body_Rotation_Speed, Space.World);
            } else {
                transform.Rotate(Vector3.up, -Body_Rotation_Speed, Space.World);
            }

            Body_Rotation_Progress += Body_Rotation_Speed;

            if(playerCam.transform.localRotation.eulerAngles.y <= Body_Rotation_Speed || playerCam.transform.localRotation.eulerAngles.y >= 360 - Body_Rotation_Speed){
                Rotate_Body = false;
                Body_Rotation_Progress = 0;
            }
        }

        if(curSpeedX != 0f || curSpeedY != 0f){
            isMoving = true;
        } else {
            isMoving = false;
        }

        // Play footstep sounds when walking
        if ((curSpeedX != 0f || curSpeedY != 0f) && !isWalking && !isFootstepCoroutineRunning)
        {
            isWalking = true;
            StartCoroutine(PlayFootstepSounds(1.3f / (isRunning ? runSpeed : walkSpeed)));
        }
        else if (curSpeedX == 0f && curSpeedY == 0f)
        {
            isWalking = false;
        }
    }

    // Play footstep sounds with a delay based on movement speed
    IEnumerator PlayFootstepSounds(float footstepDelay)
    {
        isFootstepCoroutineRunning = true;
        while (isWalking)
        {
            if (currentFootstepSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, currentFootstepSounds.Length);
                audioSource.transform.position = footstepAudioPosition.position;
                audioSource.clip = currentFootstepSounds[randomIndex];
                audioSource.Play();
                yield return new WaitForSeconds(footstepDelay);
            }
            else
            {
                yield break;
            }
        }

        isFootstepCoroutineRunning = false;
    }
}
