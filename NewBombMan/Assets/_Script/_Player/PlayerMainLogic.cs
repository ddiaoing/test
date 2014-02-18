using UnityEngine;
using System.Collections;
using GameDataStatic;

public class PlayerMainLogic : MonoBehaviour {
	
	//todo:  will take it out this, to be a "Player Class" 
	#region Player Property
	
	public int curBomb;
	public int maxBomb;
	
	public float speed = 0.5f;
	
	public bool isBombStrong;
	
	public int bombLength = 5;
	
	#endregion
	
	public GameObject bombObj;
	public GameObject bombStrongObj;
	
	public GameObject Map;
	
	public int player_x;
	public int player_y;
	
	public int curValue;
	public int upValue;
	public int downValue;
	public int leftValue;
	public int rightValue;
	public int upLeftValue;
	public int upRightValue;
	public int downLeftValue;
	public int downRightValue;

	public bool useKeyboard = true;

	public enum JoystickCmd
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
	};
	
	public enum BM_Dir
	{
		Up,
		Down,
		Left,
		Right,
		None,
	};
	public BM_Dir mDir = BM_Dir.Down;
	
//	BM_Player mPlayerData;
	
	public float wallWidth;
	
	JoystickCmd mJoystickCmd = JoystickCmd.None;
	
	void Awake()
	{
		BlockMap mMap = (BlockMap)Map.GetComponent<BlockMap>();
		Block a = mMap.GetBlockAt(1,1,0);
		BM_MapData.basePosition = a.transform.position;
	//	mPlayerData = new BM_Player(player_x,player_y);
		Vector3 pos = BM_MapData.WallPositionByXY(player_x,player_y);
		this.transform.position = pos;
		
		Input.multiTouchEnabled = true;
	}
	
	
	// Use this for initialization
	void Start ()
	{
		GetCurrentPositionInfo();
	}
	
	// Update is called once per frame
	void Update () 
	{
		PlayerControl();
	}
	
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width*0.7f,Screen.height*0.7f,Screen.width*0.1f,Screen.width*0.1f),"Bomb"))
		{
			SetBomb(player_x,player_y);
		}
	}
	
	
	void SetBomb(int x, int y)
	{
		Vector3 pos = BM_MapData.WallPositionByXY(x,y);
		// only empty place can set a bomb
		if(BM_MapData.allMap[x,y] == 0)
		{
			if(curBomb < maxBomb)
			{
				GameObject bob;
				if(isBombStrong == false)	
					bob = Instantiate(bombObj,pos,Quaternion.identity) as GameObject;
				else
					bob = Instantiate(bombStrongObj,pos,Quaternion.identity) as GameObject;
				
				bob.name = BM_Bomb.GetBombNameByXY(x,y);
					
				BombMainLogic bobScript = bob.GetComponent<BombMainLogic>();
				bobScript.posX = x;
				bobScript.posY = y;
				bobScript.isStrong = isBombStrong;
				bobScript.mLength = bombLength;
				curBomb += 1;
				BM_MapData.ChangeCubeStateOnMap(x,y,-1);
			}
		}
		
	}
	
	
	void PlayerControl()
	{
		switch(mJoystickCmd)
		{
		case JoystickCmd.Up:
			mDir = WalkUp();
			UpdatePlayerPosition();
			break;
			
		case JoystickCmd.UpLeft:
			mDir = WalkUp();
			if(mDir == BM_Dir.None)
				WalkLeft();
			UpdatePlayerPosition();
			break;
			
		case JoystickCmd.LeftUp:
			mDir = WalkLeft();
			if(mDir == BM_Dir.None)
				WalkUp();
			UpdatePlayerPosition();
			break;
			
		case JoystickCmd.UpRight:
			WalkUp();
			if(mDir == BM_Dir.None)
				WalkRight();
			UpdatePlayerPosition();
			break;	
		
		case JoystickCmd.RightUp:
			WalkRight();
			if(mDir == BM_Dir.None)
				WalkUp();
			UpdatePlayerPosition();
			break;
			
		case JoystickCmd.Down:
			WalkDown();
			UpdatePlayerPosition();
			break;
			
		case JoystickCmd.DownLeft:
			WalkDown();
			if(mDir == BM_Dir.None)
				WalkLeft();
			UpdatePlayerPosition();
			break;	
			
		case JoystickCmd.LeftDown:
			WalkLeft();
			if(mDir == BM_Dir.None)
				WalkDown();
			UpdatePlayerPosition();
			break;
			
		case JoystickCmd.DownRight:
			WalkDown();
			if(mDir == BM_Dir.None)
				WalkRight();
			UpdatePlayerPosition();
			break;	
			
		case JoystickCmd.RightDown:
			WalkRight();
			if(mDir == BM_Dir.None)
				WalkDown();
			UpdatePlayerPosition();
			break;
			
		case JoystickCmd.Left:
			WalkLeft();
			UpdatePlayerPosition();
			break;

		case JoystickCmd.Right:
			WalkRight();
			UpdatePlayerPosition();
			break;
		}
	}
	
	
	#region  Player Movement
	BM_Dir WalkUp()
	{
		Vector3 curWallPos = BM_MapData.WallPositionByXY(player_x, player_y);
		
		// allow to walk up
		if(IsAlllowToPass(upValue))
		{
			if(Mathf.Abs(this.transform.position.x - curWallPos.x) < 0.1f*wallWidth)
			{
			//	Debug.Log("move Up");
				TurnDirection(BM_Dir.Up);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Up;
			}
			else if(this.transform.position.x - curWallPos.x > 0.1f*wallWidth)
			{
			//	Debug.Log("move Right");
				TurnDirection(BM_Dir.Right);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Right;
			}
			else if(curWallPos.x - this.transform.position.x > 0.1f*wallWidth)
			{
		//		Debug.Log("move Left");
				TurnDirection(BM_Dir.Left);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Left;
			}
		}
		else
		{
			// left
			if(this.transform.position.x > curWallPos.x + 0.2f * wallWidth)
			{
				// left up
				if(IsAlllowToPass(upLeftValue))
				{
	//				Debug.Log("move left");
					TurnDirection(BM_Dir.Left);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Left;
				}
			}
			//right
			else if(this.transform.position.x < curWallPos.x - 0.2f * wallWidth)
			{
				// right up
				if(IsAlllowToPass(upRightValue))
				{
	//				Debug.Log("move right");
					TurnDirection(BM_Dir.Right);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Right;
				}
			}
			else
			{
	//			Debug.Log("go back first");
				// go back first  
				if(this.transform.position.z > curWallPos.z)
				{
					TurnDirection(BM_Dir.Up);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Up;
				}
				else if(this.transform.position.z < curWallPos.z)
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
		Vector3 curWallPos = BM_MapData.WallPositionByXY(player_x, player_y);
		
		// allow to walk down
		if(IsAlllowToPass(downValue))
		{
			if(Mathf.Abs(this.transform.position.x - curWallPos.x) < 0.1f*wallWidth)
			{
		//		Debug.Log("move Down");
				TurnDirection(BM_Dir.Down);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Down;
			}
			else if(this.transform.position.x - curWallPos.x > 0.1f*wallWidth)
			{
		//		Debug.Log("move Right");
				TurnDirection(BM_Dir.Right);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Right;
			}
			else if(curWallPos.x - this.transform.position.x > 0.1f*wallWidth)
			{
		//		Debug.Log("move Left");
				TurnDirection(BM_Dir.Left);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Left;
			}
		}
		else
		{
			// left
			if(this.transform.position.x > curWallPos.x + 0.2f * wallWidth)
			{
				// left down
				if(IsAlllowToPass(downLeftValue))
				{
		//			Debug.Log("move left");
					TurnDirection(BM_Dir.Left);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Left;
				}
			}
			//right
			else if(this.transform.position.x < curWallPos.x - 0.2f * wallWidth)
			{
				// right down
				if(IsAlllowToPass(downRightValue))
				{
	//				Debug.Log("move right");
					TurnDirection(BM_Dir.Right);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Right;
				}
			}
			else
			{
		//		Debug.Log("go back first");
				// go back first  
				if(this.transform.position.z < curWallPos.z)
				{
					TurnDirection(BM_Dir.Down);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Down;
				}
				else if(this.transform.position.z > curWallPos.z)
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
		Vector3 curWallPos = BM_MapData.WallPositionByXY(player_x, player_y);
		
		// allow to walk left
		if(IsAlllowToPass(leftValue))
		{
			if(Mathf.Abs(this.transform.position.z - curWallPos.z) < 0.1f*wallWidth)
			{
	//			Debug.Log("move Left");
				TurnDirection(BM_Dir.Left);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Left;
			}
			else if(this.transform.position.z - curWallPos.z > 0.1f*wallWidth)
			{
	//			Debug.Log("move Up");
				TurnDirection(BM_Dir.Up);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Up;
			}
			else if(curWallPos.z - this.transform.position.z > 0.1f*wallWidth)
			{
	//			Debug.Log("move Down");
				TurnDirection(BM_Dir.Down);
				this.transform.Translate(this.transform.forward*Time.deltaTime*speed);
				return BM_Dir.Down;
			}
		}
		else
		{
			// down
			if(this.transform.position.z > curWallPos.z + 0.2f * wallWidth)
			{
				// down left
				if(IsAlllowToPass(downLeftValue))
				{
	//				Debug.Log("move down");
					TurnDirection(BM_Dir.Down);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Down;
				}
			}
			//up
			else if(this.transform.position.z < curWallPos.z - 0.2f * wallWidth)
			{
				//up left 
				if(IsAlllowToPass(upLeftValue))
				{
		//			Debug.Log("move up");
					TurnDirection(BM_Dir.Up);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Up;
				}
			}
			else
			{
	//			Debug.Log("go back first");
				// go back first  
				if(this.transform.position.x < curWallPos.x)
				{
					TurnDirection(BM_Dir.Left);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Left;
				}
				else if(this.transform.position.x > curWallPos.x)
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
		Vector3 curWallPos = BM_MapData.WallPositionByXY(player_x, player_y);
		
		// allow to walk right
		if(IsAlllowToPass(rightValue))
		{
			if(Mathf.Abs(this.transform.position.z - curWallPos.z) < 0.1f*wallWidth)
			{
	//			Debug.Log("move Right");
				TurnDirection(BM_Dir.Right);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Right;
			}
			else if(this.transform.position.z - curWallPos.z > 0.1f*wallWidth)
			{
	//			Debug.Log("move Up");
				TurnDirection(BM_Dir.Up);
				this.transform.position += this.transform.forward*Time.deltaTime*speed;
				return BM_Dir.Up;
			}
			else if(curWallPos.z - this.transform.position.z > 0.1f*wallWidth)
			{
	//			Debug.Log("move Down");
				TurnDirection(BM_Dir.Down);
				this.transform.Translate(this.transform.forward*Time.deltaTime*speed);
				return BM_Dir.Down;
			}
		}
		else
		{
			// down
			if(this.transform.position.z > curWallPos.z + 0.2f * wallWidth)
			{
				// down right
				if(IsAlllowToPass(downRightValue))
				{
		//			Debug.Log("move down");
					TurnDirection(BM_Dir.Down);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Down;
				}
			}
			//up
			else if(this.transform.position.z < curWallPos.z - 0.2f * wallWidth)
			{
				//up right 
				if(IsAlllowToPass(upRightValue))
				{
		//			Debug.Log("move up");
					TurnDirection(BM_Dir.Up);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Up;
				}
			}
			else
			{
		//		Debug.Log("go back first");
				// go back first  
				if(this.transform.position.x > curWallPos.x)
				{
					TurnDirection(BM_Dir.Right);
					this.transform.position += this.transform.forward*Time.deltaTime*speed;
					return BM_Dir.Right;
				}
				else if(this.transform.position.x < curWallPos.x)
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
	
	
	#region Logic Function
	
	void GetCurrentPositionInfo()
	{
		curValue = BM_MapData.allMap[player_x,player_y];
		
		upValue     = BM_MapData.allMap[player_x,player_y+1];
		
		downValue = BM_MapData.allMap[player_x,player_y-1];
		
		leftValue    = BM_MapData.allMap[player_x-1,player_y];
		
		rightValue  = BM_MapData.allMap[player_x+1,player_y];
		
		upLeftValue        = BM_MapData.allMap[player_x-1,player_y+1];
		
		upRightValue      = BM_MapData.allMap[player_x+1,player_y+1];
		
		downLeftValue    = BM_MapData.allMap[player_x-1,player_y-1];
		
		downRightValue  = BM_MapData.allMap[player_x+1,player_y-1];
		
	}
	
	void UpdatePlayerPosition()
	{
		Vector3 curWallPos = BM_MapData.WallPositionByXY(player_x,player_y);
		
		if(this.transform.position.x > curWallPos.x + wallWidth*0.5f)
		{
			player_x -= 1;
			GetCurrentPositionInfo();
		}
		else if(this.transform.position.x < curWallPos.x - wallWidth*0.5f)
		{
			player_x += 1;
			GetCurrentPositionInfo();
		}		
		else if(this.transform.position.z > curWallPos.z + wallWidth*0.5f)
		{
			player_y -= 1;
			GetCurrentPositionInfo();
		}
		else if(this.transform.position.z < curWallPos.z - wallWidth*0.5f)
		{
			player_y += 1;
			GetCurrentPositionInfo();
		}	
	}
	
	
	#endregion
	
	
	#region tool function
	
	void TurnDirection(BM_Dir dir)
	{
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

	
	bool IsAlllowToPass(int wallValue)
	{
		//todo  whether the user can pass the wall
		if(wallValue == 0)
			return true;
		else
			return false;
	}
	
	float JoystickAngle(Vector2 pos)
	{
		float angle = Vector2.Angle(pos,Vector2.up);
		Debug.Log(angle);
		return angle;
	}
	
	#endregion
	
	
	#region  Easy Touch
	// Easy Touch Plugin Function
	void On_JoystickMove(MovingJoystick move)
	{
		// x -1 ~ 1  left ~ right
		// y -1 ~ 1  down ~ up
		float baseDis = 0.05f;
		
		Vector2 dir = move.joystickAxis;
		
		float angle = JoystickAngle(dir);
		
		if(angle <= 15)
		{
			mJoystickCmd = JoystickCmd.Up;
			return;
		}
		if(angle > 165)
		{
			mJoystickCmd = JoystickCmd.Down;
			return;
		}
		
		if(dir.x >= 0)//right
		{
			if(angle > 15 && angle <= 45)
			{
				mJoystickCmd = JoystickCmd.UpRight;
				return;
			}
			else if(angle > 45 && angle <= 75)
			{
				mJoystickCmd = JoystickCmd.RightUp;
				return;
			}
			else if(angle > 75 && angle <= 105)
			{
				mJoystickCmd = JoystickCmd.Right;
				return;
			}
			else if(angle > 105 && angle <= 135)
			{
				mJoystickCmd = JoystickCmd.RightDown;
				return;
			}
			else if(angle > 135 && angle <= 165)
			{
				mJoystickCmd = JoystickCmd.DownRight;
				return;
			}
		}
		else//left
		{
			if(angle > 15 && angle <= 45)
			{
				mJoystickCmd = JoystickCmd.UpLeft;
				return;
			}
			else if(angle > 45 && angle <= 75)
			{
				mJoystickCmd = JoystickCmd.LeftUp;
				return;
			}
			else if(angle > 75 && angle <= 105)
			{
				mJoystickCmd = JoystickCmd.Left;
				return;
			}
			else if(angle > 105 && angle <= 135)
			{
				mJoystickCmd = JoystickCmd.LeftDown;
				return;
			}
			else if(angle > 135 && angle <= 165)
			{
				mJoystickCmd = JoystickCmd.DownLeft;
				return;
			}
		}
			
	/*	
		if(dir.x > baseDis)//right
		{
			
			if(dir.y > baseDis)// up
			{
				mJoystickCmd = JoystickCmd.UpRight;
				return;
			}
			else if(dir.y < -1*baseDis)//down
			{
				mJoystickCmd = JoystickCmd.DownRight;
				return;
			}
			else
			{
				mJoystickCmd = JoystickCmd.Right;
				return;
			}
		}
		else if(dir.x < -1*baseDis)//left
		{
			if(dir.y > baseDis)// up
			{
				mJoystickCmd = JoystickCmd.UpLeft;
				return;
			}
			else if(dir.y < -1*baseDis)//down
			{
				mJoystickCmd = JoystickCmd.DownLeft;
				return;
			}
			else
			{
				mJoystickCmd = JoystickCmd.Left;
				return;
			}
		}
		else
		{
			if(dir.y > baseDis)// up
			{
				mJoystickCmd = JoystickCmd.Up;
				return;
			}
			else if(dir.y < -1*baseDis)//down
			{
				mJoystickCmd = JoystickCmd.Down;
				return;
			}
			else
			{
				mJoystickCmd = JoystickCmd.None;
				return;
			}
		}
		*/
		
	}
	
	// Easy Touch Plugin Function
	void On_JoystickMoveEnd(MovingJoystick move)
	{
		mJoystickCmd = JoystickCmd.None;
	}
	#endregion

}
