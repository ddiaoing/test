using UnityEngine;
using System.Collections;

public class Monster : Actor
{

    #region Enemy porperty

    public int hp;

    public bool isAttack;

    #endregion


    MonsterInfo info;

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

}
