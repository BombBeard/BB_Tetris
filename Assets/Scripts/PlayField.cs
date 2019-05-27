using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    public enum BoundaryDirection { left, right, bottom, top};


    public GameObject prefab;
    public GameObject origin;

    //Each gameObject needs to report collision independently.
    [SerializeField]
    public PlayAreaBoundary leftCollider;
    [SerializeField]
    public PlayAreaBoundary rightCollider;
    [SerializeField]
    public PlayAreaBoundary bottomCollider;
    [SerializeField]
    public PlayAreaBoundary topCollider;

    // Start is called before the first frame update
    void Start()
    {
        #region generate wall of blocks for testing
        /*
        if (prefab != null)
        {
            for(int y = 0; y > -20; y--)
            {
                for(int x = 0; x < 10; x++)
                {
                    GameObject tempBrick = Instantiate(prefab, transform, false);
                    tempBrick.transform.localPosition = new Vector3(x, y, -1);
                    if (x % 2 == 0) print("isEven");
                    if (x % 2 == 0)
                    {
                        tempBrick.GetComponent<Renderer>().material.shader = Shader.Find("_Color");
                        tempBrick.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    }
                }
            }

        }
        */
        #endregion


    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeLineColliders(int numRows)
    {

    }

}
