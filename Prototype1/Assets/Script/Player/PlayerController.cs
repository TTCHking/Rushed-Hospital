using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static IInteractable;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movespeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask objectLayerMask;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera playerVCam;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera investigateVCam;
    public Canvas InvestigateText;
    public GameObject Hotbar;
    public GameObject Book;
    public UIManager uIManager;

    private bool iswalking;
    private Vector3 lastinteractDir;
    private Phone selectedObject;
    private bool canMove = true;



    private void Update()
    {
        HandleMovement();
        HandleInteraction();
        HandleDrop();
        HandlePickup();
        HandleSlotSelection();
    }

    private void HandleMovement()
    {
        if (!canMove) return; 

        Vector2 inputVector = gameInput.GetMovementVectorNormorlized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = movespeed * Time.deltaTime;
        float playerRadius = 0.75f;
        float playerHeight = 1f;
        bool canMoveCheck = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMoveCheck) 
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMoveCheck = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMoveCheck) 
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMoveCheck = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMoveCheck)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // can Move any Direction
                }
            }

        }

        if (canMoveCheck)
        {
            transform.position += moveDir * moveDistance;
        }


        iswalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return iswalking;
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormorlized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastinteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastinteractDir, out RaycastHit raycastHit, interactDistance, objectLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out Phone @object))
            {
                if (@object != selectedObject)
                {
                    SetSelectedObject(@object);
                }

            }
            else
            {
                SetSelectedObject(null);
            }

        }
        else
        {
            SetSelectedObject(null);
        }
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        inventoryManager = FindObjectOfType<InventoryManager>();
        
    }
    [SerializeField] private float interactRange = 2f;
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormorlized();
        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);

        // ป้องกันการยิง Ray ถ้าผู้เล่นไม่ได้กดปุ่มเดิน
        if (direction == Vector3.zero) return;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f; // ให้ ray ยกขึ้นมานิดจากพื้น
        Ray ray = new Ray(rayOrigin, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, objectLayerMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interaction(this.gameObject);
            }
        }

#if UNITY_EDITOR
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.yellow, 1f);
#endif
    }

    public static PlayerController Instance { get; private set; }



    public event EventHandler<OnSelectedObjectChangedEventArgs> OnSelectedObjectChanged;
    public class OnSelectedObjectChangedEventArgs : EventArgs
    {
        public Phone selectedObject;

    }

    private void SetSelectedObject(Phone selectedObject)
    {
        this.selectedObject = selectedObject;
    }

    void HandleSlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventoryManager.selectedSlotIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventoryManager.selectedSlotIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventoryManager.selectedSlotIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) inventoryManager.selectedSlotIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) inventoryManager.selectedSlotIndex = 4;

        UpdateHandItem();
    }


    public Transform handPosition;
    public float pickupRange = 2f;
    public string itemTag = "Item";

    private bool isHoldingItem = false;
    private Dictionary<int, GameObject> heldItems = new();
    private InventoryManager inventoryManager;

    void HandlePickup()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            int index = inventoryManager.selectedSlotIndex;

            if (heldItems.ContainsKey(index))
            {
                Debug.Log("ช่องนี้มีของอยู่แล้ว ไม่สามารถหยิบซ้ำได้");
                return;
            }

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickupRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag(itemTag))
                {
                    GameObject currentItem = hitCollider.gameObject;
                    WorldItem worldItem = currentItem.GetComponent<WorldItem>();

                    if (worldItem != null)
                    {
                        currentItem.transform.SetParent(handPosition);
                        currentItem.transform.localPosition = Vector3.zero;
                        currentItem.transform.localRotation = Quaternion.identity;

                        heldItems[index] = currentItem;

                        inventoryManager.AddObjectAt(worldItem.objectSO, index);
                        UpdateHandItem();
                        Debug.Log("หยิบของเข้า inventory: " + worldItem.objectSO.name);
                    }

                    break;
                }
            }
        }
    }


    void HandleDrop()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int index = inventoryManager.selectedSlotIndex;

            if (heldItems.ContainsKey(index))
            {
                GameObject itemToDrop = heldItems[index];
                WorldItem worldItem = itemToDrop.GetComponent<WorldItem>();

                if (worldItem != null && worldItem.objectSO.Prefab != null)
                {
                    Instantiate(worldItem.objectSO.Prefab, transform.position + transform.forward * 2f, Quaternion.identity);
                    Debug.Log("ดรอปของ: " + worldItem.objectSO.name);
                }

                heldItems.Remove(index);
                Destroy(itemToDrop);
                inventoryManager.RemoveItemAt(index);
                UpdateHandItem();
            }
        }
    }

    public void UpdateHandItem()
    {
        foreach (Transform child in handPosition)
        {
            child.gameObject.SetActive(false);
        }

        int index = inventoryManager.selectedSlotIndex;

        if (heldItems.ContainsKey(index))
        {
            heldItems[index].SetActive(true);
        }
    }

    public GameObject GetHeldItem(int index)
    {
        return heldItems.ContainsKey(index) ? heldItems[index] : null;
    }

    public void SetHeldItem(int index, GameObject item)
    {
        if (item == null)
        {
            heldItems.Remove(index);
        }
        else
        {
            heldItems[index] = item;
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        Debug.Log("CanMove ถูกตั้งค่าเป็น: " + value);
    }

    public void ExitInvestigate()
    {
      
        Debug.Log("เรียก ExitInvestigate แล้ว");

        if (playerVCam == null)
        {
            Debug.LogError("playerVCam ยังไม่ได้ assign!");
        }
        else
        {
            playerVCam.Priority = 20;
            Debug.Log("playerVCam.Priority ถูกตั้งเป็น 20");
        }

        if (investigateVCam == null)
        {
            Debug.LogError("investigateVCam ยังไม่ได้ assign!");
        }
        else
        {
            investigateVCam.Priority = 10;
        }

        SetCanMove(true);
        InvestigateText.gameObject.SetActive(false);
        Hotbar.gameObject.SetActive(true);
        Book.gameObject.SetActive(true);
        uIManager.ShowAnswerCanvas.gameObject.SetActive(false);

    }
}









