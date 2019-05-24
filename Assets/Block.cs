using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BlockShape { I, J, L, O, S, T, Z };

    public BlockShape shape;
    BlockShape oldShape;
    public Brick brickPrototype;
    Brick[] bricks = new Brick[4];

    private void Awake()
    {
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
}
