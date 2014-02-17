using UnityEngine;
using System.Collections;
using DopplerInteractive.TidyTileMapper.Utilities;
using GameDataStatic;
using PathFindingSpace;

public class MapMailLogic : MonoBehaviour {

	BlockMap mMap;
	
	
	void Awake()
	{
		mMap = (BlockMap)this.GetComponent<BlockMap>();
		
		BM_MapData.initMap(mMap.currentWidth,mMap.currentHeight);
		
		for(int x=1; x< mMap.currentWidth-1; x++)
		{
			for(int y=1; y<mMap.currentHeight-1; y++)
			{
				Block a = mMap.GetBlockAt(x,y,0);
				if(a != null)
				{
					BM_MapData.allMap[x,y] = 1;
			//		Debug.Log("x="+x+"  y="+y+" is not null");
				}
				else
				{
					BM_MapData.allMap[x,y] = 0;
				//	Debug.Log("x="+x+"  y="+y+" is null");
				}
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		
//		AStarRoute path = new AStarRoute(BM_MapData.allMap,13,13, 4,2,6,3);
//		BM_Node[] x1 = path.getResult();
//		for(int i=0; i<x1.Length; i++)
	//		Debug.Log(" x = "+x1[i].x+"   y= "+x1[i].y);
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	
	public void DeleteCubeOnMap(int x, int y)
	{
		BM_MapData.ChangeCubeStateOnMap(x,y,0);
		
	}
	
	
	
}
