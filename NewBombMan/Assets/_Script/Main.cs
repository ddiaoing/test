using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;


public class Main : MonoBehaviour
{

    public bool enableNet;
    static GameObject uiRoot;

    void Awake()
    {
        //create game instance
        if (!Game.HasInstance)
        {
            Game instance = Game.Instance;

            Game.AddComponent<Level>();
        //    Game.AddComponent<DataCenter>();

       //     Game.ScreenManager.AddScreen<MenuScreen>();

            Game.ScreenManager.AddScreen<GamePlay>();
       /*
            uiRoot = GameObject.Find("UI Root (3D)");

            if (uiRoot != null)
            {
                GameObject.DontDestroyOnLoad(uiRoot);
            }
            else
            {
                Debug.LogError("cannot find ui root object");
            }
          */

            Game.ScreenManager.ChangeScreen(typeof(GamePlay));

        }
    }

    IEnumerator Start()
    {
        yield return null;
    }



}


