using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.Data.Mapping;
using UnityEngine.Experimental.PlayerLoop;

public class Controllable : MonoBehaviour
{
    private Player playerRef;

    private float deadZone = 0.1f;
    public float forwardAcceleration = 1200.0f;
    public float backwardAcceleration = 1200.0f;
    private float currentThrust = 0.0f;

    public float turnStrength = 10.0f;
    private float currentTurn = 0.0f;

    private int layerMask;
    public float hoverForce = 5.0f;
    public float hoverHeight = 3.0f;
    public GameObject[] hoverPoints;

    private Rigidbody rigidbodyRef;
    public Rigidbody RigidbodyRef
    {
        get
        {
            if (rigidbodyRef == null)
            {
                rigidbodyRef = GetComponent<Rigidbody>();
            }

            return rigidbodyRef;
        }
    }

    void Awake()
    {
        //DEBUG
        AssignPlayer(0);
    }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        ProcessInput();
    }

    public void AssignPlayer(int playerId)
    {
        playerRef = ReInput.players.GetPlayer(playerId);
    }

    private void GetInput()
    {
        
        Vector3 moveVector = new Vector3(playerRef.GetAxis("MoveHorizontal"),0,playerRef.GetAxis("MoveVertical"));
        Debug.Log(moveVector);

        //Main Thrust
        currentThrust = 0.0f;
        if (moveVector.z > deadZone)
        {
            currentThrust = moveVector.z * forwardAcceleration;
        }else if (moveVector.z < deadZone)
        {
            currentThrust = moveVector.z * backwardAcceleration;
        }

        //Turning
        currentTurn = 0.0f;
        if (Mathf.Abs(moveVector.x) > deadZone)
        {
            currentTurn = moveVector.x;
        }
    }

    private void ProcessInput()
    {
        //Hover force
        RaycastHit hit;
        for (int i = 0; i < hoverPoints.Length; i++)
        {
            GameObject hoverPoint = hoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, hoverHeight))
            {
                float hoverDistance = hit.distance / hoverHeight;
                RigidbodyRef.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - hoverDistance), hoverPoint.transform.position);
            }
            else
            {
                if (transform.position.y > hoverPoint.transform.position.y)
                {
                    RigidbodyRef.AddForceAtPosition(hoverPoint.transform.up * hoverForce, hoverPoint.transform.position);
                }
                else
                {
                    RigidbodyRef.AddForceAtPosition(hoverPoint.transform.up * -hoverForce, hoverPoint.transform.position);
                }
            }
        }

        if (Mathf.Abs(currentThrust) > 0)
        {
            RigidbodyRef.AddForce(transform.forward * currentThrust * Time.deltaTime);
        }

        if (currentTurn != 0)
        {
            RigidbodyRef.AddRelativeTorque(Vector3.up * currentTurn * turnStrength * Time.deltaTime);
        }
    }
}
