using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

public class PatientSpawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] patientPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public int maxPatients = 10;
    [SerializeField] private NameDatabase nameDatabase;

    private List<string> tempList1;
    private List<string> tempList2;

    private List<string> availableMaleNames;
    private List<string> availableFemaleNames;

    private List<GameObject> currentPatients = new List<GameObject>();

    [SerializeField, Range(0f, 1f)]
    private float pregnancyChance = 0.2f;

    [SerializeField] private DiseaseCategoryDatabase diseaseCategoryDatabase;

    private void Start()
    {
        tempList1 = new List<string>(nameDatabase.patientMaleName);
        tempList2 = new List<string>(nameDatabase.patientFemaleName);
        StartCoroutine(SpawnPatientsRoutine());
    }


    private void Awake()
    {
        availableMaleNames = new List<string>(nameDatabase.patientMaleName);
        availableFemaleNames = new List<string>(nameDatabase.patientFemaleName);
    }

    IEnumerator SpawnPatientsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentPatients.Count < maxPatients)
            {
                SpawnRandomPatient();
            }
            currentPatients.RemoveAll(patient => patient == null);
        }
    }

    void SpawnRandomPatient()
    {
        if ((tempList1 == null || tempList1.Count == 0) && (tempList2 == null || tempList2.Count == 0))
        {
            Debug.LogWarning("ไม่มีชื่อ");
            return;
        }

        if (patientPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        // เพศ
        string gender = "";
        if (nameDatabase.patientGender != null && nameDatabase.patientGender.Count > 0)
        {
            int genderIndex = Random.Range(0, nameDatabase.patientGender.Count);
            gender = nameDatabase.patientGender[genderIndex];
        }

        // ชื่อ
        string patientName = "";
        bool validNameFound = false;

        if (gender == "Male" && tempList1.Count > 0)
        {
            int index = Random.Range(0, tempList1.Count);
            patientName = tempList1[index];
            tempList1.RemoveAt(index);
            validNameFound = true;
        }
        else if (gender == "Female" && tempList2.Count > 0)
        {
            int index = Random.Range(0, tempList2.Count);
            patientName = tempList2[index];
            tempList2.RemoveAt(index);
            validNameFound = true;
        }

        if (!validNameFound)
        {
            Debug.LogWarning("ไม่มีชื่อสำหรับเพศนี้แล้ว!");
            return;
        }

        // อายุ
        int randomAge = Random.Range(1, 95);
        nameDatabase.patientAge = randomAge;
        string ageGroup = "";
        if (randomAge <= 12)
            ageGroup = "เด็ก";
        else if (randomAge <= 59)
            ageGroup = "ผู้ใหญ่";
        else
            ageGroup = "ผู้สูงอายุ";

        // กาชาท้อง
        bool isPregnant = false;
        if (gender == "Female" && randomAge >= 15 && randomAge <= 58)
        {
            float roll = Random.value;
            isPregnant = roll < pregnancyChance;
        }

        // หมู่เลือด
        string bloodType = "";
        if (nameDatabase.patientBloodType != null && nameDatabase.patientBloodType.Count > 0)
        {
            int bloodTypeIndex = Random.Range(0, nameDatabase.patientBloodType.Count);
            bloodType = nameDatabase.patientBloodType[bloodTypeIndex];
        }


        // โรค
        DiseaseData disease = GetRandomDisease();
        string diseaseName = disease != null ? disease.diseaseName : "ไม่ระบุโรค";

        //spawn
        GameObject patientToSpawn = patientPrefabs[Random.Range(0, patientPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newPatient = Instantiate(patientToSpawn, spawnPoint.position, spawnPoint.rotation);
        currentPatients.Add(newPatient);

        PatientData data = newPatient.GetComponent<PatientData>();
        data.patientName = patientName;
        data.age = randomAge;
        data.ageGroup = ageGroup;
        data.gender = gender;
        data.isPregnant = isPregnant;
        data.bloodType = bloodType;
        data.disease = disease;
        data.UpdateBloodStatusTexts();

        Debug.Log($"ชื่อ: {patientName} | อายุ: {randomAge} ({ageGroup}) | เพศ: {gender} | โรค: {diseaseName} | ตั้งครรภ์: {isPregnant} | หมู่เลือด: {bloodType}");
    }

    DiseaseData GetRandomDisease()
    {
        if (diseaseCategoryDatabase == null)
        {
            Debug.LogWarning("ไม่มีข้อมูล");
            return null;
        }

        int randomType = Random.Range(0, 3);

        List<DiseaseData> diseaseList = null;

        switch (randomType)
        {
            case 0:
                if (diseaseCategoryDatabase.boneDiseaseDatabase != null)
                    diseaseList = diseaseCategoryDatabase.boneDiseaseDatabase.allBoneDiseases;
                break;
            case 1:
                if (diseaseCategoryDatabase.virusDiseaseDatabase != null)
                   diseaseList = diseaseCategoryDatabase.virusDiseaseDatabase.allVirusDiseases;
                break;
            case 2:
               if (diseaseCategoryDatabase.parasiteDiseaseDatabase != null)
                   diseaseList = diseaseCategoryDatabase.parasiteDiseaseDatabase.allParasiteDiseases;
                break;

        }

        if (diseaseList == null || diseaseList.Count == 0)
        {
            Debug.LogWarning("ไม่พบโรคในประเภทนี้");
            return null;
        }

        // สุ่มโรคในประเภทที่ได้
        int randomIndex = Random.Range(0, diseaseList.Count);
        return diseaseList[randomIndex];
    }

}