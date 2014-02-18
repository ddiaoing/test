using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Singleton<T> where T : new()
{
    static T instance;

    public static void createInstance()
    {
        if (instance == null)
        {
            try
            {
                instance = new T();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                createInstance();
            }

            return instance;
        }
    }
}

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static MonoBehaviour instance;
    public SingletonBehaviour()
    {
        instance = this;
    }

    public static T Instance
    {
        get { return instance as T ; }
    }

}

