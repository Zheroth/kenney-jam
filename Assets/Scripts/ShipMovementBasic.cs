using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementBasic : MonoBehaviour
{
    [SerializeField]
    private KeyCode up = KeyCode.UpArrow;
    [SerializeField]
    private KeyCode down = KeyCode.DownArrow;
    [SerializeField]
    private KeyCode left = KeyCode.LeftArrow;
    [SerializeField]
    private KeyCode right = KeyCode.RightArrow;

    [SerializeField]
    private float speed = 5;

    // Update is called once per frame
    void Update()
    {
        float speedDelta = speed * Time.deltaTime;
        if(Input.GetKey(up))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+ speedDelta);
        }
        if(Input.GetKey(down))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - speedDelta);
        }
        if (Input.GetKey(left))
        {
            this.transform.position = new Vector3(this.transform.position.x - speedDelta, this.transform.position.y, this.transform.position.z);
        }
        if (Input.GetKey(right))
        {
            this.transform.position = new Vector3(this.transform.position.x + speedDelta, this.transform.position.y, this.transform.position.z);
        }
    }
}
