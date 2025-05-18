using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatientMovement : MonoBehaviour
{
    public float moveSpeed = 20f;
    public bool isEmergency;

    private bool isInteracted = false;
    private bool isMoving = false;

    private Transform targetRoomTransform;
    private Room currentRoom;
    private Vector3 targetPosition;
    private NavMeshAgent agent;
    private PatientData patient;
    [SerializeField] private List<string> medicineOrderList;
    [SerializeField] private float maxPatience;

    private int questionCount;
    private bool isDiagnosising = false;
    private bool patientHasStoredQuestion = false;
    private List<string> patientQuestions;

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
        StartCoroutine(MoveRandomly()); // เดินสุ่มตอนเริ่ม
        
    }

    private void Update()
    {
        // ลดความอดทนเรื่อย ๆ (เลือกใช้ถ้าจำเป็น)
        float patienceDecayRate = isEmergency ? 2f : 1f;

        // ถ้ายังไม่ถูก Interact เดินสุ่มในห้อง
        if (!isInteracted)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        // เช็คว่าเดินถึงเป้าหมายหรือยัง
        if (agent != null && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0f)
            {
                if (targetRoomTransform != null &&
                    Vector3.Distance(transform.position, targetRoomTransform.position) < 1f)
                {
                    Debug.Log("🟢 ผู้ป่วยถึงห้องแล้ว: " + targetRoomTransform.name);
                    ArriveAtRoom();
                }
            }
        }
    }

    IEnumerator MoveRandomly()
    {
        while (!isInteracted)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
            targetPosition = transform.position + randomOffset;

            yield return new WaitForSeconds(3f);
        }
    }

    private void ArriveAtRoom()
    {
        isMoving = false;
        
        currentRoom = targetRoomTransform.GetComponent<Room>();
        if (currentRoom != null)
        {
            Debug.Log("✅ Set currentRoom: " + currentRoom.GetType().Name);
            SetCurrentRoom(currentRoom); // เก็บค่าห้องปัจจุบันใน patient
            currentRoom.EnterRoom(patient); // เรียก logic ของห้องนั้น
        }

        targetRoomTransform = null;

        if (!isInteracted)
        {
            StartCoroutine(MoveRandomly());
        }
    }

    // เรียกใช้จากปุ่มหรือ UI เพื่อให้ผู้ป่วยไปยังห้องเป้าหมาย
    public void MoveToTargetRoom(Transform roomTransform)
    {
        if (agent != null && roomTransform != null)
        {
            targetRoomTransform = roomTransform;
            agent.isStopped = false;
            agent.SetDestination(roomTransform.position);
            isMoving = true;
            Debug.Log("🟡 ส่งผู้ป่วยไปยัง: " + roomTransform.name);
        }
    }

    public void SetIsInteract(bool interact)
    {
        isInteracted = interact;
        if (agent != null)
        {
            agent.isStopped = interact;
        }
    }

    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
        
    }

    public int GetQuestionCount()
    {
        Debug.Log("Get questionCount : " + questionCount);
        return questionCount;
    }

    public void SetQuestionCount(int questionCount)
    {
        Debug.Log("Set questionCount : " + questionCount);
        this.questionCount = questionCount;
    }
}