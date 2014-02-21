using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



[System.Serializable]
public class MonsterInfo
{
    public int id;
    public bool isboss;
    public int attack;
    public string model_file;

    public string anim_idle;
    public string anim_walk;
    public string anim_attack;
    public string anim_die;
}

[System.Serializable]
public class MonsterInfoTable : DataList<MonsterInfo>
{
    public MonsterInfo GetById(int id)
    {
        foreach (MonsterInfo ri in elements)
        {
            if (ri.id == id)
            {
                return ri;
            }
        }

        return null;
    }


}