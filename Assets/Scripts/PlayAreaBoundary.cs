using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaBoundary : MonoBehaviour
{
    bool isCollided;

    private void Start()
    {
        isCollided = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Brick>() != null){
            //print("Collision Entered" + gameObject.name);
            isCollided = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Brick>() != null)
        {
            //print("Collision Exited");
            isCollided = false;
        }
    }
    public bool GetCollisionState() { return isCollided; }
}
