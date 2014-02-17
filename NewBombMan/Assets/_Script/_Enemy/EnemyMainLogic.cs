using UnityEngine;
using System.Collections;
using GameDataStatic;

public class EnemyMainLogic : MonoBehaviour {
	
	#region Enemy porperty
	
	public int hp;
	public float speed;
	public bool walkingInWall;
	public bool isAttack;
	
	#endregion
	
	public GameObject Map;
	
	public int posX, posY;
	
	public int curValue;
	
	public int upValue;
	public int downValue;
	public int leftValue;
	public int rightValue;
	public int upLeftValue;
	public int upRightValue;
	public int downLeftValue;
	public int downRightValue;
	
	
	public float wallWidth = 0.2f;
	
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
	
	public enum BM_Dir
	{
		Up,
		Down,
		Left,
		Right,
		None,
	};

	public BM_Dir mDir = BM_Dir.Down;	
	public BM_Dir mNextDir = BM_Dir.None;
	
	void Awake()
	{
		BlockMap mMap = (BlockMap)Map.GetComponent<BlockMap>();
		Block a = mMap.GetBlockAt(1,1,0);
		BM_MapData.basePosition = a.transform.position;
		Vector3 pos = BM_MapData.WallPositionByXY(posX,posY);
		this.transform.position = pos;
	}
	
	// Use this for initialization
	void Start () {
		
		GetCurrentPositionInfo();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(mState)
		{
		case EnemyState.Idel:
			if(Time.time - mIdelTempTime > mIdelTime)
			{
				mState = EnemyState.Stop;
			}
			break;
			
		case EnemyState.Walk:	
			int i1=Move(mDir);
			if(i1<=0)
			{
				Debug.Log("i= "+i1+" x= "+posX+" y="+posY);;
				mState = EnemyState.Stop;
			}
			break;
			
		case EnemyState.Stop:
			int i = Random.Range(1,100)%5;
			if(i == 0)
			{
				TurnDirection(BM_Dir.Up);
				mState = EnemyState.Walk;
			}
			else if(i == 1)
			{
				TurnDirection(BM_Dir.Down);
				mState = EnemyState.Walk;
			}
			else if(i == 2)
			{
				TurnDirection(BM_Dir.Left);
				mState = EnemyState.Walk;
			}
			else if(i == 3)
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
		Vector3 curWallPos = BM_MapData.WallPositionByXY(posX, posY);
		if(dir == BM_Dir.Up)
		{
			if(IsAlllowToPass(upValue))
			{
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				UpdatePosition();
				return 1;
			}
			else
			{	
				if(this.transform.position.z > curWallPos.z)
				{
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return 1;
				}
				else if(this.transform.position.z < curWallPos.z)
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
		else if(dir == BM_Dir.Down)
		{
			if(IsAlllowToPass(downValue))
			{
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				UpdatePosition();
				return 1;
			}
			else
			{		
				if(this.transform.position.z < curWallPos.z)
				{
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return 1;
				}
				else if(this.transform.position.z > curWallPos.z)
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
		else if(dir == BM_Dir.Left)
		{
			if(IsAlllowToPass(leftValue))
			{
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				UpdatePosition();
				return 1;
			}
			else
			{				
				if(this.transform.position.x < curWallPos.x)
				{
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return 1;
				}
				else if(this.transform.position.x > curWallPos.x)
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
		else if(dir == BM_Dir.Right)
		{
			if(IsAlllowToPass(rightValue))
			{
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				UpdatePosition();
				return 1;
			}
			else
			{			
				if(this.transform.position.x > curWallPos.x)
				{
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return 1;
				}
				else if(this.transform.position.x < curWallPos.x)
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
		
	
	#region Logic function
	
	bool IsAlllowToPass(int wallValue)
	{
		//todo  whether the user can pass the wall
		if(wallValue == 0)
			return true;
		else 
		{
			if(wallValue == 2 && walkingInWall)
				return true;
			return false;
		}
	}
	
	void TurnDirection(BM_Dir dir)
	{
		mDir = dir;
		switch(dir)
		{
		case BM_Dir.Up:
			if(this.transform.rotation != Quaternion.Euler(new Vector3(0,180,0)))
				this.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
			break;
			
		case BM_Dir.Down:
			if(this.transform.rotation != Quaternion.Euler(new Vector3(0,0,0)))
				this.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
			break;
			
		case BM_Dir.Left:
			if(this.transform.rotation != Quaternion.Euler(new Vector3(0,90,0)))
				this.transform.rotation = Quaternion.Euler(new Vector3(0,90,0));
			break;
			
		case BM_Dir.Right:
			if(this.transform.rotation != Quaternion.Euler(new Vector3(0,270,0)))
				this.transform.rotation = Quaternion.Euler(new Vector3(0,270,0));
			break;
		}
		
	}
	
	void GetCurrentPositionInfo()
	{
		curValue = BM_MapData.allMap[posX,posY];
		
		upValue     = BM_MapData.allMap[posX,posY+1];
		
		downValue = BM_MapData.allMap[posX,posY-1];
		
		leftValue    = BM_MapData.allMap[posX-1,posY];
		
		rightValue  = BM_MapData.allMap[posX+1,posY];
		
		upLeftValue        = BM_MapData.allMap[posX-1,posY+1];
		
		upRightValue      = BM_MapData.allMap[posX+1,posY+1];
		
		downLeftValue    = BM_MapData.allMap[posX-1,posY-1];
		
		downRightValue  = BM_MapData.allMap[posX+1,posY-1];
	}
	
	void UpdatePosition()
	{
		Vector3 curWallPos = BM_MapData.WallPositionByXY(posX,posY);
		
		if(this.transform.position.x > curWallPos.x + wallWidth*0.5f)
		{
			posX -= 1;
			GetCurrentPositionInfo();
		}
		else if(this.transform.position.x < curWallPos.x - wallWidth*0.5f)
		{
			posX += 1;
			GetCurrentPositionInfo();
		}		
		else if(this.transform.position.z > curWallPos.z + wallWidth*0.5f)
		{
			posY -= 1;
			GetCurrentPositionInfo();
		}
		else if(this.transform.position.z < curWallPos.z - wallWidth*0.5f)
		{
			posY += 1;
			GetCurrentPositionInfo();
		}	
	}
	
	
	#endregion

}
