using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //todo singleton pattern
    GameManager instance = null;

    [SerializeField]
    PlayField currentPlayfield;

    //General game stuff
    float verticalInput = 0f;
    float horizontalInput = 0f;
    bool isDropping = false;

    public float playSpeed;
    [SerializeField]
    [Range(0f, 1f)]
    float holdDelay = .01f;
    int levelNum;


    //todo Select which Block is actively controlled by the player
    [SerializeField]
    Block activeBlock;
    Block heldBlock;

    //todo Detect a tetris, delete the line(s), drop the bricks.
    

    // Start is called before the first frame update
    void Start()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion


    }

    // Update is called once per frame
    void Update()
    {
        //during active gameplay
        if(true) //todo implement game contexts for player control
            GameplayInput();
        
        //menu navigation...

    }

    #region Gameplay Input Controls

    void GameplayInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        float wantDrop = Input.GetAxis("Drop");

        //Vertical Inputs -- Up: rotate, Down: quick descent
        if(verticalInput > 0)
        {
            //Rotate piece clockwise


        }
        else if(verticalInput < 0)
        {
            //Double rate of descent

        }

        //Horizontal Inputs
        if(horizontalInput > 0)
        {
            //Shift Block right
            if (!currentPlayfield.rightCollider.GetCollisionState())
            {
                //move right one place
                activeBlock.transform.localPosition += new Vector3(1f, 0f, 0f);
            }
        }
        else if(horizontalInput < 0 )
        {
            //Shift Block left
            //if(activeBlock.trasform.position == 
            //is at least 1 unit away from right boundary){
            //Active block position + 1 }
            if (!currentPlayfield.leftCollider.GetCollisionState())
            {
                //move right one place
                activeBlock.transform.localPosition += new Vector3(-1f, 0f, 0f);
            }
        }

        //Drop Input -- Spacebar
        if (wantDrop > 0)
        {
            //Drop active Block
        }
    }

    #endregion
}
