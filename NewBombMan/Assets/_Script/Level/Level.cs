using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DopplerInteractive.TidyTileMapper.Utilities;


public class Level : SingletonBehaviour<Level>
{
    public const float GRID_SIZE = 0.2f;
    public const float INV_GRID_SIZE = 5f;
    public const float HALF_GRID_SIZE = 0.1f;

    public int w, h;

    BlockState[,] allMap;

    BlockMap map_;

    public Player localPlayer_; //玩家自己

    List<GameObject> levelObjects_ = new List<GameObject>();

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

        float halfWidth = w * GRID_SIZE / 2;
        float halfHeight = h * GRID_SIZE / 2;

        Vector3 center = new Vector3(halfWidth - 1.5f * GRID_SIZE, 0, halfHeight - HALF_GRID_SIZE);
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
        basePosition.x = (w - 3) * GRID_SIZE;
        basePosition.z = (h - 3) * GRID_SIZE;
        basePosition.y = 0.0f;
        //to do init player position
    }

    public GameObject SpawnPlayer(int x, int y, string fileName)
    {
        GameObject obj = SpawnObject(x, y, fileName) as GameObject;
     //   obj.tag = "Player";
        if (localPlayer_ == null)
        {
            localPlayer_ = obj.GetComponent<Player>();
        }
        return obj;
    }

    public GameObject SpawnMonster(int x, int y, string fileName)
    {
        GameObject obj = SpawnObject(x, y, fileName) as GameObject;
    //    obj.tag = "Monster";
        return obj;
    }

    public GameObject SpawnBomb(int x, int y, string fileName)
    {
        GameObject obj = SpawnObject(x, y, fileName) as GameObject;
        obj.name = Bomb.MakeName(x, y);
    //    obj.tag = "Bomb";
        return obj;
    }

    public GameObject SpawnObject(int x, int y, string fileName)
    {
        GameObject res = Resources.Load(fileName) as GameObject;
        GameObject obj = Instantiate(res, GetPositionAt(x, y), Quaternion.identity) as GameObject;
        levelObjects_.Add(obj);
        return obj;
    }

    public void DestroyObject(GameObject obj)
    {
        levelObjects_.Remove(obj);
        Destroy(obj);
    }

    public void ClearBlock(int x, int y)
    {
        this[x, y] = BlockState.Empty;
        BlockUtilities.RemoveBlockFromMap(map_, x, y, 0, true, false);
    }

    public Vector3 GetPositionAt(int x, int y)
    {
        Vector3 pos = basePosition;
        //map scale = 0.2f
        pos.x += (x - 1) * -GRID_SIZE;
        pos.z += (y - 1) * -GRID_SIZE;

        return pos;
    }

    public void GetPoint(Vector3 pos, out int x, out int y)
    {
        x = (int)((basePosition.x - pos.x) * INV_GRID_SIZE) + 1;
        y = (int)((basePosition.z - pos.z) * INV_GRID_SIZE) + 1;
    }

    public List<GameObject> GetOverlapObject(int x, int y)
    {
        Vector3 pos = GetPositionAt(x, y);
        Rect rc = new Rect(pos.x - 0.1f, pos.z - 0.1f, 0.2f, 0.2f);
   
        List<GameObject> objList = new List<GameObject>();
        foreach (GameObject obj in levelObjects_)
        {
            Rect objRect = new Rect(obj.transform.position.x - HALF_GRID_SIZE, obj.transform.position.z - HALF_GRID_SIZE, GRID_SIZE, GRID_SIZE);
            if (objRect.Overlaps(rc))
            {
                objList.Add(obj);
            }
        }

        return objList;
    }

}
