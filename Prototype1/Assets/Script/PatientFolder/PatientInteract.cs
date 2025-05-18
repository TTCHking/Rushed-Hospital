using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PatientInteract : MonoBehaviour
{
    [Header("InvestigateRoom")]
    [SerializeField] private Transform warpTarget;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float cameraMoveDuration = 1f;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera investigateVCam;
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
   



    private void Start()
    {
        patientData = GetComponent<PatientData>();

        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();


        if (patientData == null)
            Debug.LogError("????? PatientData ?? GameObject ???");

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
                        patientMovement = movement; // ? ??????????????????? interact ???? ?
                        patientMovement.SetIsInteract(true);
                        uiManager.SelectPatient(patientMovement);
                    }

                    patientData = data;
                    uiManager.ShowPatientInfo(data);
                    uiManager.ShowPatientOption();

                    // ? ????? Interact ???????????????????????
                    Interact(data);

                    return;
                }
            }
        }
    }

    // ? ??????????? Interact ???? ??????????????????????????????????
    public void Interact(PatientData data)
    {
        if (data == null)
        {
            Debug.LogWarning("PatientData is null");
            return;
        }

        bool alreadyCreated = false;

        // ?? ??????????????????????????????????????
        foreach (Transform child in imageContainer)
        {
            TextMeshProUGUI existingText = child.GetComponentInChildren<TextMeshProUGUI>();
            if (existingText != null && existingText.text == data.patientName) {
                alreadyCreated = true;
                break;
            }
                
        }

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

       
        // ????????

        

        Debug.Log("Interacting with patient: " + data.patientName);
        PatientMovement movement = data.GetComponent<PatientMovement>();
        if (movement != null)
            movement.SetIsInteract(true);

        Room currentRoom = movement?.GetCurrentRoom();
        if (currentRoom == null)
        {
            Debug.LogWarning("currentRoom is null");
            return;
        }

        Debug.Log("Current Room: " + currentRoom.GetType().Name); // <== ????????? class ???????? ?

        // ? ?????????????????? index
        if (currentRoom is WaitingRoom)
        {

        }
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
                    investigateVCam.Priority = 20; // ????? priority ???????????????????
                }




                DiseaseData diseaseData = data.GetDiseaseData(); // ?????? method ????? PatientData
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
        else if (currentRoom is BloodTestRoom)
        {

        }
        else if (currentRoom is XrayRoom) 
        { 
            
        }
    }
}

    


