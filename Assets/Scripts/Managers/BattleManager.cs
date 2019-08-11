using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    Cinemachine.CinemachineTargetGroup targetGroup;

    [SerializeField] private List<CastleShip> availableCastleShips;

    private Dictionary<CastleShip.CastleShipType, GameObject> shipDict = new Dictionary<CastleShip.CastleShipType, GameObject>();

    void Awake()
    {
        for(int i=0; i<availableCastleShips.Count; i++)
        {
            if (!shipDict.ContainsKey(availableCastleShips[i].shipType))
            {
                shipDict.Add(availableCastleShips[i].shipType, availableCastleShips[i].gameObject);
            }
        }
    }

    public CastleShip SpawnShip(CastleShip.CastleShipType shipType, int playerId, Transform spawnTransform)
    {
        GameObject newShip = Instantiate(shipDict[shipType], spawnTransform);
        newShip.GetComponent<PlayerControlled>().AssignPlayer(playerId);
        targetGroup.AddMember(newShip.transform, 1, 0);
        return newShip.GetComponent<CastleShip>();
    }


}
