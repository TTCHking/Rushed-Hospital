using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoilingPot : MonoBehaviour, IInteractable
{
    [Header("ตำแหน่งที่จะ Spawn ผลผลิต")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float progressCount;
    [SerializeField] private float progressendurance;
    [SerializeField] private Image ProgressBar;
    [SerializeField] private Canvas BoilingProgressBar;
    private bool isBoiling = false;

    public void Start()
    {
        BoilingProgressBar.gameObject.SetActive(false);
    }

    public void Interaction(GameObject interactor)
    {
        var inventory = InventoryManager.Instance;
        int selectedIndex = inventory.selectedSlotIndex;

        // ดึง InventorySlot
        InventorySlot slot = inventory.inventorySlots[selectedIndex];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot == null)
        {
            Debug.Log("ไม่มีไอเท็มในช่องนี้");
            return;
        }

        ObjectSO objectSO = itemInSlot.objectSO;
        if (objectSO.boilingResultPrefab == null)
        {
            Debug.Log("ไอเท็มนี้ไม่สามารถต้มได้");
            return;
        }

        // Spawn prefab ที่กำหนดไว้ใน boilingResultPrefab
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;

        GameObject heldItem = inventory.playerController.GetHeldItem(selectedIndex);
        if (heldItem != null)
        {
            GameObject.Destroy(heldItem);
            inventory.playerController.SetHeldItem(selectedIndex, null);
        }

        inventory.RemoveItemAt(selectedIndex);

        inventory.playerController.UpdateHandItem();

        Debug.Log($"กำลังต้ม {objectSO.name}... รอ 5 วินาที");

        StartCoroutine(SpawnAfterDelay(objectSO, spawnPos));//Instantiate(objectSO.boilingResultPrefab, spawnPos, Quaternion.identity);

        progressCount = progressendurance;
        isBoiling = true;




    }

    private void Update()
    {
        if (progressCount > 0 && isBoiling)
        {

            BoilingProgressBar.gameObject.SetActive(true);
            progressCount -= Time.deltaTime;
            ProgressBar.fillAmount = progressCount * (1 / progressendurance);

            if (progressCount <= 0)
            {
                BoilingProgressBar.gameObject.SetActive(false);
            }


        }
    }

    private IEnumerator SpawnAfterDelay(ObjectSO objectSO, Vector3 spawnPos)
    {
        Debug.Log($"กำลังต้ม {objectSO.name}... รอ 5 วินาที");

        yield return new WaitForSeconds(5f); // ดีเลย์ 5 วิ

        Instantiate(objectSO.boilingResultPrefab, spawnPos, Quaternion.identity);

        Debug.Log("ต้มสำเร็จ ได้ {objectSO.boilingResultPrefab.name}");

    }


}



