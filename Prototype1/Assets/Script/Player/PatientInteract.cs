using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PatientInteract : MonoBehaviour
{
    [Header("PatientDied1")]
    public PatientEndurance PatientEndurance;


    [Header("MedicineRoom")]
    public InventoryManager inventoryManager;
    public PlayerController playerController;


    [Header("InvestigateRoom")]
    public Transform warpTarget;
    public  Transform cameraTarget;
    public float cameraMoveDuration = 1f;
    public Cinemachine.CinemachineVirtualCamera investigateVCam;
    public Canvas Investigatetext;
    public GameObject Hotbar;
    public GameObject Book;






    [Header("First time Interact")]
    public LayerMask patientLayer;
    public UIManager uiManager;
    public GameObject ShowOption;
    public PatientMovement patientMovement;
    public GameObject buttonPrefab;
    public TextMeshProUGUI nameButton;
    public Transform imageContainer;
    private PatientData patientData;
    //public TextMeshProUGUI nameText;


    public ScriptableObject Bandage;



    private void Start()
    {
        patientData = GetComponent<PatientData>();

        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();


        if (patientData == null)
            Debug.LogError("PatientData ???? GameObject ???? Null");

        //*InvestigateRoom*
        Investigatetext.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] collidersArray = Physics.OverlapSphere(transform.position, interactRange, patientLayer);
            foreach (Collider collider in collidersArray)
            {
                PatientData data = collider.GetComponent<PatientData>();
                if (data != null)
                {
                    PatientMovement movement = collider.GetComponent<PatientMovement>();
                    if (movement != null)
                    {
                        patientMovement = movement; //
                        patientMovement.SetIsInteract(true);
                        uiManager.SelectPatient(patientMovement);
                    }

                    patientData = data;
                    uiManager.ShowPatientInfo(data);


                    //???????E???Patient????????????????????????????????interact
                    Interact(data);

                    return;
                }
            }
        }
    }

    //???????Interact
    public void Interact(PatientData data)
    {
        if (patientMovement.patientHasDied)
        {
            int index = inventoryManager.selectedSlotIndex;

            GameObject heldItem = playerController.GetHeldItem(index);
            if (heldItem == null)
            {
                Debug.Log("Player handled item");
                return;
            }

            WorldItem worldItem = heldItem.GetComponent<WorldItem>();
            if (worldItem == null)
            {
                Debug.LogWarning("No Item Handle");
                return;
            }

            if (worldItem.objectSO == Bandage)
            {
                // ??? PatientEndurance ????????
                PatientEndurance patientEndurance = patientMovement.GetComponent<PatientEndurance>();
                if (patientEndurance != null)
                {
                    patientEndurance.selectedPatient = patientMovement;
                    patientEndurance.HalfHealing();
                }
                else
                {
                    Debug.LogWarning("????? PatientEndurance ????????????");
                }

                GameObject.Destroy(heldItem);
                inventoryManager.RemoveItemAt(index);
                playerController.SetHeldItem(index, null);
                playerController.UpdateHandItem();
            }

            return;
        }

        if (data == null)
        {
            Debug.LogWarning("PatientData is null");
            return;
        }

        bool alreadyCreated = false;

        //???????????PatientName????????????????PatientData???????????????????

        //??????????????????????
        Debug.Log("Interacting with patient: " + data.patientName);
        PatientMovement movement = data.GetComponent<PatientMovement>();
        if (movement != null)
            movement.SetIsInteract(true);

      

        //??????????????????????????
        Room currentRoom = movement?.GetCurrentRoom();
        if (currentRoom == null)
        {
            Debug.LogWarning("currentRoom is null");
            return;
        }

        Debug.Log("Current Room: " + currentRoom.GetType().Name); // <== ????????? class ???????? ?

        // ??????????? WaitingRoom ??????? E
        if (currentRoom is WaitingRoom)
        {
            uiManager.ShowPatientOption();

            foreach (Transform child in imageContainer)
            {
                TextMeshProUGUI existingText = child.GetComponentInChildren<TextMeshProUGUI>();
                if (existingText != null && existingText.text == data.patientName)
                {
                    alreadyCreated = true;
                    break;
                }

            }

            //????????????????????Patient?????????????????????
            if (!alreadyCreated)
            {
                GameObject newButton = Instantiate(buttonPrefab, imageContainer);

                TextMeshProUGUI textComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                    textComponent.text = data.patientName;
                else
                    Debug.LogWarning("Missing TextMeshProUGUI in button prefab");

                Button button = newButton.GetComponent<Button>();
                if (button != null && uiManager != null)
                {
                    button.onClick.AddListener(() => uiManager.ShowPatientInfo(data));
                }
            }

        }

        // ??????????? InvestigateRoom ??????? E
        else if (currentRoom is InvestigateRoom)
        {
            if (warpTarget != null)
            {
                uiManager.ExitShowPatientOption();
                transform.position = warpTarget.position;
                transform.rotation = warpTarget.rotation;
                Investigatetext.gameObject.SetActive(true);
                Hotbar.gameObject.SetActive(false);
                Book.gameObject.SetActive(false);

                PlayerController controller = data.GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.SetCanMove(false);
                }

                if (investigateVCam != null)
                {
                    investigateVCam.Priority = 20;
                }




                DiseaseData diseaseData = data.GetDiseaseData();
                if (diseaseData != null && diseaseData.Asking is AskingDatabase askingData)
                {
                    InvestigateRoom room = currentRoom as InvestigateRoom;
                    room.EnterRoom(data); // ???????????? set currentPatient
                    room.SetupQuestionButtons(askingData); // <-- ???????????
                }
                else
                {
                    Debug.LogWarning("Missing DiseaseData or AskingDatabase.");
                }

            }


        }

        else if (currentRoom is MedicineRoom)
        {

            int index = inventoryManager.selectedSlotIndex;

            GameObject heldItem = playerController.GetHeldItem(index);
            if (heldItem == null)
            {
                Debug.Log("Player handled item");
                return;
            }

            WorldItem worldItem = heldItem.GetComponent<WorldItem>();
            if (worldItem == null)
            {
                Debug.LogWarning("No Item Handle");
                return;
            }

            if (patientData == null)
            {
                Debug.LogWarning("No Patient data");
                return;
            }

            if (worldItem.objectSO == patientData.correctMedicine)
            {
                Debug.Log("Correct Medicine");
                uiManager.HospitalReputation += 20;
                //Plus Reputation
            }
            else
            {
                Debug.Log("Wrong Medicine");
                uiManager.HospitalReputation -= 20;
                //Minus Reputation
            }

            GameObject.Destroy(heldItem);
            inventoryManager.RemoveItemAt(index);
            playerController.SetHeldItem(index, null);
            playerController.UpdateHandItem();
        }

        // ??????????? BloodTestRoom ??????? E
        else if (currentRoom is BloodTestRoom)
        {
            uiManager.ShowPatientOption();
        }

        // ??????????? XrayRoom ??????? E
        else if (currentRoom is XrayRoom)
        {
            uiManager.ShowPatientOption();
        }

     
    }
}

    


