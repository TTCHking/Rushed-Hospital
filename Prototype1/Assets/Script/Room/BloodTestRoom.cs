using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BloodTestRoom : Room
{
    private PatientData currentPatient;

  
    

    public void Start()
    {
  
    }
    public override void EnterRoom(PatientData data)
    {
        base.EnterRoom(data);
        currentPatient = data;

        if (data.disease != null)
        {
            data.rbcStatusText = BloodStatusToString(data.disease.RBCStatus);
            data.wbcStatusText = BloodStatusToString(data.disease.WBCStatus);
            data.neutrophilStatusText = BloodStatusToString(data.disease.NeutrophilStatus);
            data.eosinophilStatusText = BloodStatusToString(data.disease.EosinophilStatus);
            data.basophilStatusText = BloodStatusToString(data.disease.BasophilStatus);
            data.lymphocyteStatusText = BloodStatusToString(data.disease.LymphocyteStatus);
            data.monocyteStatusText = BloodStatusToString(data.disease.MonocyteStatus);
            data.hasBloodTested = true;
        }
    }

    private string BloodStatusToString(BloodStatus status)
    {
        switch (status)
        {
            case BloodStatus.N: return "Normal";
            case BloodStatus.H: return "High";
            case BloodStatus.L: return "Low";
            default: return "Unknown";
        }
    }
}
