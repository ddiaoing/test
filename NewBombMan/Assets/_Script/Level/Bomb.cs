using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    Level level_;
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

    float mTime;
    bool[] frameStop = new bool[4];

    // if the bomb explode already
    bool bombAlready = false;

    public static string MakeName(int x, int y)
    {
        return "bomb_" + x + "_" + y;
    }

    void Awake()
    {
        level_ = Level.Instance;
    }

    // Use this for initialization
    void Start()
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
        FlameOn(posX, posY, 0);

        // create the flame 
        for (int i = 1; i <= mLength; i++)
        {
            //up
            if (frameStop[0] == false)
                frameStop[0] = FlameOn(posX, posY + i, i);

            //down
            if (frameStop[1] == false)
                frameStop[1] = FlameOn(posX, posY - i, i);

            //left
            if (frameStop[2] == false)
                frameStop[2] = FlameOn(posX - i, posY, i);

            //right
            if (frameStop[3] == false)
                frameStop[3] = FlameOn(posX + i, posY, i);
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
        Vector3 pos = level_.GetPositionAt(x, y);
        switch (level_[x, y])
        {
            case BlockState.Empty: // empty
                flame = Instantiate(flameObj, pos, Quaternion.identity) as GameObject;
                scp = flame.GetComponent<FlameBase>();
                scp.curLength = curLength;
                flame.transform.parent = this.transform;
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
