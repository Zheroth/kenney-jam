﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.Data.Mapping;
using UnityEngine.Experimental.PlayerLoop;

public class Controllable : MonoBehaviour
{
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float maxSpeed = 20.0f;
    [SerializeField] private float breakSpeed = 20.0f;
    [SerializeField] private float turnSpeed = 50.0f;

    [SerializeField] private float hoverHeight = 1.5f;
    [SerializeField] private float heightSmooth = 2.0f;
    [SerializeField] private float pitchSmooth = 5.0f;

    private Vector3 prevUp;
    public float yaw;
    private float smoothY;
    private float currentSpeed;

    private Player playerRef;
    private Vector3 moveVector;

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
        //ProcessInput();
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
        moveVector.x = playerRef.GetAxis("MoveHorizontal");
        moveVector.z = playerRef.GetAxis("MoveVertical");
    }

    private void ProcessInput()
    {
        //Update speed
        if(moveVector.z > Mathf.Epsilon)
        {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
        }else if (moveVector.z < -Mathf.Epsilon)
        {
            if (currentSpeed > -maxSpeed)
            {
                currentSpeed -= acceleration * Time.deltaTime;
            }
        }
        else
        {
            if (currentSpeed > Mathf.Epsilon)
            {
                currentSpeed -= breakSpeed * Time.deltaTime;
                if (currentSpeed < 0)
                {
                    currentSpeed = 0;
                }
            }else if (currentSpeed < Mathf.Epsilon)
            {
                currentSpeed += breakSpeed * Time.deltaTime;
                if (currentSpeed > 0)
                {
                    currentSpeed = 0;
                }
            }else
            {
                currentSpeed = 0;
            }
        }

        //Update yaw
        yaw += turnSpeed * moveVector.x * Time.deltaTime;

        prevUp = transform.up;
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        Debug.DrawRay(transform.position+Vector3.up, -Vector3.up, Color.blue);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Debug.DrawLine(transform.position, hit.point);

            //Calculate new up vector
            //Vector3 newUp = Vector3.Lerp(prevUp, hit.normal, pitchSmooth * Time.deltaTime);

            //Tilt angle
            //Quaternion tilt = Quaternion.FromToRotation(transform.up, newUp);

            //Apply to the ship
            //transform.rotation = tilt * transform.rotation;

            //Adjust height
            smoothY = Mathf.Lerp(smoothY, hoverHeight - hit.distance, heightSmooth * Time.deltaTime);
            transform.localPosition += prevUp * smoothY;
        }

        //Move ship forward
        transform.position += transform.forward * (currentSpeed * Time.deltaTime);
    }
}