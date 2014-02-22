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
    protected override void Update()
    {
        if (control_ != Control.None)
        {
            PlayerControl();

        }
        else
        {
            base.Update();
        }

    }


    void PlayerControl()
    {
        switch (control_)
        {
            case Control.Up:
                dir = WalkUp();
                UpdatePosition();
                break;

            case Control.UpLeft:
                dir = WalkUp();
                if (dir == Direction.None)
                    WalkLeft();
                UpdatePosition();
                break;

            case Control.LeftUp:
                dir = WalkLeft();
                if (dir == Direction.None)
                    WalkUp();
                UpdatePosition();
                break;

            case Control.UpRight:
                WalkUp();
                if (dir == Direction.None)
                    WalkRight();
                UpdatePosition();
                break;

            case Control.RightUp:
                WalkRight();
                if (dir == Direction.None)
                    WalkUp();
                UpdatePosition();
                break;

            case Control.Down:
                WalkDown();
                UpdatePosition();
                break;

            case Control.DownLeft:
                WalkDown();
                if (dir == Direction.None)
                    WalkLeft();
                UpdatePosition();
                break;

            case Control.LeftDown:
                WalkLeft();
                if (dir == Direction.None)
                    WalkDown();
                UpdatePosition();
                break;

            case Control.DownRight:
                WalkDown();
                if (dir == Direction.None)
                    WalkRight();
                UpdatePosition();
                break;

            case Control.RightDown:
                WalkRight();
                if (dir == Direction.None)
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

        if (control_ == Control.None)
        {
            animator.SetFloat("Speed", 0.0f);
        }
        else
        {
            animator.SetFloat("Speed", 0.5f);
        }


    }


    #region  Player Movement
    Direction WalkUp()
    {


        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

        // allow to walk up
        if (IsAlllowToPass(upValue))
        {
            if (Mathf.Abs(this.transform.position.x - curWallPos.x) < 0.1f * wallWidth)
            {
                	Debug.Log("move Up");
                TurnDirection(Direction.Up);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Up;
            }
            else if (this.transform.position.x - curWallPos.x > 0.1f * wallWidth)
            {
                //	Debug.Log("move Right");
                TurnDirection(Direction.Right);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Right;
            }
            else if (curWallPos.x - this.transform.position.x > 0.1f * wallWidth)
            {
                //		Debug.Log("move Left");
                TurnDirection(Direction.Left);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Left;
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
                    TurnDirection(Direction.Left);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Left;
                }
            }
            //right
            else if (this.transform.position.x < curWallPos.x - 0.2f * wallWidth)
            {
                // right up
                if (IsAlllowToPass(upRightValue))
                {
                    //				Debug.Log("move right");
                    TurnDirection(Direction.Right);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Right;
                }
            }
            else
            {
                //			Debug.Log("go back first");
                // go back first  
                if (this.transform.position.z > curWallPos.z)
                {
                    TurnDirection(Direction.Up);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Up;
                }
                else if (this.transform.position.z < curWallPos.z)
                {
                    Vector3 pos = this.transform.position;
                    pos.z = curWallPos.z;
                    this.transform.position = pos;
                    return Direction.Up;
                }
            }
        }
        return Direction.None;
    }

    Direction WalkDown()
    {
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

        // allow to walk down
        if (IsAlllowToPass(downValue))
        {
            if (Mathf.Abs(this.transform.position.x - curWallPos.x) < 0.1f * wallWidth)
            {
                //		Debug.Log("move Down");
                TurnDirection(Direction.Down);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Down;
            }
            else if (this.transform.position.x - curWallPos.x > 0.1f * wallWidth)
            {
                //		Debug.Log("move Right");
                TurnDirection(Direction.Right);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Right;
            }
            else if (curWallPos.x - this.transform.position.x > 0.1f * wallWidth)
            {
                //		Debug.Log("move Left");
                TurnDirection(Direction.Left);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Left;
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
                    TurnDirection(Direction.Left);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;

                    return Direction.Left;
                }
            }
            //right
            else if (this.transform.position.x < curWallPos.x - 0.2f * wallWidth)
            {
                // right down
                if (IsAlllowToPass(downRightValue))
                {
                    //				Debug.Log("move right");
                    TurnDirection(Direction.Right);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Right;
                }
            }
            else
            {
                //		Debug.Log("go back first");
                // go back first  
                if (this.transform.position.z < curWallPos.z)
                {
                    TurnDirection(Direction.Down);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Down;
                }
                else if (this.transform.position.z > curWallPos.z)
                {
                    Vector3 pos = this.transform.position;
                    pos.z = curWallPos.z;
                    this.transform.position = pos;
                    return Direction.Down;
                }
            }
        }
        return Direction.None;
    }

    Direction WalkLeft()
    {
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

        // allow to walk left
        if (IsAlllowToPass(leftValue))
        {
            if (Mathf.Abs(this.transform.position.z - curWallPos.z) < 0.1f * wallWidth)
            {
                //			Debug.Log("move Left");
                TurnDirection(Direction.Left);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Left;
            }
            else if (this.transform.position.z - curWallPos.z > 0.1f * wallWidth)
            {
                //			Debug.Log("move Up");
                TurnDirection(Direction.Up);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Up;
            }
            else if (curWallPos.z - this.transform.position.z > 0.1f * wallWidth)
            {
                //			Debug.Log("move Down");
                TurnDirection(Direction.Down);
                this.transform.Translate(this.transform.forward * Time.deltaTime * speed);
                return Direction.Down;
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
                    TurnDirection(Direction.Down);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Down;
                }
            }
            //up
            else if (this.transform.position.z < curWallPos.z - 0.2f * wallWidth)
            {
                //up left 
                if (IsAlllowToPass(upLeftValue))
                {
                    //			Debug.Log("move up");
                    TurnDirection(Direction.Up);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Up;
                }
            }
            else
            {
                //			Debug.Log("go back first");
                // go back first  
                if (this.transform.position.x < curWallPos.x)
                {
                    TurnDirection(Direction.Left);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Left;
                }
                else if (this.transform.position.x > curWallPos.x)
                {
                    Vector3 pos = this.transform.position;
                    pos.x = curWallPos.x;
                    this.transform.position = pos;
                    return Direction.Left;
                }
            }
        }
        return Direction.None;
    }

    Direction WalkRight()
    {
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

        // allow to walk right
        if (IsAlllowToPass(rightValue))
        {
            if (Mathf.Abs(this.transform.position.z - curWallPos.z) < 0.1f * wallWidth)
            {
                //			Debug.Log("move Right");
                TurnDirection(Direction.Right);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Right;
            }
            else if (this.transform.position.z - curWallPos.z > 0.1f * wallWidth)
            {
                //			Debug.Log("move Up");
                TurnDirection(Direction.Up);
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                return Direction.Up;
            }
            else if (curWallPos.z - this.transform.position.z > 0.1f * wallWidth)
            {
                //			Debug.Log("move Down");
                TurnDirection(Direction.Down);
                this.transform.Translate(this.transform.forward * Time.deltaTime * speed);
                return Direction.Down;
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
                    TurnDirection(Direction.Down);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Down;
                }
            }
            //up
            else if (this.transform.position.z < curWallPos.z - 0.2f * wallWidth)
            {
                //up right 
                if (IsAlllowToPass(upRightValue))
                {
                    //			Debug.Log("move up");
                    TurnDirection(Direction.Up);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Up;
                }
            }
            else
            {
                //		Debug.Log("go back first");
                // go back first  
                if (this.transform.position.x > curWallPos.x)
                {
                    TurnDirection(Direction.Right);
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return Direction.Right;
                }
                else if (this.transform.position.x < curWallPos.x)
                {
                    Vector3 pos = this.transform.position;
                    pos.x = curWallPos.x;
                    this.transform.position = pos;
                    return Direction.Right;
                }
            }
        }
        return Direction.None;
    }

    #endregion



}
