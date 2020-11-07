using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityControler : MonoBehaviour
{
    public int NomberOfCellToMarginLeader = 3;

    #region Unity Functions
        private void Awake() {
            EntityManager.Instance.ChangeEntityState(EntityManager.EntityState.Static);
        }

        private void Update() {
            Vector3 Joystick_Dir = new Vector3(Input.GetAxis("Left_Vertical"), 0f, Input.GetAxis("Left_Horizontal")).normalized;
            if(Joystick_Dir != Vector3.zero){
                Debug.Log("Left Joystick moved");
                Debug.DrawRay(EntityManager.Instance.GetEntityMasterPosition(), Joystick_Dir, Color.green);
                EntityManager.Instance.ChangeEntityState(EntityManager.EntityState.Moving);
                MoveEveryone(Joystick_Dir);
            }
            else{
                Debug.Log("Left Joystick used anymore");
                EntityManager.Instance.ChangeEntityState(EntityManager.EntityState.Static);
                StopEveryone();
            }
        }
    #endregion

    #region Movement Functions
        private void MoveEveryone(Vector3 Joystick_Dir){
            GameObject _leader = EntityManager.Instance.GetLeaderCell(Joystick_Dir, NomberOfCellToMarginLeader);
            foreach(var _cell in EntityManager.Instance.GetAllCells()){
                CellMover _mover = _cell.GetComponent<CellMover>();
                if(_cell == _leader){
                    _mover.ChangeState(CellMover.CellState.Leader);
                    _mover.SetTarget(Joystick_Dir);
                }
                else{
                    _mover.ChangeState(CellMover.CellState.Follower);
                    Vector3 _deducedDirection = (_leader.transform.position - _cell.transform.position).normalized;
                    _mover.SetTarget(_deducedDirection);
                }
            }
        }

        private void StopEveryone(){
            foreach(var _cell in EntityManager.Instance.GetAllCells()){
                _cell.GetComponent<CellMover>().StopCell();
            }
        }
    #endregion 
}
