using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Player : Actor
{

    //todo:  will take it out this, to be a "Player Class" 
    #region Player Property

    public int curBomb;
    public int maxBomb;

    public float speed = 0.5f;

    public bool isBombStrong;

    public int bombLength = 5;

    #endregion

    public GameObject bombObj;
    public GameObject bombStrongObj;

    public GameObject Map;

    public int player_x;
    public int player_y;

    public BlockState curValue;
    public BlockState upValue;
    public BlockState downValue;
    public BlockState leftValue;
    public BlockState rightValue;
    public BlockState upLeftValue;
    public BlockState upRightValue;
    public BlockState downLeftValue;
    public BlockState downRightValue;
    
    public bool useKeyboard = true;


    public enum BM_Dir
    {
        Up,
        Down,
        Left,
        Right,
        None,
    };

    public BM_Dir mDir = BM_Dir.Down;

    //	BM_Player mPlayerData;

    public float wallWidth = 0.2f;

    Control mJoystickCmd = Control.None;



    // Use this for initialization
    void Start()
    {
		level_.GetPoint(transform.position, out player_x, out player_y);
        GetCurrentPositionInfo();
    }



    public void SetBomb()
    {
        level_.SpawnBomb(player_x, player_y, "");
    }

    public void SetControl(Control ctrl)
    {
        mJoystickCmd = ctrl;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
    }


    void PlayerControl()
    {
        switch (mJoystickCmd)
        {
            case Control.Up:
                mDir = WalkUp();
                UpdatePlayerPosition();
                break;

            case Control.UpLeft:
                mDir = WalkUp();
                if (mDir == BM_Dir.None)
                    WalkLeft();
                UpdatePlayerPosition();
                break;

            case Control.LeftUp:
                mDir = WalkLeft();
                if (mDir == BM_Dir.None)
                    WalkUp();
                UpdatePlayerPosition();
                break;

            case Control.UpRight:
                WalkUp();
                if (mDir == BM_Dir.None)
                    WalkRight();
                UpdatePlayerPosition();
                break;

            case Control.RightUp:
                WalkRight();
                if (mDir == BM_Dir.None)
                    WalkUp();
                UpdatePlayerPosition();
                break;

            case Control.Down:
                WalkDown();
                UpdatePlayerPosition();
                break;

            case Control.DownLeft:
                WalkDown();
                if (mDir == BM_Dir.None)
                    WalkLeft();
                UpdatePlayerPosition();
                break;

            case Control.LeftDown:
                WalkLeft();
                if (mDir == BM_Dir.None)
                    WalkDown();
                UpdatePlayerPosition();
                break;

            case Control.DownRight:
                WalkDown();
                if (mDir == BM_Dir.None)
                    WalkRight();
                UpdatePlayerPosition();
                break;

            case Control.RightDown:
                WalkRight();
                if (mDir == BM_Dir.None)
                    WalkDown();
                UpdatePlayerPosition();
                break;

            case Control.Left:
                WalkLeft();
                UpdatePlayerPosition();
                break;

            case Control.Right:
                WalkRight();
                UpdatePlayerPosition();
                break;
        }
    }


    #region  Player Movement
    BM_Dir WalkUp()
    {
        Vector3 curWallPos = level_.GetPositionAt(player_x, player_y);

        // allow to walk up
        if (IsAlllowToPass(upValue))
        {
            Debug.Log("pos" + (this.transform.position.x - curWallPos.x));
            Debug.Log("wall" + (wallWidth));
            if (Mathf.Abs(this.transform.position.x - curWallPos.x) < 0.1f * wallWidth)
            {
                	Debug.Log("move Up");
                TurnDirection(BM_Dir.Up);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Up;
            }
            else if (this.transform.position.x - curWallPos.x > 0.1f * wallWidth)
            {
                //	Debug.Log("move Right");
                TurnDirection(BM_Dir.Right);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Right;
            }
            else if (curWallPos.x - this.transform.position.x > 0.1f * wallWidth)
            {
                //		Debug.Log("move Left");
                TurnDirection(BM_Dir.Left);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Left;
            }
        }
        else
        {
            // left
            if (this.transform.position.x > curWallPos.x + 0.2f * wallWidth)
            {
                // left up
                if (IsAlllowToPass(upLeftValue))
                {
                    //				Debug.Log("move left");
                    TurnDirection(BM_Dir.Left);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Left;
                }
            }
            //right
            else if (this.transform.position.x < curWallPos.x - 0.2f * wallWidth)
            {
                // right up
                if (IsAlllowToPass(upRightValue))
                {
                    //				Debug.Log("move right");
                    TurnDirection(BM_Dir.Right);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Right;
                }
            }
            else
            {
                //			Debug.Log("go back first");
                // go back first  
                if (this.transform.position.z > curWallPos.z)
                {
                    TurnDirection(BM_Dir.Up);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Up;
                }
                else if (this.transform.position.z < curWallPos.z)
                {
                    Vector3 pos = this.transform.position;
                    pos.z = curWallPos.z;
                    this.transform.position = pos;
                    return BM_Dir.Up;
                }
            }
        }
        return BM_Dir.None;
    }

    BM_Dir WalkDown()
    {
        Vector3 curWallPos = level_.GetPositionAt(player_x, player_y);

        // allow to walk down
        if (IsAlllowToPass(downValue))
        {
            if (Mathf.Abs(this.transform.position.x - curWallPos.x) < 0.1f * wallWidth)
            {
                //		Debug.Log("move Down");
                TurnDirection(BM_Dir.Down);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Down;
            }
            else if (this.transform.position.x - curWallPos.x > 0.1f * wallWidth)
            {
                //		Debug.Log("move Right");
                TurnDirection(BM_Dir.Right);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Right;
            }
            else if (curWallPos.x - this.transform.position.x > 0.1f * wallWidth)
            {
                //		Debug.Log("move Left");
                TurnDirection(BM_Dir.Left);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Left;
            }
        }
        else
        {
            // left
            if (this.transform.position.x > curWallPos.x + 0.2f * wallWidth)
            {
                // left down
                if (IsAlllowToPass(downLeftValue))
                {
                    //			Debug.Log("move left");
                    TurnDirection(BM_Dir.Left);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Left;
                }
            }
            //right
            else if (this.transform.position.x < curWallPos.x - 0.2f * wallWidth)
            {
                // right down
                if (IsAlllowToPass(downRightValue))
                {
                    //				Debug.Log("move right");
                    TurnDirection(BM_Dir.Right);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Right;
                }
            }
            else
            {
                //		Debug.Log("go back first");
                // go back first  
                if (this.transform.position.z < curWallPos.z)
                {
                    TurnDirection(BM_Dir.Down);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Down;
                }
                else if (this.transform.position.z > curWallPos.z)
                {
                    Vector3 pos = this.transform.position;
                    pos.z = curWallPos.z;
                    this.transform.position = pos;
                    return BM_Dir.Down;
                }
            }
        }
        return BM_Dir.None;
    }

    BM_Dir WalkLeft()
    {
        Vector3 curWallPos = level_.GetPositionAt(player_x, player_y);

        // allow to walk left
        if (IsAlllowToPass(leftValue))
        {
            if (Mathf.Abs(this.transform.position.z - curWallPos.z) < 0.1f * wallWidth)
            {
                //			Debug.Log("move Left");
                TurnDirection(BM_Dir.Left);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Left;
            }
            else if (this.transform.position.z - curWallPos.z > 0.1f * wallWidth)
            {
                //			Debug.Log("move Up");
                TurnDirection(BM_Dir.Up);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Up;
            }
            else if (curWallPos.z - this.transform.position.z > 0.1f * wallWidth)
            {
                //			Debug.Log("move Down");
                TurnDirection(BM_Dir.Down);
                this.transform.Translate(this.transform.forward * Time.deltaTime * speed);
                return BM_Dir.Down;
            }
        }
        else
        {
            // down
            if (this.transform.position.z > curWallPos.z + 0.2f * wallWidth)
            {
                // down left
                if (IsAlllowToPass(downLeftValue))
                {
                    //				Debug.Log("move down");
                    TurnDirection(BM_Dir.Down);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Down;
                }
            }
            //up
            else if (this.transform.position.z < curWallPos.z - 0.2f * wallWidth)
            {
                //up left 
                if (IsAlllowToPass(upLeftValue))
                {
                    //			Debug.Log("move up");
                    TurnDirection(BM_Dir.Up);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Up;
                }
            }
            else
            {
                //			Debug.Log("go back first");
                // go back first  
                if (this.transform.position.x < curWallPos.x)
                {
                    TurnDirection(BM_Dir.Left);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Left;
                }
                else if (this.transform.position.x > curWallPos.x)
                {
                    Vector3 pos = this.transform.position;
                    pos.x = curWallPos.x;
                    this.transform.position = pos;
                    return BM_Dir.Left;
                }
            }
        }
        return BM_Dir.None;
    }

    BM_Dir WalkRight()
    {
        Vector3 curWallPos = level_.GetPositionAt(player_x, player_y);

        // allow to walk right
        if (IsAlllowToPass(rightValue))
        {
            if (Mathf.Abs(this.transform.position.z - curWallPos.z) < 0.1f * wallWidth)
            {
                //			Debug.Log("move Right");
                TurnDirection(BM_Dir.Right);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Right;
            }
            else if (this.transform.position.z - curWallPos.z > 0.1f * wallWidth)
            {
                //			Debug.Log("move Up");
                TurnDirection(BM_Dir.Up);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return BM_Dir.Up;
            }
            else if (curWallPos.z - this.transform.position.z > 0.1f * wallWidth)
            {
                //			Debug.Log("move Down");
                TurnDirection(BM_Dir.Down);
                this.transform.Translate(this.transform.forward * Time.deltaTime * speed);
                return BM_Dir.Down;
            }
        }
        else
        {
            // down
            if (this.transform.position.z > curWallPos.z + 0.2f * wallWidth)
            {
                // down right
                if (IsAlllowToPass(downRightValue))
                {
                    //			Debug.Log("move down");
                    TurnDirection(BM_Dir.Down);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Down;
                }
            }
            //up
            else if (this.transform.position.z < curWallPos.z - 0.2f * wallWidth)
            {
                //up right 
                if (IsAlllowToPass(upRightValue))
                {
                    //			Debug.Log("move up");
                    TurnDirection(BM_Dir.Up);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Up;
                }
            }
            else
            {
                //		Debug.Log("go back first");
                // go back first  
                if (this.transform.position.x > curWallPos.x)
                {
                    TurnDirection(BM_Dir.Right);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return BM_Dir.Right;
                }
                else if (this.transform.position.x < curWallPos.x)
                {
                    Vector3 pos = this.transform.position;
                    pos.x = curWallPos.x;
                    this.transform.position = pos;
                    return BM_Dir.Right;
                }
            }
        }
        return BM_Dir.None;
    }

    #endregion


    #region Logic Function

    void GetCurrentPositionInfo()
    {
        curValue =  level_[player_x, player_y];

        upValue = level_[player_x, player_y + 1];

        downValue = level_[player_x, player_y - 1];

        leftValue = level_[player_x - 1, player_y];

        rightValue = level_[player_x + 1, player_y];

        upLeftValue = level_[player_x - 1, player_y + 1];

        upRightValue = level_[player_x + 1, player_y + 1];

        downLeftValue = level_[player_x - 1, player_y - 1];

        downRightValue = level_[player_x + 1, player_y - 1];
        
    }

    void UpdatePlayerPosition()
    {
        Vector3 curWallPos = level_.GetPositionAt(player_x, player_y);

        if (this.transform.position.x > curWallPos.x + wallWidth * 0.5f)
        {
            player_x -= 1;
            GetCurrentPositionInfo();
        }
        else if (this.transform.position.x < curWallPos.x - wallWidth * 0.5f)
        {
            player_x += 1;
            GetCurrentPositionInfo();
        }
        else if (this.transform.position.z > curWallPos.z + wallWidth * 0.5f)
        {
            player_y -= 1;
            GetCurrentPositionInfo();
        }
        else if (this.transform.position.z < curWallPos.z - wallWidth * 0.5f)
        {
            player_y += 1;
            GetCurrentPositionInfo();
        }
    }


    #endregion


    #region tool function

    void TurnDirection(BM_Dir dir)
    {
        switch (dir)
        {
            case BM_Dir.Up:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 180, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;

            case BM_Dir.Down:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 0, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case BM_Dir.Left:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 90, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;

            case BM_Dir.Right:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 270, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                break;
        }

    }


    bool IsAlllowToPass(BlockState wallValue)
    {
        //todo  whether the user can pass the wall
        if (wallValue == BlockState.Empty)
            return true;
        else
            return false;
    }



    #endregion


}
