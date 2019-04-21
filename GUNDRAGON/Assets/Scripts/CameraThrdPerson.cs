using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraThrdPerson : MonoBehaviour
{
    public float rotateSensitivity;
    public Transform target;
    private Transform targetPlayer;
    //public List<Transform> targetObjects;
    public Queue<Transform> targets = new Queue<Transform>();

    public static bool amLocked = false;

    public float dstFromTarget = 4;
    public Vector2 verticalMinMax = new Vector2(-30, 60);

    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public float verticalRotate;
    public float horizontalRotate;

    void Start()
    {
        SetInitialPrefs();
    }

    void Update()
    {

        if (targets.Count == 0)
            amLocked = false;

        if (Input.GetMouseButtonDown(2))
        {
            if (!amLocked)
                CycleTarget();
            else
                amLocked = false;
        }

        if (!amLocked)
            target = targetPlayer;
        else
        {

            if (Input.GetKeyDown(KeyCode.Tab))
                CycleTarget();
        }
    }

    void LateUpdate()
    {
        if (amLocked == false)
        {
            transform.LookAt(targetPlayer);

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
            verticalRotate = 40.0f;
            transform.position = targetPlayer.position - transform.forward * dstFromTarget;
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
        if (other.CompareTag("Enemy"))
            targets.Enqueue(other.attachedRigidbody.transform);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            targets = new Queue<Transform>(targets.Where(x => x != other.attachedRigidbody.transform));
    }

    private void CycleTarget()
    {
        if(targets.Count <= 0)
        {
            amLocked = false;
            return;
        }

        amLocked = true;
        if (target && target != targetPlayer)
            targets.Enqueue(target);
        target = targets.Dequeue();
    }
}
