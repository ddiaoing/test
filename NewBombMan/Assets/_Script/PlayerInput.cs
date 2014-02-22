using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    public bool useKeyboard = true;

	// Use this for initialization
	void Start () {
        Input.multiTouchEnabled = true;

#if (UNITY_ANDROID || UNITY_IPHONE)
        useKeyboard = false;
#endif
    }
	
	// Update is called once per frame
	void Update () {

        if (useKeyboard && !beginJoystick)
        {
            Player player = Level.Instance.localPlayer_;
            if (player == null)
            {
                return;
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (h > 0)
            {
                if (v > 0)
                {
                    player.SetControl(Control.RightUp);
                }
                else if (v == 0)
                {
                    player.SetControl(Control.Right);
                }
                else
                {
                    player.SetControl(Control.RightDown);
                }
            }
            else if (h == 0)
            {
                if (v > 0)
                {
                    player.SetControl(Control.Up);
                }
                else if (v == 0)
                {
                    player.SetControl(Control.None);
                }
                else
                {
                    player.SetControl(Control.Down);
                }
            }
            else
            {
                if (v > 0)
                {
                    player.SetControl(Control.LeftUp);
                }
                else if (v == 0)
                {
                    player.SetControl(Control.Left);
                }
                else
                {
                    player.SetControl(Control.LeftDown);
                }
            }
        }
	}

	
	void OnGUI()
	{
        Player player = Level.Instance.localPlayer_;
        if (player == null)
        {
            return;
        }

        if (useKeyboard)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                player.SetBomb();
            }

            
        }
  
		if (GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.7f, Screen.width * 0.1f, Screen.width * 0.1f), "Bomb"))
		{
            player.SetBomb();
		}
	}

    #region  Easy Touch

    float JoystickAngle(Vector2 pos)
    {
        float angle = Vector2.Angle(pos, Vector2.up);
     //   Debug.Log(angle);
        return angle;
    }
 
    // Easy Touch Plugin Function
    bool beginJoystick;
    void On_JoystickMove(MovingJoystick move)
    {
        Player player = Level.Instance.localPlayer_;
        if (player == null)
        {
            return;
        }
        beginJoystick = true;
        Control control;
        GetCmd(move, out control);
        player.SetControl(control);
    }

    void GetCmd(MovingJoystick move, out Control control)
    {     
        // x -1 ~ 1  left ~ right
        // y -1 ~ 1  down ~ up
        float baseDis = 0.05f;

        Vector2 dir = move.joystickAxis;

        control = Control.None;

        float angle = JoystickAngle(dir);

        if (angle <= 15)
        {
            control = Control.Up;
            return;
        }
        if (angle > 165)
        {
            control = Control.Down;
            return;
        }

        if (dir.x >= 0)//right
        {
            if (angle > 15 && angle <= 45)
            {
                control = Control.UpRight;
                return;
            }
            else if (angle > 45 && angle <= 75)
            {
                control = Control.RightUp;
                return;
            }
            else if (angle > 75 && angle <= 105)
            {
                control = Control.Right;
                return;
            }
            else if (angle > 105 && angle <= 135)
            {
                control = Control.RightDown;
                return;
            }
            else if (angle > 135 && angle <= 165)
            {
                control = Control.DownRight;
                return;
            }
        }
        else//left
        {
            if (angle > 15 && angle <= 45)
            {
                control = Control.UpLeft;
                return;
            }
            else if (angle > 45 && angle <= 75)
            {
                control = Control.LeftUp;
                return;
            }
            else if (angle > 75 && angle <= 105)
            {
                control = Control.Left;
                return;
            }
            else if (angle > 105 && angle <= 135)
            {
                control = Control.LeftDown;
                return;
            }
            else if (angle > 135 && angle <= 165)
            {
                control = Control.DownLeft;
                return;
            }
        }


    }

    // Easy Touch Plugin Function
    void On_JoystickMoveEnd(MovingJoystick move)
    {
        Player player = Level.Instance.localPlayer_;
        if (player == null)
        {
            return;
        }
        beginJoystick = false;
        player.SetControl(Control.None);
    }
    #endregion

}
