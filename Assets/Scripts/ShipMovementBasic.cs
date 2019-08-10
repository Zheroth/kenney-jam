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


        if (Input.GetKey(up) || Input.GetKey(down) || Input.GetKey(left) || Input.GetKey(right))
        {
            Vector3 direction = Vector3.zero;
            if(Input.GetKey(up))
            {
                direction += new Vector3(0, 0, 1);
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + speedDelta);
            }
            if(Input.GetKey(down))
            {
                direction += new Vector3(0, 0, -1);
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - speedDelta);
            }
            if (Input.GetKey(left))
            {
                direction += new Vector3(-1, 0, 0);
                this.transform.position = new Vector3(this.transform.position.x - speedDelta, this.transform.position.y, this.transform.position.z);
            }
            if (Input.GetKey(right))
            {
                direction += new Vector3(1, 0, 0);
                this.transform.position = new Vector3(this.transform.position.x + speedDelta, this.transform.position.y, this.transform.position.z);
            }
            this.transform.forward = direction;
        }

    }
}
