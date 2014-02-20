﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]
public class BombInfo
{
    public int id;
    public int length;
    public int attack;
    public string model_file;


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


}