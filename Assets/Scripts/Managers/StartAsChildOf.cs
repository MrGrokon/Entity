using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAsChildOf : MonoBehaviour
{
    public string ParentName;
    private void Awake() {
        GameObject Parent = GameObject.Find(ParentName);
        if(Parent != null || this.transform.parent != Parent){
            this.transform.SetParent(Parent.transform);
        } 
    }
}
