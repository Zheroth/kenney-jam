using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControlled : MonoBehaviour
{
    [SerializeField]
    private CastleShip castleShip;
    [SerializeField]
    private Color playerColour;
    
    // Start is called before the first frame update
    void Start()
    {
        castleShip.SetColourMaterial(this.playerColour);
    }
}
