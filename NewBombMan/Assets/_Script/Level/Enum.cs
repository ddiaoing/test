using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Control
{
    None,
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
    LeftUp,
    LeftDown,
    RightUp,
    RightDown,
}

public enum BlockState : int
{
	Empty = 0,
	DeadCube,
	Cube,
	
	Bomb = -1
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None,
}


public enum ActorState
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
/*
public enum AIState
{
    Thinking,
    Partol,
    Revenge
}*/