using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [Header("UI")]
    public Image image;
    [HideInInspector]public ObjectSO objectSO;
    [HideInInspector] public Transform parentAfterDrag;

  
    public void InitialiseItem(ObjectSO newObjectSO) {
        objectSO = newObjectSO;
        image.sprite = newObjectSO.sprite;

    }
    public void OnBeginDrag(PointerEventData eventData) { 
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
