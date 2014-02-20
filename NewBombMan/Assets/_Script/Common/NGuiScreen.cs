using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NGuiScreen : GameScreen
{
    protected GameObject uiPrefab;
    protected GameObject uiObject;
    protected string uiFile;
    protected bool freeOnExit = false;

    public NGuiScreen(string uifile = "")
    {
        uiFile = uifile;
    }

    public GameObject UIObject
    {
        get
        {
            return uiObject;
        }
        set
        {
            if (uiObject != value)
            {
                uiObject = value;
                OnLoadUI();
            }

        }
    }
    
    public T FindUIObject<T>(string name) where T : MonoBehaviour
    {
        Transform obj = FindChild(name, uiObject.transform);
        return obj.GetComponent<T>();
    }

    public Transform Find(string name)
    {
        Transform obj = FindChild(name, uiObject.transform);
        return obj;
    }

    Transform FindChild(string name, Transform t)
    {
        if (t.name == name)
            return t;

        foreach (Transform child in t)
        {
            Transform ct = FindChild(name, child);
            if (ct != null)
                return ct;
        }
        return null;
    }

    public override void OnInit()
    {
        base.OnInit();

        if (!string.IsNullOrEmpty(uiFile))
        {
            uiPrefab = Resources.Load(uiFile) as GameObject;
            uiObject = GameObject.Instantiate(uiPrefab) as GameObject;
            uiObject.SetActive(false);
            OnLoadUI();
        }
    }

    public override void OnEnter(object param)
    {
        base.OnEnter(param);
       
        if (uiObject == null)
        {
            if (!string.IsNullOrEmpty(uiFile))
            {
                uiPrefab = Resources.Load(uiFile) as GameObject;
                uiObject = GameObject.Instantiate(uiPrefab) as GameObject;
                OnLoadUI();
            }
        }
        
        if (uiObject != null)
        {
            uiObject.SetActive(true);
        }
    }


    public override void OnExit()
    {
        base.OnExit();

        if (uiObject != null)
        {
            uiObject.SetActive(false);
        }

        if (freeOnExit)
        {
            OnUnloadUI();
            Resources.UnloadAsset(uiPrefab);
            GameObject.Destroy(uiObject);
        }
    }

    protected virtual void OnLoadUI()
    {

    }

    protected virtual void OnUnloadUI()
    {

    }
}
