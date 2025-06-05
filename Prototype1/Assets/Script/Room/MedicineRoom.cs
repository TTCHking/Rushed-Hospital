using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineRoom : Room
{
    private PatientData currentPatient;
    public override void EnterRoom(PatientData data)
    {
        base.EnterRoom(data);
        currentPatient = data;


    }
}