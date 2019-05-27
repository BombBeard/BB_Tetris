using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    [SerializeField] Brick[] neighbors = new Brick[4];
    /* Array compass of neighbors:
     *   0 
     * 1   2
     *   3
    */

    Color color;


    

    bool debugNeighbors = false;

    // Update is called once per frame
    void Update()
    {
    }

    void FallCheck()
    {//todo decide if falling is necessary
        //if all horizontal neighbors have no -y neighbors, fall.
        
    }

    //todo Implement special abilities
    void ActivateSpecial()
    {

    }

    public void DiscoverNeighbors()
    {
        //IEnumerator for concurrency?
        //Raycast in 4 directions, assign found bricks to corresponding
        //array indices, and register with new neighbors
        //Is called when active block settles,
        //when a brick is destroyed.

        #region Initialize Neighbors
        //set up raycast
        int layermask = 1 << 10;
        RaycastHit hit;

        //Debug.DrawRay(origin, Vector3.down * .5f, Color.red, 100, false);

        //Is it worth optimizing code to remove effectively duplicate calls?
        //raycast in cardinal directions

        //Up 
        Vector3 origin = (transform.position + new Vector3(0f, .49f, 0f));
        if (Physics.Raycast(origin, Vector3.up, out hit, .5f, layermask))
        {
            if(debugNeighbors) print("Up Discovered");
            neighbors[0] = hit.collider.GetComponent<Brick>();
            neighbors[0].SetNeighbor(3, this); //duplicate safety assignment
        }
        //Down
        origin = (transform.position + new Vector3(0f, -.49f, 0f));
        if (Physics.Raycast(origin, Vector3.down, out hit, .5f, layermask))
        {
            if (debugNeighbors) print("Down Discovered");
            neighbors[3] = hit.collider.GetComponent<Brick>();
            neighbors[3].SetNeighbor(0, this);
        }
        //Right
        origin = (transform.position + new Vector3(.49f, 0f, 0f));
        if (Physics.Raycast(origin, Vector3.right, out hit, .5f, layermask))
        {
            if (debugNeighbors) print("Right Discovered");
            neighbors[2] = hit.collider.GetComponent<Brick>();
            neighbors[2].SetNeighbor(1, this);
        }
        //Left
        origin = (transform.position + new Vector3(-.49f, 0f, 0f));
        if (Physics.Raycast(origin, Vector3.left, out hit, .5f, layermask))
        {
            if (debugNeighbors) print("Left Discovered");
            neighbors[1] = hit.collider.GetComponent<Brick>();
            neighbors[1].SetNeighbor(2, this);
        }
        #endregion

    }
    void SetNeighbor( int index, Brick brick)
    {//neighbor may be null
        /*   0
         * 1   2
         *   3
         */
        if (debugNeighbors) print("Setting Neighbor");
        neighbors[index] = brick;

    }

    void Destroy()
    {
        //remove references of self from neighbors by assigning null
    }

}
