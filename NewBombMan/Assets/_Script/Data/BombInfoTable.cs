using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]
public class BombInfo
{
    public int id;
    public int level;
    public int lv_up_exp;
    public int leadership;
    public int max_power;
    public int max_friend;

}

[System.Serializable]
public class BombInfoTable : DataList<BombInfo>
{
    public BombInfo GetById(int id)
    {
        foreach (BombInfo ri in elements)
        {
            if (ri.id == id)
            {
                return ri;
            }
        }

        return null;
    }

    public BombInfo GetByLevel(int level)
    {
        foreach (BombInfo ri in elements)
        {
            if (ri.level == level)
            {
                return ri;
            }
        }

        return null;
    }
}