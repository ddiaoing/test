using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
    protected StateMachine<Actor> stateManager_;
    protected Level level_;

    void Awake()
    {
        level_ = Level.Instance;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
