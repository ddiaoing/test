using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{
    protected Level level_;

    public int posX, posY;
    public Object flameObj;
    // at least 2
    public int length;

    // player property
    public bool isStrong;

    // is the bomb be hit by others
    public bool beHit;

    // bomb cd time
    float mBombTime = 5.0f;

    float mTime;
    bool[] frameStop = new bool[4];

    // if the bomb explode already
    bool bombAlready = false;

    public static string MakeName(int x, int y)
    {
        return "bomb_" + x + "_" + y;
    }

    protected virtual void Awake()
    {
        level_ = Level.Instance;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        level_.GetPoint(transform.position, out posX, out posY);
      
        for (int i = 0; i < frameStop.Length; i++)
        {
            frameStop[i] = false;
        }

        mTime = Time.time;

    }

    // Update is called once per flame
    void Update()
    {

        if ((Time.time - mTime > mBombTime || beHit == true)
            && bombAlready == false)
        {
            bombAlready = true;
            Explode();
         //   playerScript.curBomb -= 1;
            Destroy(this.gameObject, 1.5f);
        }
    }


    void Explode()
    {
        level_[posX, posY] = BlockState.Empty;

        // hide the bomb
        Hide();
        
        HashSet<GameObject> flamedObjects = new HashSet<GameObject>();
        FlameOn(posX, posY, 0, flamedObjects);


        // create the flame 
        for (int i = 1; i <= length; i++)
        {
            //up
            if (frameStop[0] == false)
                frameStop[0] = FlameOn(posX, posY + i, i, flamedObjects);

            //down
            if (frameStop[1] == false)
                frameStop[1] = FlameOn(posX, posY - i, i, flamedObjects);

            //left
            if (frameStop[2] == false)
                frameStop[2] = FlameOn(posX - i, posY, i, flamedObjects);

            //right
            if (frameStop[3] == false)
                frameStop[3] = FlameOn(posX + i, posY, i, flamedObjects);
        }

        foreach (GameObject obj in flamedObjects)
        {
            obj.SendMessage("OnFlamed", this, SendMessageOptions.DontRequireReceiver);
        }
    }


    /// <summary>
    /// Flames on.
    /// if the flame stop, return true
    /// </summary>
    bool FlameOn(int x, int y, int curLength, HashSet<GameObject> objects)
    {
        GameObject flame = null;
        FlameBase scp;
        Vector3 pos = level_.GetPositionAt(x, y);
        List<GameObject> list;

        switch (level_[x, y])
        {
            case BlockState.Empty: // empty
                flame = Instantiate(flameObj, pos, Quaternion.identity) as GameObject;
                scp = flame.GetComponent<FlameBase>();
                scp.curLength = curLength;
                flame.transform.parent = this.transform;

                list = level_.GetOverlapObject(x, y);
                objects.UnionWith(list);
                return false;

            case BlockState.DeadCube: // dead cube
                return true;

            case BlockState.Cube: // normal cube
                flame = Instantiate(flameObj, pos, Quaternion.identity) as GameObject;
                scp = flame.GetComponent<FlameBase>();
                scp.curLength = curLength;
                flame.transform.parent = this.transform;
                level_.ClearBlock(x, y);
                if (isStrong)
                    return false;
                return true;

            case BlockState.Bomb: // bomb here
                flame = Instantiate(flameObj, pos, Quaternion.identity) as GameObject;
                scp = flame.GetComponent<FlameBase>();
                scp.curLength = curLength;
                flame.transform.parent = this.transform;
                  
                list = level_.GetOverlapObject(x, y);
                objects.UnionWith(list);

                GameObject bomb = GameObject.Find(Bomb.MakeName(x, y));
                if (bomb != null)
                {
                    Bomb bob = bomb.GetComponent<Bomb>();
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
        foreach (Renderer render in allRender)
        {
            render.enabled = false;
        }


        // close the render
        // close the collider
        // and so on
    }
	
}
