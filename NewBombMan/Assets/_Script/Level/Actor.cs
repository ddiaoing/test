using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour 
{
    protected StateMachine<Actor> ai_;
    protected Level level_;

    public int posX, posY;

    public float speed = 0.5f;
    public float wallWidth = 0.2f;

    public bool walkingInWall;

    protected BlockState curValue;
    protected BlockState upValue;
    protected BlockState downValue;
    protected BlockState leftValue;
    protected BlockState rightValue;
    protected BlockState upLeftValue;
    protected BlockState upRightValue;
    protected BlockState downLeftValue;
    protected BlockState downRightValue;


    public Direction dir = Direction.Down;

    protected ActorState mState = ActorState.Idle;
    public ActorState state
    {
        get { return mState; }
    }

    protected Animator animator;

    protected virtual void Awake()
    {
        level_ = Level.Instance;
        animator = GetComponent<Animator>();
        ai_ = new StateMachine<Actor>(this);
    }
	// Use this for initialization
    protected virtual void Start()
    {
        level_.GetPoint(transform.position, out posX, out posY);
        GetCurrentPositionInfo();
	}

    protected virtual void Update()
    {



        ai_.OnUpdate(Time.deltaTime);
    }

    // invoke by SendMessage
#region EVENT_HANDLE

    void Idle()
    {
        mState = ActorState.Idle;
        animator.SetFloat("Speed", 0.0f);
     //   animator.SetTrigger("Idle");
    }

    void Stop()
    {
        mState = ActorState.Stop;
        animator.SetFloat("Speed", 0.0f);
    }

    void Walk(object param)
    {
        mState = ActorState.Walk;
        animator.SetFloat("Speed", 0.5f);
        TurnDirection((Direction)param);
    }

    void Attack()
    {
        mState = ActorState.Attack;
    }

    void Hurt()
    {
        mState = ActorState.Hurt;
    }

    void OnFlamed(object param)
    {
        animator.SetTrigger("Die");
   //     level_.DestroyObject(this.gameObject);
    }

#endregion

    protected void GetCurrentPositionInfo()
    {
        curValue = level_[posX, posY];

        upValue = level_[posX, posY + 1];

        downValue = level_[posX, posY - 1];

        leftValue = level_[posX - 1, posY];

        rightValue = level_[posX + 1, posY];

        upLeftValue = level_[posX - 1, posY + 1];

        upRightValue = level_[posX + 1, posY + 1];

        downLeftValue = level_[posX - 1, posY - 1];

        downRightValue = level_[posX + 1, posY - 1];
    }

    protected void UpdatePosition()
    {
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

        if (this.transform.position.x > curWallPos.x + wallWidth * 0.5f)
        {
            posX -= 1;
            GetCurrentPositionInfo();
        }
        else if (this.transform.position.x < curWallPos.x - wallWidth * 0.5f)
        {
            posX += 1;
            GetCurrentPositionInfo();
        }
        else if (this.transform.position.z > curWallPos.z + wallWidth * 0.5f)
        {
            posY -= 1;
            GetCurrentPositionInfo();
        }
        else if (this.transform.position.z < curWallPos.z - wallWidth * 0.5f)
        {
            posY += 1;
            GetCurrentPositionInfo();
        }
    }

    public bool IsAlllowToPass(BlockState wallValue)
    {
        //todo  whether the user can pass the wall
        if (wallValue == BlockState.Empty)
            return true;
        else
        {
            if (wallValue == BlockState.Cube && walkingInWall)
                return true;
            return false;
        }
    }

    public void TurnDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 180, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;

            case Direction.Down:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 0, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case Direction.Left:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 90, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;

            case Direction.Right:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 270, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                break;
        }

        this.dir = dir;
    }

    public int Move(Direction dir)
    {
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);
        if (dir == Direction.Up)
        {
            if (IsAlllowToPass(upValue))
            {
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                UpdatePosition();
                return 1;
            }
            else
            {
                if (this.transform.position.z > curWallPos.z)
                {
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return 1;
                }
                else if (this.transform.position.z < curWallPos.z)
                {
                    Vector3 pos = this.transform.position;
                    pos.z = curWallPos.z;
                    this.transform.position = pos;
                    UpdatePosition();
                    return 0;
                }
                return -1;
            }
        }
        else if (dir == Direction.Down)
        {
            if (IsAlllowToPass(downValue))
            {
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                UpdatePosition();
                return 1;
            }
            else
            {
                if (this.transform.position.z < curWallPos.z)
                {
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return 1;
                }
                else if (this.transform.position.z > curWallPos.z)
                {
                    Vector3 pos = this.transform.position;
                    pos.z = curWallPos.z;
                    this.transform.position = pos;
                    UpdatePosition();
                    return 0;
                }
                return -1;
            }

        }
        else if (dir == Direction.Left)
        {
            if (IsAlllowToPass(leftValue))
            {
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                UpdatePosition();
                return 1;
            }
            else
            {
                if (this.transform.position.x < curWallPos.x)
                {
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return 1;
                }
                else if (this.transform.position.x > curWallPos.x)
                {
                    Vector3 pos = this.transform.position;
                    pos.z = curWallPos.z;
                    this.transform.position = pos;
                    UpdatePosition();
                    return 0;
                }
                return -1;
            }
        }
        else if (dir == Direction.Right)
        {
            if (IsAlllowToPass(rightValue))
            {
                this.transform.position += this.transform.forward * Time.deltaTime * speed;
                UpdatePosition();
                return 1;
            }
            else
            {
                if (this.transform.position.x > curWallPos.x)
                {
                    this.transform.position += this.transform.forward * Time.deltaTime * speed;
                    return 1;
                }
                else if (this.transform.position.x < curWallPos.x)
                {
                    Vector3 pos = this.transform.position;
                    pos.z = curWallPos.z;
                    this.transform.position = pos;
                    UpdatePosition();
                    return 0;
                }
                return -1;
            }

        }
        return -1;
    }


}
