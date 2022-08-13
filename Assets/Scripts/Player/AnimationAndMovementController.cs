using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    public ParticleSystem runningParticleSystem;
    public GameObject particleEffectTemplate;
    public Transform impactPosition;

    PlayerActions playerInput;
    CharacterController characterController;
    Animator animator;
    ParticleSystem impact;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    float initialJumpVelocity;
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int isPushingHash;
    int isPullingHash;

    public float walkMultiplier = 4.0f;
    public float runMultiplier = 9.0f;
    private float dragBasePenalty = .75f;
    private float dragPenalty;
    float rotationFactorPerFrame = 15.0f;
    float gravity = -9.8f;
    float groundedGravity = -.05f;
    public float maxJumpHeight = 5.0f;
    public float maxJumpTime = 1f;

    bool isJumpPressed = false;
    bool isJumpAnimating = false;
    bool isJumping = false;

    bool isDragAnimating = false;
    public bool isDragging = false;
    public GameObject draggingGameObj;
    private Vector3 draggingGameObjOrigPos;
    public GameObject dragPrompt;
    private Vector3 dragOrigPos;
    Transform draggingGameObjOrigParent;
    public string draggingGameObjLoc;

    public bool canDrag = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("IsJumping");
        isPushingHash = Animator.StringToHash("IsPushing");
        isPullingHash = Animator.StringToHash("IsPulling");

        setupPlayerInput();
        setupJumpVariables();
        setupParticleEffects();
    }

    void setupParticleEffects()
    {
        GameObject particleEffectGO = Instantiate(particleEffectTemplate);
        impact = particleEffectGO.GetComponent<ParticleSystem>();
    }

    void setupPlayerInput()
    {
        playerInput = new PlayerActions();
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
        playerInput.CharacterControls.Drag.started += onDrag;

    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed && !isDragging)
        {
            runningParticleSystem.Stop();
            impact.transform.position = impactPosition.position;
            impact.Stop();
            impact.Play();
            animator.SetBool(isJumpingHash, true);
            isJumping = true;
            isJumpAnimating = true;
            currentMovement.y = initialJumpVelocity *.5f;
            currentRunMovement.y = initialJumpVelocity *.5f;
        } else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;

        if (characterController.isGrounded)
        {
            if(isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                impact.transform.position = impactPosition.position;
                impact.Stop();
                impact.Play();
            }
            if(runningParticleSystem.isStopped && isRunPressed)
            {
                runningParticleSystem.Play();
            }
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else if(isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -30.0f);
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }


        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }

    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void onDrag(InputAction.CallbackContext context)
    {
        if(isDragging)
        {
            isDragging = false;
        } else
        {
            isDragging = canDrag;
        }

        if(isDragging)
        {
            dragPrompt.SetActive(false);
            dragOrigPos = transform.position;
            draggingGameObjOrigPos = draggingGameObj.transform.position;
            draggingGameObjOrigParent = draggingGameObj.transform.parent;
            draggingGameObj.transform.parent = transform;
            draggingGameObj.transform.GetChild(0).GetComponent<Rigidbody>().freezeRotation = true;
            // draggingGameObj.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            // Destroy(draggingGameObj.transform.GetChild(0).GetComponent<Rigidbody>());
            // draggingGameObj.transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
            // draggingGameObj.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        } else
        {
            dragPrompt.SetActive(true);
            animator.SetBool(isPullingHash, false);
            animator.SetBool(isPushingHash, false);
            draggingGameObj.transform.parent = draggingGameObjOrigParent;
            draggingGameObj.transform.GetChild(0).GetComponent<Rigidbody>().freezeRotation = false;
            // draggingGameObj.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            // draggingGameObj.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
            // draggingGameObj.transform.GetChild(0).GetComponent<MeshCollider>().enabled = true;
            // draggingGameObj.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
    }

    void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkMultiplier;
        currentMovement.z = currentMovementInput.y * walkMultiplier;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if(isDragging)
        {
            runningParticleSystem.Stop();
            if ((draggingGameObjLoc == "left" && currentMovement.x < 0) || (draggingGameObjLoc == "right" && currentMovement.x > 0) || (draggingGameObjLoc == "top" && currentMovement.z > 0) || (draggingGameObjLoc == "bottom" && currentMovement.z < 0))
            {
                dragPenalty = dragBasePenalty;
                animator.SetBool(isPushingHash, true);
                animator.SetBool(isPullingHash, false);
            } else if(currentMovement.x != 0 || currentMovement.z != 0)
            {
                dragPenalty = dragBasePenalty - .2f;
                animator.SetBool(isPushingHash, false);
                animator.SetBool(isPullingHash, true);
            }
                
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, false);
        }
        else
        {
            if (isMovementPressed && !isWalking)
            {
                animator.SetBool(isWalkingHash, true);
            }
            if (!isMovementPressed && isWalking)
            {
                animator.SetBool(isWalkingHash, false);
            }

            if (isMovementPressed && isRunPressed && !isRunning)
            {
                if (characterController.isGrounded)
                    runningParticleSystem.Play();
                animator.SetBool(isRunningHash, true);
            }
            if ((!isMovementPressed || !isRunPressed) && isRunning)
            {
                runningParticleSystem.Stop();
                animator.SetBool(isRunningHash, false);
            }
        }
        
    }

    void handleRotation()
    {
        if(!isDragging)
        {
            Vector3 positionToLookAt;
            positionToLookAt.x = currentMovement.x;
            positionToLookAt.y = 0.0f;
            positionToLookAt.z = currentMovement.z;
            Quaternion currentRotation = transform.rotation;

            if (isMovementPressed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
            }
        }
        

    }

    private void handleDragObj()
    {
        if (isDragging && draggingGameObj != null)
        {
            // Vector3 diff = transform.position - dragOrigPos;
            // draggingGameObj.transform.position = draggingGameObjOrigPos + diff;
        }
    }
    private void Update()
    {
        handleAnimation();
        handleRotation();
        characterController.Move((isRunPressed ? isDragging ? currentRunMovement * dragPenalty : currentRunMovement : currentMovement) * Time.deltaTime);
        handleGravity();
        handleJump();
        handleDragObj();
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

}
