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
    [SerializeField] Brick[] bricks;
    public List<Brick> scoutBricks = new List<Brick>();
    bool bricksAreNeighbors = false;
    public bool isAtFloor = false;


    private void Awake()
    {
        bricks = new Brick[numBricks];
        //todo instantiate blocks
        for (int i = 0; i < 4; i++)
        {
            Brick tmpBrick = Instantiate(brickPrototype, transform, false);
            bricks[i] = tmpBrick;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        oldShape = shape;
        SetBlockShape(shape);
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
            UpdateScoutBlocks();
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
            bricks[0].transform.localPosition = new Vector3(0, 2.5f, 0);
            bricks[1].transform.localPosition = new Vector3(0, 1.5f, 0);
            bricks[2].transform.localPosition = new Vector3(0, .5f, 0);
            bricks[3].transform.localPosition = new Vector3(0, -.5f, 0);
        }
        if (shape == BlockShape.J)
        {
            bricks[0].transform.localPosition = new Vector3(0, 1.5f, 0);
            bricks[1].transform.localPosition = new Vector3(0, .5f, 0);
            bricks[2].transform.localPosition = new Vector3(0, -.5f, 0);
            bricks[3].transform.localPosition = new Vector3(-1f, -.5f, 0);
        }
    }

    void SetBrickColor( Color c)
    {

    }

    public void UpdateScoutBlocks()
    {
        int layermask = 1 << 10;
        RaycastHit hit;
        Vector3 origin;

        for (int i = 0; i < numBricks; i++)
        {
            origin = (bricks[i].transform.position + new Vector3(0f, -.49f, 0f));
            if (Physics.Raycast(origin, Vector3.down, out hit, .5f, layermask))
            {
                if (!hit.collider.gameObject == gameObject)
                {
                    print("hit a neighbor");
                    if(!scoutBricks.Contains(bricks[i]))
                        scoutBricks.Add(bricks[i]);
                }
                else
                {
                    if(scoutBricks.Contains(bricks[i]))
                        scoutBricks.Remove(bricks[i]);
                    //If weird shit is happening near non-brick colliders
                    //Assume you need to check what type of object you're colliding with.
                }
            }
            else
            {
                if (!scoutBricks.Contains(bricks[i]))
                    scoutBricks.Add(bricks[i]);
            }
        }


    }

}
