using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectGridArrange : MonoBehaviour
{
    public GameObject[] gameObjects;

    public int x = 12;
    public float distance = 1;
    public float yPos = 0.5f;

    [ContextMenu("Arrange")]
    public void Arrange()
    {
        int xPos = 0;
        int zPos = 0;
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if(xPos > x-1)
            {
                xPos = 0;
                zPos++;
            }

            gameObjects[i].transform.localPosition = new Vector3(xPos * distance, yPos, zPos * distance);

            xPos++;
        }
    }
}
