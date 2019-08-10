using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utils;
using Rewired;

public class GameManager : MonoSingleton<GameManager>
{
    public Player[] playerArr = new Player[4];

    void Update()
    {
        IList<Joystick> joystickLists = ReInput.controllers.Joysticks;
        for (int i = 0; i < joystickLists.Count; i++)
        {
            if (joystickLists[i].GetAnyButtonDown())
            {
                SpawnPlayer(joystickLists[i].id);
            }
        }
    }

    public void SpawnPlayer(int playerId)
    {
        Debug.Log("Spawn player: "+playerId);
    }
}
