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

    // IK Setup:
    public Transform leftFootTarget, rightFootTarget;
    public Transform leftHandTarget, rightHandTarget;
    public float footOffset = 0.1f; // Offset to avoid foot clipping into ground
    public LayerMask groundMask;
    public float stepHeight = 0.2f; // Height of the step arc
    public float stepSpeed = 6f; // Speed of the stepping motion

    private bool isWalking = false;
    private bool isFootstepCoroutineRunning = false;
    private AudioClip[] currentFootstepSounds;

    private bool leftLegStepping = false;
    private bool rightLegStepping = false;

    private Vector3 leftFootStartPos, leftFootEndPos;
    private Vector3 rightFootStartPos, rightFootEndPos;
    private float leftfootStepProgress = 0f;
    private float rightfootStepProgress = 0f;
    private Vector3 leftlastPosition, rightlastPosition;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    public float curSpeedX, curSpeedY;

    // Can The Player Move?:
    private bool canMove = true;

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

        // Initial foot positions
        leftFootStartPos = leftFootTarget.position;
        rightFootStartPos = rightFootTarget.position;

        
    }

    void Update()
    {
        // Walking/Running In Action:
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        RaycastHit hitl;
        RaycastHit hitr;


        

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

            playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            if(MouseLast != rotationY){
                transform.rotation = Quaternion.Euler(0, rotationY, 0);
                MouseLast = rotationY;
            }

        }

        // Update foot target positions to follow the character's movement
        UpdateFootTargetPosition(leftFootTarget);
        UpdateFootTargetPosition(rightFootTarget);

        // IK Foot Placement:
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

    // Ensure that foot targets move with the character
    void UpdateFootTargetPosition(Transform footTarget)
    {
        footTarget.position = transform.TransformPoint(footTarget.localPosition);
    }

    // Handle foot placement and procedural stepping
    void HandleFootPlacement(Vector3 direction)
    {   

        if(leftfootStepProgress == 0) {
            MoveFoot(ref rightfootStepProgress, rightFootTarget, rightlastPosition, playerTransform.position + direction + playerTransform.right*0.15f, false);
            leftFootTarget.position = leftlastPosition;
        }
        if(rightfootStepProgress == 0) {
            MoveFoot(ref leftfootStepProgress, leftFootTarget, leftlastPosition, playerTransform.position + direction - playerTransform.right*0.15f, true);
            rightFootTarget.position = rightlastPosition;
        }
    }

    void MoveFoot(ref float footStepProgress, Transform footTarget, Vector3 footStartPos, Vector3 footEndPos, bool left)
    {
        
            footStepProgress += Time.deltaTime * stepSpeed;

            // Arc movement for the foot to simulate lifting
            footTarget.position = Vector3.Lerp(footStartPos, footEndPos, footStepProgress);

            

            if (footStepProgress >= 0.2f) {
                footTarget.position = footTarget.position + Vector3.up * stepHeight*(2f - 5f*footStepProgress);
            } else {
                footTarget.position = footTarget.position + Vector3.up * stepHeight*(5f*footStepProgress);
            }

            // Check if the foot has completed the step
            if (footStepProgress >= 0.4f)
            {   
                if(left){
                    leftlastPosition = footTarget.position;
                } else {
                    rightlastPosition = footTarget.position;
                }
                
                footStepProgress = 0f;
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
