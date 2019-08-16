using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineTargetGroup targetGroup;
    [SerializeField] private List<CastleShip> availableCastleShips;
    [SerializeField] private List<CastleShip> availableAICastleShips;

    private int roundNumber;
    private List<GamePlayer> humanPlayers = new List<GamePlayer>();
    private GamePlayer currentKing = null;

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

    private Dictionary<CastleShip.CastleShipType, GameObject> aiShipDict;

    private Dictionary<CastleShip.CastleShipType, GameObject> AIShipDict
    {
        get
        {
            if (aiShipDict == null)
            {
                aiShipDict = new Dictionary<CastleShip.CastleShipType, GameObject>();

                for (int i = 0; i < availableAICastleShips.Count; i++)
                {
                    if (!aiShipDict.ContainsKey(availableAICastleShips[i].shipType))
                    {
                        aiShipDict.Add(availableAICastleShips[i].shipType, availableAICastleShips[i].gameObject);
                    }
                }
            }

            return aiShipDict;
        }
    }


    public CastleShip SpawnShip(CastleShip.CastleShipType shipType, int playerId, Transform spawnTransform, bool ai = false)
    {
        GameObject newShip = ai ? Instantiate(AIShipDict[shipType], spawnTransform) : Instantiate(ShipDict[shipType], spawnTransform);
        newShip.transform.position = spawnTransform.position;
        newShip.transform.rotation = spawnTransform.rotation;
        if (!ai) { newShip.GetComponent<PlayerControlled>().AssignPlayer(playerId); }
        targetGroup.AddMember(newShip.transform,1.0f,3.0f);
        return newShip.GetComponent<CastleShip>();
    }

    public CastleShip GetShip(CastleShip.CastleShipType shipType)
    {
        return ShipDict[shipType].GetComponent<CastleShip>();
    }
}
