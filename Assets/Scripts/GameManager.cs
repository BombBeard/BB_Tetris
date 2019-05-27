using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameManager instance = null;

    enum GameState { MAINMENU, ACTIVEGAME, PAUSED };

    #region Inspector Exposed Variables

    [SerializeField]
    GameState gameState = GameState.ACTIVEGAME;

    [SerializeField]
    PlayField currentPlayfield;

    #region Player Control Adjustments

    //Hold delay intended to set the length of time a player
    //may input before scrolling behavior begins.
    [SerializeField]
    [Range(0f, 1f)]
    float horizontalScrollDelay = .2f;

    [SerializeField]
    [Range(0f, .3f)]
    float tapDelay = .02f;

    public int scrollRate = 20;

    #endregion

    [SerializeField]
    int levelNum;

    //todo Select which Block is actively controlled by the player
    [SerializeField]
    Block activeBlock;

    [SerializeField]
    bool blockFalling = false;

    #endregion

    #region Helper Variables

    Block heldBlock;

    float verticalInput = 0f;
    float horizontalInput = 0f;
    //If horizontalInput is the same as prevHorizontalInput, and scrollInputTimer
    //is >= horizontalScrollDelay, scrolling behavior is enacted.
    float prevHorizontalInput;


    #region Block Falling Helper Properties

    bool isDropping = false; //Updated in GameplayInput
    public bool canFall = true;
    //How fast the blocks are dropping without player input
    public float playSpeed = 2f; //Tiles / second
    public float fallSpeedOffset = 0f; //Meant to provide functionality for future
    float finalFallSpeed = 0f;
    float fallTimer = .001f;
    float numRowsToFall = 0f;

    #endregion

    float scrollInputTimer = .001f;
    bool isScrolling = false;
    float columnsScrolled = 0f; //Number of columns traversed per second

    #endregion




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
        /* If the player is pressing down, there is a fixed
         * scroll rate. By wrapping this in its own condition
         * we can eliminate that variable from our calculation.
         * 
         * That means for normal operation, final fallingRate
         * is simply a scroll operation. However, embed a
         * stubbed out offset for unique brick effects.
         */

        fallTimer += Time.deltaTime;

        if (gameState == GameState.ACTIVEGAME)
        {
            GameplayInput();

            /* Blocks don't always fall.
             * We need to check whether it's possible to fall, at all.
             * A piece in limbo may rotate, move horizontally, but not fall.
             * SoooooooooOoOooOOoooOO... Check if it's in limbo
             */
            #region Falling Conditionals

            UpdateCanFall();
            if (canFall && isDropping)
            {
                //Scroll Speed Drop
            }
            else if(canFall)
            {
                //Fall normal speed
                UpdateFallSpeed();
                if( (numRowsToFall = finalFallSpeed * fallTimer ) >= 1f)
                {
                    activeBlock.transform.localPosition += new Vector3(0f, -Mathf.Floor(numRowsToFall), 0f);
                    fallTimer = .001f;
                }
            }
            #endregion
        }
        else if (gameState == GameState.MAINMENU) { }
        else if (gameState == GameState.PAUSED) { }

        
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
        if(horizontalInput > 0f)
        {
            //how long the player has held the horizontal input
            scrollInputTimer += Time.deltaTime;
            float numOfColsToScroll = 0f;

            if (prevHorizontalInput > 0f &&
                scrollInputTimer >= horizontalScrollDelay)
            {
                //SCROLL BABY!
                isScrolling = true;

                /* Multiply the scrollRate by how long the player has 
                 * held the input, offsetting by the initial scrollDelay.
                 * This gives how many columns should have been scrolled by now.
                 * Subtracting this from how many HAVE been traversed will show
                 * whether we ought to move or not.
                 */
                numOfColsToScroll = (scrollRate * (scrollInputTimer - horizontalScrollDelay)) + 1;

            }
            else if(isScrolling == true)
            {
                isScrolling = false;
                columnsScrolled = 0f;
            }

            //If we haven't reached the bounds of movement yet...
            if (!currentPlayfield.rightCollider.GetCollisionState())
            {
                //...and scrolling is detected, and it's been long enough to scroll... move 1 column
                if (isScrolling && numOfColsToScroll - columnsScrolled >= 1.0f)
                {
                    activeBlock.transform.localPosition += new Vector3(1f, 0f, 0f);
                    columnsScrolled++;
                }
                //...and a valid discrete movement is detected... move 1 column
                else if(!isScrolling && scrollInputTimer <= tapDelay)
                {
                    activeBlock.transform.localPosition += new Vector3(1f, 0f, 0f);
                    columnsScrolled = 0f;
                }
            }
            prevHorizontalInput = horizontalInput;
        }
        else if(horizontalInput < 0f )
        {
            /*
            scrollInputTimer += Time.deltaTime;

            //Shift Block left
            if (!currentPlayfield.leftCollider.GetCollisionState())
            {
                //move left one place
                activeBlock.transform.localPosition += new Vector3(-1f, 0f, 0f);
            }
            prevHorizontalInput = horizontalInput;
            */

            //how long the player has held the horizontal input
            scrollInputTimer += Time.deltaTime;
            float numOfColsToScroll = 0f;

            if (prevHorizontalInput < 0f &&
                scrollInputTimer >= horizontalScrollDelay)
            {
                //SCROLL BABY!
                isScrolling = true;

                /* Multiply the scrollRate by how long the player has 
                 * held the input, offsetting by the initial scrollDelay.
                 * This gives how many columns should have been scrolled by now.
                 * Subtracting this from how many HAVE been traversed will show
                 * whether we ought to move or not.
                 */
                numOfColsToScroll = (scrollRate * (scrollInputTimer - horizontalScrollDelay)) + 1;
                //+1 on the end for player feel. Makes it so that as soon as the game decides
                //scrolling is being requested it begins moving.
            }
            else if (isScrolling == true)
            {
                isScrolling = false;
                columnsScrolled = 0f;
            }

            //If we haven't reached the bounds of movement yet...
            if (!currentPlayfield.leftCollider.GetCollisionState())
            {
                //...and scrolling is detected, and it's been long enough to scroll... move 1 column
                if (isScrolling && numOfColsToScroll - columnsScrolled >= 1.0f)
                {
                    activeBlock.transform.localPosition += new Vector3(-1f, 0f, 0f);
                    columnsScrolled++;
                }
                //...and a valid discrete movement is detected... move 1 column
                else if (!isScrolling && scrollInputTimer <= tapDelay)
                {
                    activeBlock.transform.localPosition += new Vector3(-1f, 0f, 0f);
                    columnsScrolled = 0f;
                }
            }
            prevHorizontalInput = horizontalInput;

        }
        else
        {
            //No horizontal input detected-- reset everything
            isScrolling = false;
            scrollInputTimer = 0f;
            prevHorizontalInput = 0f;
        }

        //Drop Input -- Spacebar
        if (wantDrop > 0)
        {
            //Drop active Block

        }
    }

    #endregion

    #region Helper Functions

    void UpdateFallSpeed()
    {
        /* Meant to be called when the speed is altered elsewhere.
         * This allows for block falling speed to be altered a bit
         * more simply.
         * 
         * This function assumes that everything it will check is already updated
         * and ready for calculations. 
         */

        finalFallSpeed = playSpeed + fallSpeedOffset;
    }

    void UpdateCanFall()
    {
        RaycastHit hit;
        int layermask = 1 << 10;
        Vector3 origin;

        for (int i = 0; i < activeBlock.scoutBricks.Count; i++)
        {
            origin = (activeBlock.scoutBricks[i].transform.position + new Vector3(0f, -.49f, 0f));
            if (activeBlock.isAtFloor)
            {
                canFall = false;

            }
            else if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, layermask))
            {
                if (hit.collider.gameObject != activeBlock.gameObject)
                {
                    print("Distance: " + (Mathf.Abs(Vector3.Distance(hit.collider.transform.position, activeBlock.scoutBricks[i].transform.position))));
                    //Debug.Log(hit.collider.GetComponentInParent<Block>().name);
                    if (Mathf.Abs(Vector3.Distance(hit.collider.transform.position, activeBlock.scoutBricks[i].transform.position)) <= 1.05f)
                    {
                        //Is adjacent to a foreign brick.
                        //Enter Limbo!
                        canFall = false;
                    }
                    else
                    {
                        canFall = true;
                    }
                }
            }
            else
            {
                canFall = true;
            }
        }
        //Scouts recon!@
    }

    void RotateActiveBlock()
    {
        activeBlock.UpdateScoutBlocks();
        //todo rotate the block
    }

    //todo implement one of these methods for detecting a tetris
    //Option 1
    //Pass in a line number to detect if a tetris is present
    bool IsLineATetris( int lineNum)
    {
        return false;
    }

    //Option 2
    //Pass a single block which queries its neighbors horizontally
    //If returned List<Block> length is 10, then return true.
    bool IsLineATetris( Block startingBlock)
    {
        return false;
    }

    #endregion
}
