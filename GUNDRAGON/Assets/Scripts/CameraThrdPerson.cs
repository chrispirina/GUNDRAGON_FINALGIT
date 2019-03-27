using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThrdPerson : MonoBehaviour
{
    public float rotateSensitivity;
    public Transform target;
    private Transform targetPlayer;
    private Transform targetEnemy;
    public List<Transform> targetObjects;

    public static bool amLocked = false;

    public float dstFromTarget = 4;
    public Vector2 verticalMinMax = new Vector2(-30, 60);

    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float verticalRotate;
    float horizontalRotate;

    void Start()
    {
        SetInitialPrefs();
    }
	
    void Update()
    {
     
        if (targetObjects[0] == null)
        {
            amLocked = false;
            targetObjects.Remove(targetObjects[0]);            
        }  

        if (Input.GetMouseButtonDown(2))
        {
            if (amLocked == true)
            {
                targetEnemy = targetObjects[0];
                amLocked = false;
            }
            else if (amLocked == false)
            {
                amLocked = true;
            }
        }

        if (amLocked == false)
        {
            target = targetPlayer;
        }

        else if (amLocked == true)
        {
            target = targetEnemy;
            if (Input.GetKeyDown(KeyCode.Tab))
            {

            }
        }
    }

	void LateUpdate ()
    {
        if (Player.didPause == false)
        {
            if (amLocked == false)
            {
                transform.LookAt(target);

                verticalRotate -= Input.GetAxis("Mouse Y") * rotateSensitivity;
                verticalRotate = Mathf.Clamp(verticalRotate, verticalMinMax.x, verticalMinMax.y);
                horizontalRotate += Input.GetAxis("Mouse X") * rotateSensitivity;

                currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(verticalRotate, horizontalRotate), ref rotationSmoothVelocity, rotationSmoothTime);
                transform.eulerAngles = currentRotation;

                transform.position = targetPlayer.position - transform.forward * dstFromTarget;
            }

            else if (amLocked == true)
            {                
                /*
                verticalRotate -= Input.GetAxis("Mouse Y") * rotateSensitivity;
                verticalRotate = Mathf.Clamp(verticalRotate, verticalMinMax.x, verticalMinMax.y);
                horizontalRotate += Input.GetAxis("Mouse X") * rotateSensitivity;
                */
                currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(verticalRotate, horizontalRotate), ref rotationSmoothVelocity, rotationSmoothTime);
                transform.eulerAngles = currentRotation;

                
                transform.LookAt(target);
                transform.position = targetPlayer.position - transform.forward * dstFromTarget;
            }
            
        }

        
        
	}

    void SetInitialPrefs()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        List<Transform> targetObjects = new List<Transform>();
        amLocked = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Enemy") == true)
        {
            targetObjects.Add(other.attachedRigidbody.gameObject.GetComponent<Transform>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Enemy") == true)
        {
            targetObjects.Remove(other.attachedRigidbody.gameObject.GetComponent<Transform>());
        }
    }
}
