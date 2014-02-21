using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ActorAI : StateMachine<Actor>
{
    public ActorAI(string name) : base(name)
    {
    }
}

public class AIState : State<Actor>
{
    public AIState(string name)
        : base(name)
    {
    }
}

public class SimpleAI : ActorAI
{
    float mIdelTime = 0.5f;
    float mIdelTempTime;
    public SimpleAI()
        : base("Simple")
    {
    }

    public override void OnUpdate(float deltaTime)
    {
        Actor actor = Manager.Holder;
        switch (actor.state)
        {
            case ActorState.Idle:
                if (Time.time - mIdelTempTime > mIdelTime)
                {
                    actor.SendMessage("Stop");
                }
                break;

            case ActorState.Walk:
                int i1 = actor.Move(actor.dir);
                if (i1 <= 0)
                {
                    actor.SendMessage("Stop"); ;
                }
                break;

            case ActorState.Stop:
                int i = UnityEngine.Random.Range(1, 100) % 5;
                if (i == 0)
                {
                    actor.SendMessage("Walk", Direction.Up);
                }
                else if (i == 1)
                {
                    actor.SendMessage("Walk", Direction.Down);
                }
                else if (i == 2)
                {
                    actor.SendMessage("Walk", Direction.Left);
                }
                else if (i == 3)
                {
                    actor.SendMessage("Walk", Direction.Right);
                }
                else
                {
                    mIdelTempTime = Time.time;
                    actor.SendMessage("Idle");
                }
                break;

        }

    }
}
