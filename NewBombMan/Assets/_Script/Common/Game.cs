using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;


public class Game : MonoBehaviour
{
    static GameObject rootObject_;
    static Game instance_ = null;

    public static bool HasInstance
    {
        get { return instance_ != null; }
    }

    public static Game Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = new GameObject("GameRoot").AddComponent<Game>();
            }

            return instance_;
        }
    }

    GameScreen screenManager_;
    public static GameScreen ScreenManager
    {
        get { return instance_.screenManager_; }
    }

    public static T AddComponent<T>() where T : Component
    {
        return rootObject_.AddComponent<T>();
    }

    public static T Get<T>() where T : MonoBehaviour
    {
        return rootObject_.GetComponent<T>();
    }

    void Awake()
    {
        rootObject_ = gameObject;
        DontDestroyOnLoad(rootObject_);

        screenManager_ = AddComponent<ScreenRoot>();

    }
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    #region Screen Fade
    Color screenColor = new Color(0, 0, 0, 0);
    public Color ScreenColor
    {
        get 
        {
            return screenColor; 
        }

        set 
        {
            screenColor = value;
            fadeAlpha = screenColor.a;
        }
    }

    float fadeState = 0; // 1 fade out£¬ -1 fade in
    float fadeAlpha = 0.0f;

    void OnGUI()
    {
        if (fadeState != 0)
        {
            fadeAlpha += Time.deltaTime * fadeState;

            if (fadeState > 0 && fadeAlpha >= 1.0f)
            {
                fadeState = 0;
                fadeAlpha = 1.0f;
            }

            if (fadeState < 0 && fadeAlpha <= 0.0f)
            {
                fadeState = 0;
                fadeAlpha = 0.0f;
            }

        }

        if (fadeAlpha > 0)
        {
            Rect position = new Rect(0, 0, Screen.width, Screen.height);
            screenColor.a = fadeAlpha;
            Utils.DrawRectangle(position, screenColor);
        }
    }

    public void FadeInScreen(float time)
    {
        fadeState = -1 / time;
    }

    public void FadeOutScreen(float time)
    {
        fadeState = 1 / time;
    }

    #endregion
}
