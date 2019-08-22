using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelectable : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private UnityEvent onPointerEnter;

    [SerializeField]
    private UnityEvent onLeft;
    [SerializeField]
    private UnityEvent onRight;

    [SerializeField]
    private Image selectionImage;
    public void Select()
    {
        this.selectionImage.enabled = true;
    }
    public void Deselect()
    {
        this.selectionImage.enabled = false;
    }

    public void OnUpdate(Player playerRef)
    {
        if (playerRef.GetButtonDown("Left"))
        {
            onLeft?.Invoke();
        }
        else if (playerRef.GetButtonDown("Right"))
        {
            onRight?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke();
    }
}
