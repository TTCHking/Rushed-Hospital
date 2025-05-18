using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;
using UnityEngine.UI;

public class Phone : MonoBehaviour, IInteractable
{
    [SerializeField] private ObjectSO orange;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ObjectSO square;
    [SerializeField] private Phone secondphone;
    [SerializeField] private bool testing;

    private Object @object;

    public Canvas canvas;
    public int Token;
    public TextMeshProUGUI Token_Text;

    
    public void Interaction(GameObject interactor)
    {
        Debug.Log("Interact!");
        Show();
        
    }

    private void Start()
    {
        canvas.gameObject.SetActive(false);
        Token = 2000;
        Token_Text.text = Token.ToString();
    }

    private void Show() 
    { 
        canvas.gameObject.SetActive(true);
    }

    public void HideSpawnCanvas() 
    {
        canvas.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.G)) {
            if (@object != null) {
                @object.SetClearSpawnPoint(secondphone);
            }
        }
    }

    public Transform GetObjectFollowTransform() { 
        return spawnPoint;
    }

    public void BuyNourP()
    {
        if (Token >= 350)
        {
            Token -= 350;
            Token_Text.text = Token.ToString();
            SpawnNourP();
        }
        else {
            print("not enough Token");
        }
    }

    public void SpawnNourP()
    {
        if (@object == null)
        {
            Transform prefabTransform = Instantiate(orange.Prefab, spawnPoint);
            prefabTransform.localPosition = Vector3.zero;
        }
    }

    public void BuyNourT()
    {
        if (Token >= 200)
        {
            Token -= 200;
            Token_Text.text = Token.ToString();
            SpawnNourT();
        }
        else
        {
            print("not enough Token");
        }
    }

    public void SpawnNourT()
    {
        if (@object == null)
        {
            Transform prefabTransform = Instantiate(square.Prefab, spawnPoint);
            prefabTransform.localPosition = Vector3.zero;

        }
    }





}