using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;

public class CastleShip : MonoBehaviour
{
    public float forwardAcceleration = 1200.0f;
    public float backwardAcceleration = 1200.0f;
    public float turnStrength = 10.0f;
    public float hoverForce = 5.0f;
    public float hoverHeight = 3.0f;
    public GameObject[] hoverPoints;

    public OnCastleShipUseAction OnActionA;
    public OnCastleShipUseAction OnActionB;
    public OnCastleShipUseAction OnActionC;
    public OnCastleShipUseAction OnActionD;

    private float currentThrust = 0.0f;
    private float currentTurn = 0.0f;

    private Rigidbody rigidbodyRef;
    public Rigidbody RigidbodyRef
    {
        get
        {
            if (rigidbodyRef == null)
            {
                rigidbodyRef = GetComponent<Rigidbody>();
            }

            return rigidbodyRef;
        }
    }

    void FixedUpdate()
    {
        //Hover force
        RaycastHit hit;
        for (int i = 0; i < hoverPoints.Length; i++)
        {
            GameObject hoverPoint = hoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, hoverHeight))
            {
                float hoverDistance = hit.distance / hoverHeight;
                RigidbodyRef.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - hoverDistance), hoverPoint.transform.position);
            }
            else
            {
                if (transform.position.y > hoverPoint.transform.position.y)
                {
                    RigidbodyRef.AddForceAtPosition(hoverPoint.transform.up * hoverForce, hoverPoint.transform.position);
                }
                else
                {
                    RigidbodyRef.AddForceAtPosition(hoverPoint.transform.up * -hoverForce, hoverPoint.transform.position);
                }
            }
        }

        if (Mathf.Abs(currentThrust) > 0)
        {
            RigidbodyRef.AddForce(transform.forward * currentThrust * Time.deltaTime);
        }

        if (currentTurn != 0)
        {
            RigidbodyRef.AddRelativeTorque(Vector3.up * currentTurn * turnStrength * Time.deltaTime);
        }
    }

    public void SetCurrentThrust(float newThrust)
    {
        currentThrust = newThrust;
    }

    public void SetCurrentTurn(float newTurn)
    {
        currentTurn = newTurn;
    }

    public void FireActionA()
    {
        Debug.Log("ActionA");
        OnActionA?.Invoke(this, "ActionA");
    }

    public void FireActionB()
    {
        Debug.Log("ActionB");
        OnActionB?.Invoke(this, "ActionB");
    }

    public void FireActionC()
    {
        Debug.Log("ActionC");
        OnActionC?.Invoke(this, "ActionC");
    }

    public void FireActionD()
    {
        Debug.Log("ActionD");
        OnActionD?.Invoke(this, "ActionD");
    }
}

[System.Serializable]
public class OnCastleShipUseAction : UnityEvent<CastleShip,string>
{

}
