using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PatientEndurance : MonoBehaviour
{
    public Image EnduranceProgressBar;
    public UnityEvent OnEnduranceProgressStart;
    public UnityEvent OnEnduranceProgressOut;
    public float startEnduranceProgress;
    public float EnduranceProgressCount;
    public float EnduranceProgressHealing;
    public bool isStartEnduranceProgress = false;
    public Canvas EnduranceProgressUI;
    public PatientMovement selectedPatient;
    private InventoryManager inventoryManager;
    private PlayerController playerController;
    public bool hasDeath = false;
    
    public UIManager manager;

    public void Start()
    {
        selectedPatient = GetComponent<PatientMovement>();
        StartEnduranceProgress();
        EnduranceProgressUI.gameObject.SetActive(false);
    }

    public void StartEnduranceProgress()
    {
        OnEnduranceProgressStart.Invoke();

        isStartEnduranceProgress = true;
        EnduranceProgressCount = startEnduranceProgress;
    }

    private void FixedUpdate()
    {
        if (EnduranceProgressCount > 0 && isStartEnduranceProgress)
        {
            EnduranceProgressUI.gameObject.SetActive(true);
            EnduranceProgressCount -= Time.deltaTime;
            EnduranceProgressBar.fillAmount = EnduranceProgressCount * (1 / startEnduranceProgress);

            if (EnduranceProgressCount <= 0)
            {
                OnEnduranceProgressOut.Invoke();
                EnduranceProgressUI.gameObject.SetActive(false);
                if (hasDeath)
                {
                    Debug.Log("died");
                    selectedPatient.SetIsInteract(true);
                    EnduranceProgressUI.gameObject.SetActive(true);


                }
            }
           
        }
    }

    public void ProgressStart()
    {
        Debug.Log("Starting");
    }

    public void ProgressEnd()
    {
        Debug.Log("Died");
        selectedPatient.SetIsInteract(true);
        selectedPatient.PatienthasDied();


    }

    public void HalfHealing() {
        if (selectedPatient == null)
        {
            Debug.LogWarning("selectedPatient is NULL");
            return;
        }

        Debug.Log("HalfHealing Called");
        selectedPatient.SetIsInteract(false);
        selectedPatient.StartCoroutine(selectedPatient.MoveRandomly());
        EnduranceProgressCount = EnduranceProgressHealing;
        EnduranceProgressUI.gameObject.SetActive(true);
        isStartEnduranceProgress = true; // ? ???????????????
        selectedPatient.patientHasDied = false;
        hasDeath = true;

       
    }
}
