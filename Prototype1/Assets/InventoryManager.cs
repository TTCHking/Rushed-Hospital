using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject InventoryItemPrefab;
    public int selectedSlotIndex = 0;


    public GameObject worldItemPrefab;

    // Dictionary ??? List ????? map slot index -> GameObject (item ?????)
    public Dictionary<int, GameObject> pickedItemDict = new();

    public void AddObjectAt(ObjectSO objectSO, int index)
    {
        if (index >= 0 && index < inventorySlots.Length)
        {
            InventorySlot slot = inventorySlots[index];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                Debug.LogWarning("???????????????????? ????????????????????");
                return;
            }

            SpawnNewObject(objectSO, slot);
        }
        else
        {
            Debug.LogWarning("Index ????????????? AddObjectAt");
        }
    }

    void SpawnNewObject(ObjectSO objectSO, InventorySlot slot)
    {
        GameObject newObjectGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newObjectGo.GetComponentInChildren<InventoryItem>();
        inventoryItem.InitialiseItem(objectSO);
    }

    public void RemoveItemAt(int index)
    {
        if (index >= 0 && index < inventorySlots.Length)
        {
            InventorySlot slot = inventorySlots[index];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);
                Debug.Log("?????????? UI: " + index);
            }

            // ?? object ????????? Dictionary
            if (pickedItemDict.ContainsKey(index))
            {
                GameObject item = pickedItemDict[index];
                pickedItemDict.Remove(index);
                if (item != null) Destroy(item);
            }
        }
        else
        {
            Debug.LogWarning("Index ????????????? RemoveItemAt");
        }
    }

    public void SwapItems(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex ||
            fromIndex < 0 || fromIndex >= inventorySlots.Length ||
            toIndex < 0 || toIndex >= inventorySlots.Length)
            return;

        var temp = pickedItemDict.ContainsKey(fromIndex) ? pickedItemDict[fromIndex] : null;

        if (pickedItemDict.ContainsKey(toIndex))
        {
            pickedItemDict[fromIndex] = pickedItemDict[toIndex];
        }
        else
        {
            pickedItemDict.Remove(fromIndex);
        }

        if (temp != null)
        {
            pickedItemDict[toIndex] = temp;
        }
    }

    private void Start()
    {
        selectedSlotIndex = 0;
    }

    public static InventoryManager Instance { get; private set; }
    public PlayerController playerController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SwapUIItems(int fromIndex, int toIndex)
    {
        // 1. ???? Sprite UI
        Transform fromSlot = inventorySlots[fromIndex].transform;
        Transform toSlot = inventorySlots[toIndex].transform;

        InventoryItem fromItem = fromSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem toItem = toSlot.GetComponentInChildren<InventoryItem>();

        if (fromItem != null) fromItem.transform.SetParent(toSlot);
        if (toItem != null) toItem.transform.SetParent(fromSlot);

        // 2. ???? GameObject ??????????
        GameObject fromGO = playerController.GetHeldItem(fromIndex);
        GameObject toGO = playerController.GetHeldItem(toIndex);

        playerController.SetHeldItem(fromIndex, toGO);
        playerController.SetHeldItem(toIndex, fromGO);

        // 3. ??????????????????????????????????????????
        playerController.UpdateHandItem();
    }

    public ObjectSO GetObjectAt(int index)
    {
        if (index >= 0 && index < inventorySlots.Length)
        {
            InventoryItem item = inventorySlots[index].GetComponentInChildren<InventoryItem>();
            return item?.objectSO;
        }
        return null;
    }

   // ??????????????????prefab

}

