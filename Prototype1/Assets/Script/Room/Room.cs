using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public string roomName;

    public virtual void EnterRoom(PatientData patient)
    {
        Debug.Log(patient.patientName + " has entered " + roomName);
        // เพิ่ม Logic เฉพาะของห้อง เช่น ตรวจวัดความดัน หรือการตอบคำถาม
       
    }


}
