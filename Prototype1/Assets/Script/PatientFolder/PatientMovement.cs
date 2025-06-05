using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatientMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isEmergency;

    private bool isInteracted = false;
   

    private Transform targetRoomTransform;
    private Room currentRoom;
    private Vector3 targetPosition;
    private NavMeshAgent agent;
    private PatientData patient;
    [SerializeField] private List<string> medicineOrderList;
    [SerializeField] private float maxPatience;

    private int questionCount;

    public bool patientHasDied;

    private void Start()
    {
        patient = GetComponent<PatientData>();

        if (patient == null)
        {
            Debug.LogError("PatientData component ไม่พบใน GameObject นี้");
        }
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        Debug.Log("Start patient: " + patient.patientName);

        //ตรวจ tag เเละเดินไปหาห้อง WaitingRoom เป็นห้องเเรก
        GameObject waitingRoomObj = GameObject.FindWithTag("WaitingRoom");
        if (waitingRoomObj != null)
        {
            targetRoomTransform = waitingRoomObj.transform;

            // เพิ่มตรงนี้เพื่อเซต currentRoom เป็น WaitingRoom ทันที
            Room waitingRoom = waitingRoomObj.GetComponent<Room>();
            if (waitingRoom != null)
            {
                SetCurrentRoom(waitingRoom);  // เซต currentRoom เป็น WaitingRoom
                Debug.Log("currentRoom ถูกตั้งเป็น WaitingRoom ทันทีตอนเริ่ม");
            }

            MoveToTargetRoom(targetRoomTransform); // เดินไปหาห้อง WaitingRoom
            Debug.Log(patient.patientName + " Go to " + waitingRoomObj.name);
        }
    }

    
    private void Update()
    {
        // ลดความอดทนเรื่อย ๆ 
        float patienceDecayRate = isEmergency ? 2f : 1f;

        // ถ้า agent กำลังเดินไปยัง targetRoomTransform
        if (agent != null && targetRoomTransform != null)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0f)
                {
                    if (Vector3.Distance(transform.position, targetRoomTransform.position) < 1.5f)
                    {
                        Debug.Log("🟢 ผู้ป่วยถึงห้องแล้ว: " + targetRoomTransform.name);
                        ArriveAtRoom();
                    }
                }
            }
        }
        else // ถ้าไม่มีเป้าหมายเดินด้วย NavMeshAgent ให้เดินแบบสุ่มด้วย targetPosition
        {
            if (!isInteracted)
            {
                // เคลื่อนที่ไปยัง targetPosition ที่ตั้งใน MoveRandomly()
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }

    }

    // สั่งให้เดินสุ่มทุก ๆ 3 วิ
     public IEnumerator MoveRandomly()
    {
        Debug.Log("MoveRandomly Coroutine started");
        while (!isInteracted)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
            targetPosition = transform.position + randomOffset;

            yield return new WaitForSeconds(3f);
        }
    }

    //ถึงห้อง
    private void ArriveAtRoom()
    {
       
        
        currentRoom = targetRoomTransform.GetComponent<Room>();
        if (currentRoom != null)
        {
            Debug.Log("✅ Set currentRoom: " + currentRoom.GetType().Name);
            SetCurrentRoom(currentRoom); // เก็บค่าห้องปัจจุบันใน patient
            currentRoom.EnterRoom(patient); // เรียก logic ของห้องนั้น
        }

        targetRoomTransform = null;

       
    }

    // เรียกใช้จากปุ่มหรือ UI เพื่อให้ผู้ป่วยไปยังห้องเป้าหมาย
    public void MoveToTargetRoom(Transform roomTransform)
    {
        if (agent != null && roomTransform != null)
        {
            targetRoomTransform = roomTransform;
            agent.isStopped = false;
            agent.SetDestination(roomTransform.position);
            
            Debug.Log("🟡 ส่งผู้ป่วยไปยัง: " + roomTransform.name);
        }
    }

    //เมื่อInteractให้หยุดนิ่ง
    public void SetIsInteract(bool interact)
    {
        isInteracted = interact;
        if (agent != null)
        {
            agent.isStopped = interact;
        }
    }

  

    //ให้ทำให้ห้องนี้ของผู้ป่วยคนนี้ = ค่าห้องที่รับมา
    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }

    //รับค่าห้องมา
    public Room GetCurrentRoom()
    {
        return currentRoom;
        
    }

    //นับจำนวนการถามตอบผู้ป่วยคนนี้
    public int GetQuestionCount()
    {
        Debug.Log("Get questionCount : " + questionCount);
        return questionCount;
    }

    //ทำให้ค่าการนับคำถามปัจจุบันเท่ากับจำนวนคำถามที่ถามไป
    public void SetQuestionCount(int questionCount)
    {
        Debug.Log("Set questionCount : " + questionCount);
        this.questionCount = questionCount;
    }

    public void PatienthasDied() 
    {
        patientHasDied = true;
        
    }
}