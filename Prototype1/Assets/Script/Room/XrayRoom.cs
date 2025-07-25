using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XrayRoom : Room
{
    private PatientData currentPatient;
    public Sprite XrayTested;
    public override void EnterRoom(PatientData data)
    {
        base.EnterRoom(data);
        currentPatient = data;

        if (data.disease != null)
        {
            data.UpdateDiseaseIcon();   
            data.hasXrayTested = true;
        }
    }
}
