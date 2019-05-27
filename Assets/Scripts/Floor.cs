using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    //Class purely for distinguishing this collider from 
    //other colliders.

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Brick>() != null)
        {
            other.GetComponentInParent<Block>().isAtFloor = true;
        }
    }
}
