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