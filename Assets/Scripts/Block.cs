using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BlockShape { I, J, L, O, S, T, Z, BUCKET};

    public BlockShape shape;
    public int numBricks = 4;
    BlockShape oldShape;
    public Brick brickPrototype;
    public Brick[] bricks;
    public List<Brick> scoutBricks = new List<Brick>(); //bottom facing bricks
    public List<Brick> leftBumperBricks = new List<Brick>(); //left outward facing bricks
    public List<Brick> rightBumperBricks = new List<Brick>(); //right outward facing bricks
    public Brick pivotBrick;
    public Vector3 pivotPoint = new Vector3(0f,0f,0f);
    //public GameObject pivotParent;
    bool bricksAreNeighbors = false;
    [HideInInspector]
    public bool isAtFloor = false;



    private void Awake()
    {
        bricks = new Brick[numBricks];
        //todo instantiate blocks
        for (int i = 0; i < numBricks; i++)
        {
            Brick tmpBrick = Instantiate(brickPrototype, transform, false);
            bricks[i] = tmpBrick;
        }
        //pivotParent = new GameObject();
    }

    // Start is called before the first frame update
    void Start()
    {
        oldShape = shape;
        SetBlockShape(shape);
        //pivotParent.transform.parent = transform.parent;
        //transform.parent = pivotParent.transform;
        
    }
    // Update is called once per frame
    void Update()
    {

        if (shape != oldShape)
        {
            SetBlockShape(shape);
            oldShape = shape;
        }
        if (!bricksAreNeighbors)
        {
            UpdateScoutAndBumperBlocks();
            bricksAreNeighbors = true;
            for (int i = 0; i < 4; i++)
            {
                bricks[i].DiscoverNeighbors();
            }
        }
    }

    void SetBlockShape( BlockShape shape)
    {
        if (shape == BlockShape.I)
        {
            /* XOXX
             */
            bricks[0].transform.localPosition = new Vector3(2f, 0f, 0);
            bricks[1].transform.localPosition = new Vector3(1f, 0f, 0);
            pivotBrick = bricks[1];
            bricks[2].transform.localPosition = new Vector3(0f, 0f, 0);
            bricks[3].transform.localPosition = new Vector3(-1f, 0f, 0);
        }
        if (shape == BlockShape.J)
        {
            /* X
             * XOX
             */
            bricks[0].transform.localPosition = new Vector3(-1f, 1f, 0);
            bricks[1].transform.localPosition = new Vector3(-1f, 0f, 0);
            bricks[2].transform.localPosition = new Vector3(0, 0f, 0);
            bricks[3].transform.localPosition = new Vector3(1f, 0f, 0);
        }
        if (shape == BlockShape.L)
        {
            /*   X
             * XOX
             */
            bricks[0].transform.localPosition = new Vector3(0, 1.5f, 0);
            bricks[1].transform.localPosition = new Vector3(0, .5f, 0);
            bricks[2].transform.localPosition = new Vector3(0, -.5f, 0);
            bricks[3].transform.localPosition = new Vector3(-1f, -.5f, 0);
        }
        if (shape == BlockShape.O)
        {//Pivot point, O, is not associated with a brick, but merely in the center.
            /* XX
             * XX
             */
            bricks[0].transform.localPosition = new Vector3(-1f, 1f, 0);
            bricks[1].transform.localPosition = new Vector3(-1f, 0f, 0);
            bricks[2].transform.localPosition = new Vector3(0f, 1f, 0);
            bricks[3].transform.localPosition = new Vector3(0f, 0f, 0);
        }
        if (shape == BlockShape.S)
        {
            /*  X
             * XO
             * X
             */
            bricks[0].transform.localPosition = new Vector3(0, 1.5f, 0);
            bricks[1].transform.localPosition = new Vector3(0, .5f, 0);
            bricks[2].transform.localPosition = new Vector3(0, -.5f, 0);
            bricks[3].transform.localPosition = new Vector3(-1f, -.5f, 0);
        }
        if (shape == BlockShape.Z)
        {
            /* X
             * OX
             *  X
             */
            bricks[0].transform.localPosition = new Vector3(0, 1.5f, 0);
            bricks[1].transform.localPosition = new Vector3(0, .5f, 0);
            bricks[2].transform.localPosition = new Vector3(0, -.5f, 0);
            bricks[3].transform.localPosition = new Vector3(-1f, -.5f, 0);
        }
        if (shape == BlockShape.T)
        {
            /*  X
             * XOX
             */
            bricks[0].transform.localPosition = new Vector3(0, 1.5f, 0);
            bricks[1].transform.localPosition = new Vector3(0, .5f, 0);
            bricks[2].transform.localPosition = new Vector3(0, -.5f, 0);
            bricks[3].transform.localPosition = new Vector3(-1f, -.5f, 0);
        }
    }

    void SetBrickColor( Color c)
    {

    }


    /* Optimization possibility:
     * If rotation becomes too intensive due to updating which bricks 
     * are scouts, consider hard-coding which bricks are scouts 
     * in which orientation. This is annoying with blocks which contain
     * more than 4 bricks, but would reduce the number instructions dramatically.
     */
    public void UpdateScoutAndBumperBlocks()
    {
        int layermask = 1 << 10;
        RaycastHit hit;
        Vector3 origin;

        for (int i = 0; i < numBricks; i++)
        {
            origin = (bricks[i].transform.position + new Vector3(0f, -.49f, 0f));
            //Check for bottom-facing scouts
            Debug.DrawRay(origin, Vector3.down * .5f, Color.red, 1f, false);

            if (Physics.Raycast(origin, Vector3.down, out hit, .5f, layermask))
            {
                if (!hit.collider.gameObject == gameObject)
                {
                    if(!scoutBricks.Contains(bricks[i]))
                        scoutBricks.Add(bricks[i]);
                }
                else
                {
                    if (scoutBricks.Contains(bricks[i]))
                    {
                        scoutBricks.Remove(bricks[i]);
                        print("removing");
                    }
                    //If weird shit is happening near non-brick colliders
                    //Assume you need to check what type of object you're colliding with.
                }
            }
            else
            {
                if (!scoutBricks.Contains(bricks[i]))
                    scoutBricks.Add(bricks[i]);
            }
            //Check for left-facing bumper bricks
            origin = (bricks[i].transform.position + new Vector3(-.49f, 0f, 0f));
            if (Physics.Raycast(origin, Vector3.left, out hit, .5f, layermask))
            {
                if (!hit.collider.gameObject == gameObject)
                {
                    if(!leftBumperBricks.Contains(bricks[i]))
                        leftBumperBricks.Add(bricks[i]);
                }
                else
                {
                    if(leftBumperBricks.Contains(bricks[i]))
                        leftBumperBricks.Remove(bricks[i]);
                    //If weird shit is happening near non-brick colliders
                    //Assume you need to check what type of object you're colliding with.
                }
            }
            else
            {
                if (!leftBumperBricks.Contains(bricks[i]))
                    leftBumperBricks.Add(bricks[i]);
            }
            //Check for right-facing bumper bricks
            origin = (bricks[i].transform.position + new Vector3(.49f, 0f, 0f));
            if (Physics.Raycast(origin, Vector3.right, out hit, .5f, layermask))
            {
                if (!hit.collider.gameObject == gameObject)
                {
                    if(!rightBumperBricks.Contains(bricks[i]))
                        rightBumperBricks.Add(bricks[i]);
                }
                else
                {
                    if(rightBumperBricks.Contains(bricks[i]))
                        rightBumperBricks.Remove(bricks[i]);
                    //If weird shit is happening near non-brick colliders
                    //Assume you need to check what type of object you're colliding with.
                }
            }
            else
            {
                if (!rightBumperBricks.Contains(bricks[i]))
                    rightBumperBricks.Add(bricks[i]);
            }

        }
    }//End UpdateScoutAndBumperBlcoks()

}
