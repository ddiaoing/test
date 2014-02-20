using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Player : Actor
{
    public int curBomb;
    public int maxBomb;
    public bool isBombStrong;
    public int bombLength = 5;

    Control control_ = Control.None;


    public void SetBomb()
    {
        if (isBombStrong)
        {

        }
        else
        {
            level_.SpawnBomb(posX, posY, "Prefab/Bombs/Bomb");
        }

    }

    public void SetControl(Control ctrl)
    {
        control_ = ctrl;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
    }


    void PlayerControl()
    {
        switch (control_)
        {
            case Control.Up:
                mDir = WalkUp();
                UpdatePosition();
                break;

            case Control.UpLeft:
                mDir = WalkUp();
                if (mDir == BM_Dir.None)
                    WalkLeft();
                UpdatePosition();
                break;

            case Control.LeftUp:
                mDir = WalkLeft();
                if (mDir == BM_Dir.None)
                    WalkUp();
                UpdatePosition();
                break;

            case Control.UpRight:
                WalkUp();
                if (mDir == BM_Dir.None)
                    WalkRight();
                UpdatePosition();
                break;

            case Control.RightUp:
                WalkRight();
                if (mDir == BM_Dir.None)
                    WalkUp();
                UpdatePosition();
                break;

            case Control.Down:
                WalkDown();
                UpdatePosition();
                break;

            case Control.DownLeft:
                WalkDown();
                if (mDir == BM_Dir.None)
                    WalkLeft();
                UpdatePosition();
                break;

            case Control.LeftDown:
                WalkLeft();
                if (mDir == BM_Dir.None)
                    WalkDown();
                UpdatePosition();
                break;

            case Control.DownRight:
                WalkDown();
                if (mDir == BM_Dir.None)
                    WalkRight();
                UpdatePosition();
                break;

            case Control.RightDown:
                WalkRight();
                if (mDir == BM_Dir.None)
                    WalkDown();
                UpdatePosition();
                break;

            case Control.Left:
                WalkLeft();
                UpdatePosition();
                break;

            case Control.Right:
                WalkRight();
                UpdatePosition();
                break;
        }
    }


    #region  Player Movement
    BM_Dir WalkUp()
    {
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

        // allow to walk up
        if (IsAlllowToPass(upValue))
        {
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
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

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
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

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
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

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



}
