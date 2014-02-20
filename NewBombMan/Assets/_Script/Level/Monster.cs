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
        Idle,
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

    public EnemyState mState = EnemyState.Idle;

    float mIdelTime = 0.5f;
    float mIdelTempTime;

    public Direction mNextDir = Direction.None;

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
            case EnemyState.Idle:
                if (Time.time - mIdelTempTime > mIdelTime)
                {
                    mState = EnemyState.Stop;
                }
                break;

            case EnemyState.Walk:
                int i1 = Move(dir_);
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
                    TurnDirection(Direction.Up);
                    mState = EnemyState.Walk;
                }
                else if (i == 1)
                {
                    TurnDirection(Direction.Down);
                    mState = EnemyState.Walk;
                }
                else if (i == 2)
                {
                    TurnDirection(Direction.Left);
                    mState = EnemyState.Walk;
                }
                else if (i == 3)
                {
                    TurnDirection(Direction.Right);
                    mState = EnemyState.Walk;
                }
                else
                {
                    mIdelTempTime = Time.time;
                    mState = EnemyState.Idle;
                }
                break;

        }

    }


}
