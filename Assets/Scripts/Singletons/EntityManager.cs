using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class EntityManager : MonoBehaviour
{
    public enum EntityState {Moving, Climbing,Crossing, Static};

    public static EntityManager Instance;
    public List<GameObject> Cells = new List<GameObject>();

    private Vector3 _entityPosition;
    private EntityState MyState;

    #region Unity Functions
        private void Awake() {
            #region Singleton Instance
            if(Instance == null){
                Instance = this;
            }
            else{
                Destroy(this);
            }
            #endregion
        }

        private void Update() {
            _entityPosition = GetEntityMasterPosition();
        }
    #endregion

    #region  Debug
        private void OnDrawGizmos() {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_entityPosition, 0.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetCell_X(true), 0.5f);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(GetCell_X(false), 0.5f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetCell_Z(true), 0.5f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(GetCell_Z(false), 0.5f);
        }
    #endregion

    #region Data Managing Functions
        public void ChangeEntityState(EntityState _state){
            MyState = _state;
        }

        public void AddCell(GameObject _cell){
            //fonction qui ajoute une cellule à l'entité, ci elle n'est pas déja comprise dans cette dernière
            if(!Cells.Contains(_cell)){
                Cells.Add(_cell);
            }
        }
        
        public void RemoveCell(GameObject _cell){
            if(Cells.Contains(_cell)){
                Cells.Remove(_cell);
            }
        }
    #endregion

    #region Data Returning Functions
        public EntityState GetEntityState(){
            return MyState;
        }

        public GameObject GetLeaderCell(Vector3 _dir, int nmbOfCellMargin = 3){
            //float _compareAngle = Vector3.Angle(GetEntityMasterPosition(), _dir);

            //Target -> cell
            //Position -> master
            //Target-Position -> comparer a _dir
            //Vector3.Angle(_dir, Target - Positon)

            //remplacer Vector3.Angle() par Vector3.Dot();
            //GameObject[] Cells_orderedByAngle = GetAllCells().OrderBy(x => Mathf.Abs(_compareAngle - Vector3.Angle(GetEntityMasterPosition(), x.transform.position))).ToArray();
            GameObject[] Cells_orderedByAngle = GetAllCells().OrderBy(x => Vector3.Angle(_dir, x.transform.position - GetEntityMasterPosition())).ToArray<GameObject>();
            GameObject FarestCellOfAngle = Cells_orderedByAngle[0];
            for (int i = 0; i < nmbOfCellMargin; i++)
            {
                if(Vector3.Distance(GetEntityMasterPosition(), Cells_orderedByAngle[i].transform.position) > Vector3.Distance(GetEntityMasterPosition(), FarestCellOfAngle.transform.position)){
                    FarestCellOfAngle = Cells_orderedByAngle[i];
                }
            }
            return FarestCellOfAngle;
        }
  

        public GameObject[] GetAllCells(){
            return Cells.ToArray();
        }

        public int GetCellsCount(){
            return Cells.Count;
        }

        public Vector3 GetEntityMasterPosition(){
            //TODO: we may add an weight systme ?
            Vector3 EntityEstimatedPosition = Vector3.zero;
            if(GetCellsCount() > 0){
                EntityEstimatedPosition += GetCell_X(true);
                EntityEstimatedPosition += GetCell_X(false);
                EntityEstimatedPosition += GetCell_Z(true);
                EntityEstimatedPosition += GetCell_Z(false);
                EntityEstimatedPosition = EntityEstimatedPosition/4;
            }
            else{
                Debug.Log("No Cell in Entity");
            }
            return EntityEstimatedPosition;
        }

        private Vector3 GetCell_X(bool _max){
            GameObject[] Ordered_Cells = GetAllCells().OrderBy(x => x.transform.position.x).ToArray();
            int MaxArraySize = Ordered_Cells.Length;
            if(_max){
                return Ordered_Cells[MaxArraySize-1].transform.position;
            }
            else{
                return Ordered_Cells[0].transform.position;
            }
        }

        private Vector3 GetCell_Z(bool _max){
            GameObject[] Ordered_Cells = GetAllCells().OrderBy(x => x.transform.position.z).ToArray();
            int MaxArraySize = Ordered_Cells.Length;
            if(_max){
                return Ordered_Cells[MaxArraySize-1].transform.position;
            }
            else{
                return Ordered_Cells[0].transform.position;
            }
        }
    #endregion
}
