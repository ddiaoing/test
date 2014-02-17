using UnityEngine;
using System.Collections;
using GameDataStatic;

public class BombMainLogic : MonoBehaviour {
	
	public Object flameObj;
	
	// position on map
	public int posX;
	public int posY;
	
	// at least 2
	public int mLength;
	
	// player property
	public bool isStrong;
	
	// is the bomb be hit by others
	public bool beHit;
	
	// bomb cd time
	float mBombTime = 5.0f;
	
	MapMailLogic mapScript;
	PlayerMainLogic playerScript;
	
	float mTime;
	bool[] frameStop = new bool[4];
	
	// if the bomb explode already
	bool bombAlready = false;
	
	
	// Use this for initialization
	void Start () {
		for(int i=0; i<frameStop.Length; i++)
		{
			frameStop[i] = false;
		}
		
		mTime = Time.time;
		
		playerScript = GameObject.Find("Player").GetComponent<PlayerMainLogic>();
	}
	
	// Update is called once per flame
	void Update () {
		
		if( (Time.time - mTime > mBombTime||beHit == true)
			&&bombAlready == false)
		{
			bombAlready = true;
			Bomb();
			playerScript.curBomb -= 1;
			Destroy(this.gameObject, 1.5f);
		}
	}
	
	
	void Bomb()
	{
		BM_MapData.ChangeCubeStateOnMap(posX,posY,0);
		
		// hide the bomb
		Hide();
		FlameOn(posX,posY,0);
		
		// create the flame 
		for(int i=1; i<=mLength; i++)
		{
			//up
			if(frameStop[0] == false)
				frameStop[0] = FlameOn(posX,posY+i,i);
			
			//down
			if(frameStop[1] == false)
				frameStop[1] = FlameOn(posX,posY-i,i);
			
			//left
			if(frameStop[2] == false)
				frameStop[2] = FlameOn(posX-i,posY,i);
			
			//right
			if(frameStop[3] == false)
				frameStop[3] = FlameOn(posX+i,posY,i);
		}
	}
	
	
	/// <summary>
	/// Flames on.
	/// if the flame stop, return true
	/// </summary>
	bool FlameOn(int x, int y, int curLength)
	{
		GameObject flame = null;
		FlameBase scp;
		Vector3 pos = BM_MapData.WallPositionByXY(x,y);
		switch(BM_MapData.allMap[x,y])
		{
		case 0: // empty
			flame = Instantiate(flameObj,pos,Quaternion.identity) as GameObject;
			scp = flame.GetComponent<FlameBase>();
			scp.curLength = curLength;
			flame.transform.parent = this.transform;
			return false;
			
		case 1: // dead cube
			return true;
			
		case 2: // normal cube
			flame = Instantiate(flameObj,pos,Quaternion.identity) as GameObject;
			scp = flame.GetComponent<FlameBase>();
			scp.curLength = curLength;
			flame.transform.parent = this.transform;
			
			mapScript.DeleteCubeOnMap(x,y);
			if(isStrong)
				return false;
			return true;
			
		case -1: // bomb here
			flame = Instantiate(flameObj,pos,Quaternion.identity) as GameObject;
			scp = flame.GetComponent<FlameBase>();
			scp.curLength = curLength;
			flame.transform.parent = this.transform;
			
			GameObject bomb = GameObject.Find(BM_Bomb.GetBombNameByXY(x,y));
			if(bomb != null)
			{
				BombMainLogic bob = bomb.GetComponent<BombMainLogic>();
				bob.beHit = true;
			}
			return false;
		}
		Debug.Log("error : : nothing found!!!!!");
		return true;
	}
	
	
	void Hide()
	{
		Renderer[] allRender = this.GetComponentsInChildren<Renderer>();
		foreach(Renderer render in allRender)
		{
			render.enabled = false;
		}
		
		
		// close the render
		// close the collider
		// and so on
	}
	

	
	
	
}
