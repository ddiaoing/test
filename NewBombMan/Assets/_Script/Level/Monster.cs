using UnityEngine;
using System.Collections;

public class Monster : Actor
{

    #region Enemy porperty

    public int hp;

    public bool isAttack;

    #endregion


    public enum EnemyLogicState
    {
        Partol,
        Revenge,
    };

    public enum EnemyState
    {
        Idel,
        Stop,
        StopOver,
        Walk,
        Attack,
        AttackOver,
        Hurt,
        HurtOver,
        Skill,
        SkillOver,
        Die,
        DieOver,
    };

    public EnemyState mState = EnemyState.Idel;

    float mIdelTime = 0.5f;
    float mIdelTempTime;

    public BM_Dir mNextDir = BM_Dir.None;

    protected override void Awake()
    {
        base.Awake();

    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mState)
        {
            case EnemyState.Idel:
                if (Time.time - mIdelTempTime > mIdelTime)
                {
                    mState = EnemyState.Stop;
                }
                break;

            case EnemyState.Walk:
                int i1 = Move(mDir);
                if (i1 <= 0)
                {
                    Debug.Log("i= " + i1 + " x= " + posX + " y=" + posY); ;
                    mState = EnemyState.Stop;
                }
                break;

            case EnemyState.Stop:
                int i = Random.Range(1, 100) % 5;
                if (i == 0)
                {
                    TurnDirection(BM_Dir.Up);
                    mState = EnemyState.Walk;
                }
                else if (i == 1)
                {
                    TurnDirection(BM_Dir.Down);
                    mState = EnemyState.Walk;
                }
                else if (i == 2)
                {
                    TurnDirection(BM_Dir.Left);
                    mState = EnemyState.Walk;
                }
                else if (i == 3)
                {
                    TurnDirection(BM_Dir.Right);
                    mState = EnemyState.Walk;
                }
                else
                {
                    mIdelTempTime = Time.time;
                    mState = EnemyState.Idel;
                }
                break;

        }

    }

    // true :  stop moving
    // false : go on moving
    int Move(BM_Dir dir)
    {
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);
        if (dir == BM_Dir.Up)
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
        else if (dir == BM_Dir.Down)
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
        else if (dir == BM_Dir.Left)
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
        else if (dir == BM_Dir.Right)
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
