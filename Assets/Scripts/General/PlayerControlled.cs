using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.Data.Mapping;
using UnityEngine.Experimental.PlayerLoop;

[RequireComponent(typeof(CastleShip))]
public class PlayerControlled : MonoBehaviour
{
    [SerializeField]
    Color DEBUG_Colour;

    [SerializeField]
    private int DEBUG_player = 0;
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

    public int PlayerID
    {
        get
        {
            return DEBUG_player;
        }
    }

    void Awake()
    {
        //DEBUG
        AssignPlayer(DEBUG_player);
        CastleShipRef.SetColourMaterial(DEBUG_Colour);
    }

    void Update()
    {
        GetMovementInput();
        GetActionInput();
    }

    public void AssignPlayer(int playerId)
    {
        playerRef = ReInput.players.GetPlayer(playerId);
    }

    private void GetMovementInput()
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

    private void GetActionInput()
    {
        if (playerRef.GetButton("ActionA"))
        {
            CastleShipRef.FireActionA();
        }

        if (playerRef.GetButton("ActionB"))
        {
            CastleShipRef.FireActionB();
        }

        if (playerRef.GetButton("ActionC"))
        {
            CastleShipRef.FireActionC();
        }

        if (playerRef.GetButton("ActionD"))
        {
            CastleShipRef.FireActionD();
        }
    }
}
