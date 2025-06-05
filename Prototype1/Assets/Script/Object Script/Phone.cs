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

    public UIManager manager;
   

    
    public void Interaction(GameObject interactor)
    {
        Debug.Log("Interact!");
        Show();
        
    }

    private void Start()
    {
        canvas.gameObject.SetActive(false);
       
    }

    private void Show() 
    { 
        canvas.gameObject.SetActive(true);
    }

    public void HideSpawnCanvas() 
    {
        canvas.gameObject.SetActive(false);
    }


    public Transform GetObjectFollowTransform() { 
        return spawnPoint;
    }

    public void BuyNourP()
    {
        if (manager.Token >= 350)
        {
            manager.Token -= 350;
            manager.Token_Text.text = manager.Token.ToString();
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
        if (manager.Token >= 200)
        {   
            manager.Token -= 200;
            manager.Token_Text.text = manager.Token.ToString();
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