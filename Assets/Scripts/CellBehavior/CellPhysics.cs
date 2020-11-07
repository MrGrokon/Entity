using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPhysics : MonoBehaviour
{
    public bool DebugMode = false;

    private CellMover _Mover;
    private Rigidbody _Rb;
    public Vector3 NormalOfContact;

    #region Init
        private void Awake() {
            _Mover = this.GetComponent<CellMover>();
            _Rb = this.GetComponent<Rigidbody>();
        }

        void Start()
        {
            EntityManager.Instance.AddCell(this.gameObject);
        }
    #endregion
    
    #region Unity Function
        void Update()
        {
            if(!DebugMode){
                switch (_Mover.GetCrossingState())
                {
                    case CellMover.CrossingState.None:
                    _Rb.AddForce(_Mover.GetMotionToPhysics() * Time.deltaTime);
                    break;

                    case CellMover.CrossingState.Climb:
                    //remplacer cela par une application du movement projeter sur la surfaace
                    _Rb.AddForce(Vector3.ProjectOnPlane(_Mover.GetMotionToPhysics(Vector3.up), NormalOfContact) * Time.deltaTime);
                    
                    //_Rb.AddForce((_Mover.GetMotionToPhysics(Vector3.up)) * Time.deltaTime);
                    break;

                    default:
                    break;
                }
            }
        }

        private void OnCollisionStay(Collision col) {
            if(col.gameObject.tag != "Cell" && col.gameObject.tag != "Ground"){
                //if i touche something thats not a cell nor a ground
                Ray _ray = new Ray(this.transform.position, _Mover.GetMotionToPhysics().normalized);
                if(Physics.Raycast(_ray,out RaycastHit _hit, 1f) && _hit.collider.gameObject == col.gameObject){
                    Debug.Log("I climb");
                    NormalOfContact = col.contacts[0].normal;
                    _Mover.ChangeCrossState(CellMover.CrossingState.Climb);
                }
                else{
                    _Mover.ChangeCrossState(CellMover.CrossingState.None);
                }
            }
        }
    #endregion
}
