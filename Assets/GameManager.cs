using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //todo singleton pattern
    GameManager instance = null;

    //General game stuff
    public float playSpeed;
    int levelNum;


    //todo Select which Block is actively controlled by the player
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
        PlayerInput();
        
    }

    void PlayerInput()
    {
        //todo Detect player input, and determine action to take
    }
}
