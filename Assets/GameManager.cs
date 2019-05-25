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
    //Hold delay intended to set the length of time a player
    //may input before scrolling behavior begins.
    [SerializeField]
    [Range(0f, 1f)]
    float horizontalScrollDelay = .01f;
    [SerializeField]
    [Range(0f, 1f)]
    float tapDelay = .02f;
    //If prevHI is the same as before and horScrollDelay has 
    //been reached, scrolling behavior is enacted.
    float prevHorizontalInput;
    float scrollInputTimer;
    bool isScrolling = false;
    //Number of columns traversed per second
    public int scrollRate = 7;
    float columnsScrolled = 0f;

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
            print("Scroll Input Timer" + scrollInputTimer);
            print("Scroll Delay" + horizontalScrollDelay);

        }

        //Drop Input -- Spacebar
        if (wantDrop > 0)
        {
            //Drop active Block

        }
    }

    #endregion
}
