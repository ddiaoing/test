using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameScreen : MonoBehaviour
{
    public List<GameScreen> gameScreenStack_ = new List<GameScreen>();
    Dictionary<Type, GameScreen> gameScreenDict_ = new Dictionary<Type, GameScreen>();
    public GameScreen parent;
    protected bool inited = false;
    public GameScreen()
    {
    }

    public virtual void OnInit()
    {
        inited = true;
    }

    public virtual void OnEnter(object param)
    {
        gameObject.SetActive(true);
        enabled = true;

        GameScreen gs = CurrentScreen;
        if (gs != null)
        {
            gs.OnEnter(param);
        }
        Debug.Log("enter screen " + GetType().Name);
    }

    public virtual void OnExit()
    {
        Debug.Log("exit screen " + GetType().Name);

        ChangeScreen(null);
        /*
        GameScreen gs = CurrentScreen;
        if (gs != null)
        {
            gs.OnExit();
        }
        */
        gameObject.SetActive(false);
        enabled = false;
    }

    public virtual void OnGUI()
    {
#if TEST_GUI
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, 10, 400, 50));
        GUILayout.TextArea(GetType().Name);
        GUILayout.EndArea();
#endif
    }

    public T AddScreen<T>() where T : GameScreen
    {
        GameObject obj = new GameObject();
        obj.SetActive(false);
        GameScreen screen = obj.AddComponent<T>();
        screen.enabled = false;
        obj.transform.parent = gameObject.transform;
        screen.parent = this;
        obj.name = typeof(T).Name;
        gameScreenDict_[typeof(T)] = screen;
        screen.OnInit();

        return screen as T;
    }

    public void ChangeScreen(Type name, object param = null)
    {
        GameScreen gs = CurrentScreen;
        if (gs != null)
        {
            if (gs.GetType() == name)
            {
                return;
            }

            gs.OnExit();
        }
        else if (name == null)
        {
            return;
        }

        gameScreenStack_.Clear();

        if (name != null)
        {
            GameScreen gameScreen = null;
            if (gameScreenDict_.TryGetValue(name, out gameScreen))
            {
                gameScreenStack_.Add(gameScreen);
                gameScreen.OnEnter(param);
            }
        }


        OnScreenChanged();
    }

    public void PushScreen(Type name, object param = null)
    {
        GameScreen gs = CurrentScreen;
        if (gs != null)
        {
            if (gs.GetType() == name)
            {
                return;
            }

            gs.OnExit();
        }

        GameScreen gameScreen = null;
        if (gameScreenDict_.TryGetValue(name, out gameScreen))
        {
            gameScreenStack_.Add(gameScreen);
            gameScreen.OnEnter(param);
        }

        OnScreenChanged();
    }

    public void PopScreen(object param = null)
    {
        if (CurrentScreen != null)
        {
            CurrentScreen.OnExit();
        }
        else
            return;

        if (gameScreenStack_.Count > 0)
        {
            gameScreenStack_.RemoveAt(gameScreenStack_.Count - 1);
        }

        if (CurrentScreen != null)
        {
            CurrentScreen.OnEnter(param);
        }

        OnScreenChanged();
    }

    public void OnBack()
    {
        parent.PopScreen();
    }

    public GameScreen CurrentScreen
    {
        get
        {
            if (gameScreenStack_.Count == 0)
            {
                return null;
            }

            return gameScreenStack_[gameScreenStack_.Count - 1];
        }

    }

    protected virtual void OnScreenChanged()
    {
    }
}


public class ScreenRoot : GameScreen 
{
    public override void OnGUI()
    {
    }
}
