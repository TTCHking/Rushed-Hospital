using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showinventory : MonoBehaviour, IInteractable
{

    public Canvas inventorycanvas;
    public void Interaction(GameObject interactor)
    {
        inventorycanvas.gameObject.SetActive(true);
    }

    private void Start()
    {
        inventorycanvas.gameObject.SetActive(false);
    }

    public void ExitInventory() {
        inventorycanvas.gameObject.SetActive(false);
    }

}
