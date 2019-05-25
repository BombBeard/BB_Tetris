using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    //Properties
    Vector2 shapePosition;  //The position of the brick in the shape it belongs to
    Color color;
    Brick[] neighbors = new Brick[8];
    /* Array compass of neighbors:
     * 0 1 2  
     * 3   4
     * 5 6 7 
    */

    // Start is called before the first frame update
    void Start()
    {
        //query all directions for neighbors
    }

    // Update is called once per frame
    void Update()
    {
        FallCheck(); //todo listen for delegate instead of frame checking
    }

    void FallCheck()
    {//todo decide if falling is necessary
        //if all horizontal neighbors have no -y neighbors, fall.
        
    }

    void ActivateSpecial()
    {

    }

    void Destroy()
    {

    }

}
