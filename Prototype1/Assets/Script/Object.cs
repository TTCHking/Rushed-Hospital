using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
   [SerializeField] private ObjectSO objectSO;

    private Phone phone;

    public ObjectSO GetObjectSO() { 
        return objectSO;
    }

    public void SetClearSpawnPoint(Phone phone) { 
        this.phone = phone;
        transform.parent = phone.GetObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public Phone GetClearSpawnPoint() { 
        return phone;
    }
}
