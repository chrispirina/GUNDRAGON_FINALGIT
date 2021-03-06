﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator moveAnim;
    GameObject myCameraHolder;
    Transform cameraTransform;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    Vector3 forwardDir;
    Vector3 rightDir;

    public float moveSpeed = 6.0f;
    public float startSpeed = 0.0f;
    public bool isMoving = false;

    public float dodgeStrength = 5.0f;
    public float maxDodgeTime = 1.0f;
    public float dodgeTimer = 0.0f;
    public bool didDodge = false;



    public float gravityMotion = 0.0f; //Defines y axis movement of controller
    public float gravityMultiplier; // Defines modifieer on GravityMotion to increase/decrease fall speed.
    public float jumpStrength = 10.0f; // How high controller can jump.
    public float maxJumpTime = 1.0f; // The maximum limit of the jumpTimer.
    public float jumpTimer = 0.0f; // Counts how long until the controller reaches the apex of its jump. 
    public static bool didJump = false; // Is the controller jumping now.


    void Awake()
    {
        moveAnim = GetComponent<Animator>();
        myCameraHolder = GameObject.FindGameObjectWithTag("CameraHold");
        cameraTransform = myCameraHolder.transform;
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        forwardDir = cameraTransform.TransformDirection(Vector3.forward);
        forwardDir.y = 0.0f;
        forwardDir = forwardDir.normalized;
        rightDir = new Vector3(forwardDir.z, 0.0f, -forwardDir.x);
        moveDirection = (horizontal * rightDir + vertical * forwardDir);
        moveDirection = moveDirection * moveSpeed;

        WalkStart();

        DodgeCooldown();


        if (controller.isGrounded == true)
        {
            didJump = false;
            gravityMotion = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveAnim.SetTrigger(Anim.JUMP);
                gravityMotion = jumpStrength;
                jumpTimer = maxJumpTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (startSpeed > 0 && didDodge == false)
            {
                if (!moveAnim.GetCurrentAnimatorStateInfo(0).IsName("Evade"))
                {
                    moveAnim.SetTrigger(Anim.DASH);
                }
                dodgeTimer = maxDodgeTime;
            }
            else
            {
                Debug.Log("Could not dodge");
            }
        }

        isJumping();


        moveDirection.y = gravityMotion;

        moveDirection *= Time.deltaTime;

        if (startSpeed < 0.4f || didJump == true)
        {
            moveDirection.x = moveDirection.x / 2;
            moveDirection.z = moveDirection.z / 2;
        }

        if (didDodge == true)
        {
            moveDirection.x = moveDirection.x * dodgeStrength;
            moveDirection.z = moveDirection.z * dodgeStrength;
        }

        controller.Move(moveDirection); // Character moves following transform information.
    }

    public void WalkStart()
    {
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.15F);
            startSpeed += Time.deltaTime;
            isMoving = true;
        }

        else if (moveDirection.z == 0 && moveDirection.x == 0)
        {
            startSpeed = 0;
            isMoving = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forwardDir), 5 * Time.deltaTime);
        }

        moveAnim.SetBool(Anim.IS_MOVING, isMoving);

        if (isMoving)
            moveAnim.SetFloat(Anim.MOVING, moveAnim.GetFloat(Anim.MOVING) + Time.deltaTime);
        else
            moveAnim.SetFloat(Anim.MOVING, 0F);
    }

    public void DodgeCooldown()
    {
        if (dodgeTimer > 0)
        {
            //gameObject.GetComponent<CapsuleCollider>().enabled = false;
            dodgeTimer -= Time.deltaTime;
            didDodge = true;
        }
        else if (dodgeTimer <= 0)
        {
            didDodge = false;
        }
        if (dodgeTimer <= dodgeTimer / 2)
        {
            //gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }
    }

    public void isJumping()
    {
        if (jumpTimer <= 0)
        {

            gravityMotion -= 9.8f * gravityMultiplier * Time.deltaTime;

        }

        else if (jumpTimer > 0)
        {
            gravityMotion += 9.8f * jumpStrength * Time.deltaTime * 0.5f;
            jumpTimer -= Time.deltaTime * 2;
            didJump = true;
        }

    }

}
