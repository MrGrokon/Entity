using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMover : MonoBehaviour
{
    public float CellSpeedValue = 100f;
    public enum CellState{Leader, Follower, Null};
    public enum CrossingState{None, Climb, Cross};

    public CellState MyState;
    public CrossingState MyCrossState;
    private Vector3 Trajectory;

    #region Physics Modifier
        public Vector3 GetMotionToPhysics(){
            return Trajectory * CellSpeedValue;
        }

        public Vector3 GetMotionToPhysics(Vector3 OptionalAdditiveVector3){
            return (Trajectory +OptionalAdditiveVector3) * CellSpeedValue;
        }
    #endregion

    #region Value Modifier Functions
        public void SetTarget(Vector3 _newTarget){
            Trajectory = _newTarget;
            if(MyState == CellState.Leader){
                Debug.DrawRay(this.transform.position, _newTarget, Color.red);
            }
            else if(MyState == CellState.Follower){
                Debug.DrawRay(this.transform.position, _newTarget, Color.blue);
            }
        }

        public void StopCell(){
            ChangeState(CellState.Null);

            Trajectory = Vector3.zero;
        }

        public void ChangeState(CellState _state){
            MyState = _state;
        }

        public void ChangeCrossState(CrossingState _state){
            MyCrossState = _state;
        }
    #endregion

    #region Value Returning Funtions
        public CrossingState GetCrossingState(){
            return MyCrossState;
        }

        public CellState GetState(){
            return MyState;
        }
    #endregion
}