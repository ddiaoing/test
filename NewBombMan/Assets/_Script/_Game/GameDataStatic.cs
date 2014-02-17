using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GameDataStatic{
	
	
	public class BM_MapData
	{
		// 0   empty
		// 1   dead cube(can't be sestroy by bomb)
		// 2   normal cube(can be destroy by bomb)
		// -1  bomb here
		public static int[,] allMap;
		
		
		public static Vector3 basePosition;
		
		public static void initMap(int x, int y)
		{
			allMap = new int[x,y];
		}
		
		
		
		public static void ChangeCubeStateOnMap(int x, int y, int _value)
		{
			allMap[x,y] = _value;
		}
		
		public static Vector3 WallPositionByXY(int x, int y)
		{
			Vector3 pos = basePosition;
			
			//map scale = 0.2f
			pos.x += (x-1)*-0.2f;
			pos.z += (y-1)*-0.2f;
			
			return pos;
		}

		
	}
	
	
	
	public class BM_Player
	{
		public BM_Player(int x, int y)
		{
			posX = x;
			posY = y;
		}
		
		// position on map 
		public int posX;
		public int posY;
		
		public float speed;
		
	}
		
	
	
	public class BM_Bomb
	{
		public static string GetBombNameByXY(int x, int y)
		{
			return "BombX"+x+"Y"+y;
		}
	}
}