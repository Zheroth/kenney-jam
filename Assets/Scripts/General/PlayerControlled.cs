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
    private int playerID = 0;
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
            return playerID;
        }
    }

    void Update()
    {
        if(playerRef!=null)
        {
            GetMovementInput();
            GetActionInput();
        }
    }

    public void AssignPlayer(int playerId)
    {
        this.playerID = playerId;
        playerRef = ReInput.players.GetPlayer(playerId);
    }

    private void GetMovementInput()
    {
        Vector3 moveVector = new Vector3(playerRef.GetAxis("MoveHorizontal"),0,playerRef.GetAxis("MoveVertical"));
        float sideThrust = playerRef.GetAxis("SideThrust");
        Debug.Log(sideThrust);

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

        //Side
        CastleShipRef.SetCurrentSideThrust(0.0f);
        if (Mathf.Abs(sideThrust) > deadZone)
        {
            CastleShipRef.SetCurrentSideThrust(sideThrust * CastleShipRef.sideAcceleration);
        }

        if (playerRef.GetButtonDoublePressDown("SideDodgeLeft"))
        {
            CastleShipRef.SideDodge(false);
        }

        if (playerRef.GetButtonDoublePressDown("SideDodgeRight"))
        {
            CastleShipRef.SideDodge(true);
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
