using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineTargetGroup targetGroup;
    [SerializeField] private List<CastleShip> availableCastleShips;

    private Dictionary<CastleShip.CastleShipType, GameObject> shipDict;

    private Dictionary<CastleShip.CastleShipType, GameObject> ShipDict
    {
        get
        {
            if (shipDict == null)
            {
                shipDict = new Dictionary<CastleShip.CastleShipType, GameObject>();

                for(int i=0; i<availableCastleShips.Count; i++)
                {
                    if (!shipDict.ContainsKey(availableCastleShips[i].shipType))
                    {
                        shipDict.Add(availableCastleShips[i].shipType, availableCastleShips[i].gameObject);
                    }
                }
            }

            return shipDict;
        }
    }

    public CastleShip SpawnShip(CastleShip.CastleShipType shipType, int playerId, Transform spawnTransform)
    {
        GameObject newShip = Instantiate(ShipDict[shipType], spawnTransform);
        newShip.GetComponent<PlayerControlled>().AssignPlayer(playerId);
        targetGroup.AddMember(newShip.transform,1.0f,3.0f);
        return newShip.GetComponent<CastleShip>();
    }

    public CastleShip GetShip(CastleShip.CastleShipType shipType)
    {
        return ShipDict[shipType].GetComponent<CastleShip>();
    }
}
