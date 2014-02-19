using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DopplerInteractive.TidyTileMapper.Utilities;


public enum BlockState : int
{
    Empty = 0,
    DeadCube,
    Cube,

    Bomb = -1
}

public class Level : SingletonBehaviour<Level>
{
    BlockState[,] allMap;
    int w, h;
    BlockMap map_;

    public Player localPlayer_; //玩家自己
    List<GameObject> players_;  //网络玩家或者ai
    List<GameObject> monsters_; //怪物

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Load(string mapFile)
    {
		GameObject res = Resources.Load(mapFile) as GameObject;
		GameObject obj = Instantiate(res,Vector3.zero,Quaternion.identity) as GameObject;
        Load(obj.GetComponent<BlockMap>());
    }
    
    public void Load(BlockMap map)
    {
        w = map.currentWidth;
        h = map.currentHeight;  
        map_ = map;

        float halfWidth = w * 0.2f / 2;
        float halfHeight = h * 0.2f / 2;

        Vector3 center = new Vector3(halfWidth - 0.1f, 0.0f, halfHeight - 0.1f);
        Camera.main.transform.position = center + new Vector3(0, 1, -1) * 1;
        Camera.main.transform.LookAt(center, Vector3.up);

        Vector3 testCenter = BlockUtilities.GetMathematicalPosition(map_, w / 2, h / 2, 0);

        allMap = new BlockState[w, h];
		
        for (int x = 1; x < w - 1; x++)
        {
            for (int y = 1; y < h - 1; y++)
            {
                Block a = map.GetBlockAt(x, y, 0);
                if (a != null)
                {
                    allMap[x, y] = BlockState.DeadCube;
                    //		Debug.Log("x="+x+"  y="+y+" is not null");
                }
                else
                {
                    allMap[x, y] = BlockState.Empty;
                    //	Debug.Log("x="+x+"  y="+y+" is null");
                }
            }
        }
    }

    public void SpawnPlayer(int x, int y, string name)
    {
    }

    public void SpawnMonster(int x, int y, string name)
    {
    }

    public void SpawnBomb(int x, int y, string name)
    {

    }

}
