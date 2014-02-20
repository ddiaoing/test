using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DopplerInteractive.TidyTileMapper.Utilities;


public class Level : SingletonBehaviour<Level>
{
    BlockState[,] allMap;
    int w, h;
    BlockMap map_;

    public Player localPlayer_; //玩家自己
    List<GameObject> players_ = new List<GameObject>();  //网络玩家或者ai
    List<GameObject> monsters_ = new List<GameObject>(); //怪物
    List<GameObject> bombs_ = new List<GameObject>();
    List<GameObject> items_ = new List<GameObject>();

    Vector3 basePosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public BlockState this[int x, int y]
    {
        get
        {
            return allMap[x, y];
        }
        set
        {
            allMap[x, y] = value;
        }
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

		Vector3 center = new Vector3(halfWidth - 0.3f, 0, halfHeight - 0.1f);
		Camera.main.fieldOfView = 45;
        Camera.main.transform.position = center + new Vector3(0, 1.732f, 1.0f) * 1;
        Camera.main.transform.LookAt(center, Vector3.up);
		

		allMap = new BlockState[w, h];
		
        for (int x = 1; x < w - 1; x++)
        {
            for (int y = 1; y < h - 1; y++)
            {
                Block a = map.GetBlockAt(x, y, 0);
                if (a != null)
                {
                    allMap[x, y] = BlockState.DeadCube;
                    //Debug.Log("x="+x+"  y="+y+" is not null");
                }
                else
                {
                    allMap[x, y] = BlockState.Empty;
                    //	Debug.Log("x="+x+"  y="+y+" is null");
                }
            }
        }

    //    Block block = map_.GetBlockAt(1, 1, 0);
     //   basePosition = block.transform.position;
        basePosition.x = w * 0.2f - 0.6f;
        basePosition.z = h * 0.2f - 0.6f;
        basePosition.y = 0.0f;
        //to do init player position
    }

    public Vector3 GetPositionAt(int x, int y)
    {
        Vector3 pos = basePosition;
        //map scale = 0.2f
        pos.x += (x - 1) * -0.2f;
        pos.z += (y - 1) * -0.2f;

        return pos;
    }

    public void GetPoint(Vector3 pos, out int x, out int y)
    {
        x = (int)((basePosition.x - pos.x) * 5) + 1;
        y = (int)((basePosition.z - pos.z) * 5) + 1;
    }

    public GameObject SpawnPlayer(int x, int y, string name)
    {
        GameObject res = Resources.Load(name) as GameObject;
        GameObject obj = Instantiate(res, GetPositionAt(x, y), Quaternion.identity) as GameObject;
        players_.Add(obj);

        if (localPlayer_ == null)
        {
            localPlayer_ = obj.GetComponent<Player>();
        }
        return obj;
    }

    public GameObject SpawnMonster(int x, int y, string name)
    {
        GameObject res = Resources.Load(name) as GameObject;
        GameObject obj = Instantiate(res, GetPositionAt(x, y), Quaternion.identity) as GameObject;
        monsters_.Add(obj);
        return obj;
    }

    public GameObject SpawnBomb(int x, int y, string name)
    {
        GameObject res = Resources.Load(name) as GameObject;
        GameObject obj = Instantiate(res, GetPositionAt(x, y), Quaternion.identity) as GameObject;
        obj.name = Bomb.MakeName(x, y);
        bombs_.Add(obj);
        return obj;
    }

    public void ClearBlock(int x, int y)
    {
        this[x, y] = BlockState.Empty;
        BlockUtilities.RemoveBlockFromMap(map_, x, y, 0, true, false);
    }
}
