using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;

[RequireComponent(typeof(Damageable))]
public class CastleShip : MonoBehaviour
{
    public CastleShipType shipType;

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

    public HumanPlayer.OnGoldChanged OnGoldChanged;

    private float currentThrust = 0.0f;
    private float currentTurn = 0.0f;

    private float thrustModifier = 1.0f;

    private List<Modifier> modifierList = new List<Modifier>();

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

    private Damageable damageableRef;
    public Damageable DamageableRef
    {
        get
        {
            if (damageableRef == null)
            {
                damageableRef = GetComponent<Damageable>();
            }

            return damageableRef;
        }
    }

    [SerializeField]
    private Sprite shipImage;
    public Sprite Image { get { return shipImage; } }
    [SerializeField]
    private string shipName;
    public string ShipName{ get { return shipName; } }

    void Update()
    {
        UpdateModifiers();
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
            RigidbodyRef.AddForce(transform.forward * currentThrust * thrustModifier * Time.deltaTime);
        }

        if (currentTurn != 0)
        {
            RigidbodyRef.AddRelativeTorque(Vector3.up * currentTurn * turnStrength * Time.deltaTime);
        }
    }

    public void SetColourMaterial(Color color)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;
            for (int j = 0; j < materials.Length; j++)
            {
                if(materials[j].name == "PLAYERCOLOUR" || materials[j].name == "PLAYERCOLOUR (Instance)")
                {
                    materials[j].color = color;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Treasure treasure;
        if(other.TryGetComponent<Treasure>(out treasure))
        {
            this.AddGold(treasure.Gold);
            GameObject.Destroy(treasure.gameObject);
            return;
        }

        SpeedPickup speedPickup;
        if (other.TryGetComponent<SpeedPickup>(out speedPickup))
        {
            AddModifier(new SpeedModifier(), 8.0f);
            GameObject.Destroy(speedPickup.gameObject);
            return;
        }
    }

    public void AddGold(int gold)
    {
        OnGoldChanged?.Invoke(gold);
    }

    public void SetCurrentThrust(float newThrust)
    {
        currentThrust = newThrust;
    }

    public void SetCurrentTurn(float newTurn)
    {
        currentTurn = newTurn;
    }

    public void SetCurrentThrustModifier(float newThrustModifier)
    {
        thrustModifier = newThrustModifier;
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

    public void AddModifier(Modifier modifier, float durations)
    {
        modifier.Duration = durations;
        modifier.ActivateModifier(this);
        modifierList.Add(modifier);
    }

    public void UpdateModifiers()
    {
        for (int i = modifierList.Count-1; i >= 0; i--)
        {
            modifierList[i].Duration -= Time.deltaTime;
            if (modifierList[i].Duration <= 0)
            {
                modifierList[i].DeactivateModifier(this);
                modifierList.RemoveAt(i);
            }
        }
    }

    public enum CastleShipType
    {
        Assaulter,
        Tank,
        Nimble
    }

    [System.Serializable]
    public class OnCastleShipUseAction : UnityEvent<CastleShip,string>
    {

    }
}
