using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] GameObject objectToDestroy;
    public void DestroyAssignedObject()
    {
        Destroy(objectToDestroy);
    }
}
