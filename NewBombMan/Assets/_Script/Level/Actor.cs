using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
    protected StateMachine<Actor> stateManager_;
    protected Level level_;

    public int posX, posY;

    public float speed = 0.5f;
    public float wallWidth = 0.2f;

    public bool walkingInWall;

    protected BlockState curValue;
    protected BlockState upValue;
    protected BlockState downValue;
    protected BlockState leftValue;
    protected BlockState rightValue;
    protected BlockState upLeftValue;
    protected BlockState upRightValue;
    protected BlockState downLeftValue;
    protected BlockState downRightValue;
    

    public enum BM_Dir
    {
        Up,
        Down,
        Left,
        Right,
        None,
    };

    public BM_Dir mDir = BM_Dir.Down;


    protected virtual void Awake()
    {
        level_ = Level.Instance;
    }
	// Use this for initialization
    protected virtual void Start()
    {
        level_.GetPoint(transform.position, out posX, out posY);
        GetCurrentPositionInfo();
	}

    #region tool function

    protected void TurnDirection(BM_Dir dir)
    {
        switch (dir)
        {
            case BM_Dir.Up:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 180, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;

            case BM_Dir.Down:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 0, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case BM_Dir.Left:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 90, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;

            case BM_Dir.Right:
                if (this.transform.rotation != Quaternion.Euler(new Vector3(0, 270, 0)))
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                break;
        }
		mDir = dir;
    }


    protected bool IsAlllowToPass(BlockState wallValue)
    {
        //todo  whether the user can pass the wall
        if (wallValue == BlockState.Empty)
            return true;
        else
        {
            if (wallValue == BlockState.Cube && walkingInWall)
                return true;
            return false;
        }
    }

    protected void GetCurrentPositionInfo()
    {
        curValue = level_[posX, posY];

        upValue = level_[posX, posY + 1];

        downValue = level_[posX, posY - 1];

        leftValue = level_[posX - 1, posY];

        rightValue = level_[posX + 1, posY];

        upLeftValue = level_[posX - 1, posY + 1];

        upRightValue = level_[posX + 1, posY + 1];

        downLeftValue = level_[posX - 1, posY - 1];

        downRightValue = level_[posX + 1, posY - 1];
    }

    protected void UpdatePosition()
    {
        Vector3 curWallPos = level_.GetPositionAt(posX, posY);

        if (this.transform.position.x > curWallPos.x + wallWidth * 0.5f)
        {
            posX -= 1;
            GetCurrentPositionInfo();
        }
        else if (this.transform.position.x < curWallPos.x - wallWidth * 0.5f)
        {
            posX += 1;
            GetCurrentPositionInfo();
        }
        else if (this.transform.position.z > curWallPos.z + wallWidth * 0.5f)
        {
            posY -= 1;
            GetCurrentPositionInfo();
        }
        else if (this.transform.position.z < curWallPos.z - wallWidth * 0.5f)
        {
            posY += 1;
            GetCurrentPositionInfo();
        }
    }


    #endregion

}
