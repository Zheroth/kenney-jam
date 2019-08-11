using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private List<CastleShip> availableCastleShips;

    private Dictionary<CastleShip.CastleShipType, GameObject> shipDict = new Dictionary<CastleShip.CastleShipType, GameObject>();

    public bool doStuff;

    void Start()
    {
        for(int i=0; i<availableCastleShips.Count; i++)
        {
            if (!shipDict.ContainsKey(availableCastleShips[i].shipType))
            {
                shipDict.Add(availableCastleShips[i].shipType, availableCastleShips[i].gameObject);
            }
        }
    }

    void Update()
    {
        if (doStuff)
        {
            SpawnShip(CastleShip.CastleShipType.Assaulter, 0, new Vector3(170,50,45));
            doStuff = false;
        }
    }

    public void SpawnShip(CastleShip.CastleShipType shipType, int playerId, Vector3 position)
    {
        GameObject newShip = Instantiate(shipDict[shipType], position, Quaternion.identity);
        newShip.GetComponent<PlayerControlled>().AssignPlayer(playerId);
    }


}
