using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoom : Room
{
    PatientData currentPatient;
    private int questionCount = 0;

    public override void EnterRoom(PatientData patient)
    {
        base.EnterRoom(patient);
        currentPatient = patient;
        questionCount = 0;

        //Debug.Log("WaitingRoom Patient: " + patient.patientName);

    }

   
}
