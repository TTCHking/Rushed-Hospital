using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (draggedItem == null) return;

        InventorySlot fromSlot = draggedItem.parentAfterDrag?.GetComponent<InventorySlot>();
        InventorySlot toSlot = this;

        if (fromSlot == null || toSlot == null) return;

        int fromIndex = System.Array.IndexOf(InventoryManager.Instance.inventorySlots, fromSlot);
        int toIndex = System.Array.IndexOf(InventoryManager.Instance.inventorySlots, toSlot);

        InventoryManager.Instance.SwapUIItems(fromIndex, toIndex);

        // ????????????? parent ??? UI ?????????
        draggedItem.parentAfterDrag = transform;
    }

    
}