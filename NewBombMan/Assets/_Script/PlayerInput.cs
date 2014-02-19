using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Input.multiTouchEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnGUI()
	{
        Player player = Level.Instance.localPlayer_;
        if (player == null)
        {
            return;
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

    void On_JoystickMove(MovingJoystick move)
    {
        Control mJoystickCmd;
        GetCmd(move, out mJoystickCmd);
        Level.Instance.localPlayer_.SetControl(mJoystickCmd);
    }

    void GetCmd(MovingJoystick move, out Control mJoystickCmd)
    {     
        // x -1 ~ 1  left ~ right
        // y -1 ~ 1  down ~ up
        float baseDis = 0.05f;

        Vector2 dir = move.joystickAxis;

        mJoystickCmd = Control.None;

        float angle = JoystickAngle(dir);

        if (angle <= 15)
        {
            mJoystickCmd = Control.Up;
            return;
        }
        if (angle > 165)
        {
            mJoystickCmd = Control.Down;
            return;
        }

        if (dir.x >= 0)//right
        {
            if (angle > 15 && angle <= 45)
            {
                mJoystickCmd = Control.UpRight;
                return;
            }
            else if (angle > 45 && angle <= 75)
            {
                mJoystickCmd = Control.RightUp;
                return;
            }
            else if (angle > 75 && angle <= 105)
            {
                mJoystickCmd = Control.Right;
                return;
            }
            else if (angle > 105 && angle <= 135)
            {
                mJoystickCmd = Control.RightDown;
                return;
            }
            else if (angle > 135 && angle <= 165)
            {
                mJoystickCmd = Control.DownRight;
                return;
            }
        }
        else//left
        {
            if (angle > 15 && angle <= 45)
            {
                mJoystickCmd = Control.UpLeft;
                return;
            }
            else if (angle > 45 && angle <= 75)
            {
                mJoystickCmd = Control.LeftUp;
                return;
            }
            else if (angle > 75 && angle <= 105)
            {
                mJoystickCmd = Control.Left;
                return;
            }
            else if (angle > 105 && angle <= 135)
            {
                mJoystickCmd = Control.LeftDown;
                return;
            }
            else if (angle > 135 && angle <= 165)
            {
                mJoystickCmd = Control.DownLeft;
                return;
            }
        }


    }

    // Easy Touch Plugin Function
    void On_JoystickMoveEnd(MovingJoystick move)
    {
        Level.Instance.localPlayer_.SetControl(Control.None);
    }
    #endregion

}
