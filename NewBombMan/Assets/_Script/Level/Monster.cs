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


    float mIdelTime = 0.5f;
    float mIdelTempTime;

    public Direction mNextDir = Direction.None;

    protected override void Awake()
    {
        base.Awake();

        ActorAI aiState = new SimpleAI();
        ai_.AddState(aiState);
        ai_.ChangeState(aiState);
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {   
        base.Update();
    }

    void SimpleAI()
    {
        switch (mState)
        {
            case ActorState.Idle:
                if (Time.time - mIdelTempTime > mIdelTime)
                {
                    mState = ActorState.Stop;
                }
                break;

            case ActorState.Walk:
                int i1 = Move(dir);
                if (i1 <= 0)
                {
                    Debug.Log("i= " + i1 + " x= " + posX + " y=" + posY); ;
                    mState = ActorState.Stop;
                }
                break;

            case ActorState.Stop:
                int i = Random.Range(1, 100) % 5;
                if (i == 0)
                {
                    TurnDirection(Direction.Up);
                    mState = ActorState.Walk;
                }
                else if (i == 1)
                {
                    TurnDirection(Direction.Down);
                    mState = ActorState.Walk;
                }
                else if (i == 2)
                {
                    TurnDirection(Direction.Left);
                    mState = ActorState.Walk;
                }
                else if (i == 3)
                {
                    TurnDirection(Direction.Right);
                    mState = ActorState.Walk;
                }
                else
                {
                    mIdelTempTime = Time.time;
                    mState = ActorState.Idle;
                }
                break;

        }

    }

}
