﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.Data.Mapping;
using UnityEngine.Experimental.PlayerLoop;

[RequireComponent(typeof(CastleShip))]
public class PlayerControlled : MonoBehaviour
{
    private Player playerRef;

    private float deadZone = 0.1f;

    private CastleShip castleShipRef;
    private CastleShip CastleShipRef
    {
        get
        {
            if (castleShipRef == null)
            {
                castleShipRef = GetComponent<CastleShip>();
            }

            return castleShipRef;
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

    public void AssignPlayer(int playerId)
    {
        playerRef = ReInput.players.GetPlayer(playerId);
    }

    private void GetInput()
    {
        Vector3 moveVector = new Vector3(playerRef.GetAxis("MoveHorizontal"),0,playerRef.GetAxis("MoveVertical"));

        //Main Thrust
        CastleShipRef.SetCurrentThrust(0.0f);
        if (moveVector.z > deadZone)
        {
            CastleShipRef.SetCurrentThrust(moveVector.z * CastleShipRef.forwardAcceleration);
        }else if (moveVector.z < deadZone)
        {
            CastleShipRef.SetCurrentThrust(moveVector.z * CastleShipRef.backwardAcceleration);
        }

        //Turning
        CastleShipRef.SetCurrentTurn(0.0f);
        if (Mathf.Abs(moveVector.x) > deadZone)
        {
            CastleShipRef.SetCurrentTurn(moveVector.x);
        }
    }
}
