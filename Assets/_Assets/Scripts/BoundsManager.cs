using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : Singleton<BoundsManager>
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float top;
    public float Top {
        get {return top;}
        private set {top = value;}
    }
    [SerializeField] private float bottom;
    public float Bottom {
        get {return bottom;}
        private set {bottom = value;}
    }
    [SerializeField] private float left;
    public float Left {
        get {return left;}
        private set {left = value;}
    }
    [SerializeField] private float right;
    public float Right {
        get {return right;}
        private set {right = value;}
    }

    void FixedUpdate() {
        Vector3 pos = player.transform.position;

        if (player.transform.position.x > right) {
            pos.x = left;
            player.transform.position = pos;
        }

        if (player.transform.position.x < left) {
            pos.x = right;
            player.transform.position = pos;
        }
        
        if (player.transform.position.y > top) {
            pos.y = bottom;
            player.transform.position = pos;
        }

        if (player.transform.position.y < bottom) {
            pos.y = top;
            player.transform.position = pos;
        }
        
    }

    void OnDrawGizmos() {
        Gizmos.DrawLine(new Vector2(left, top), new Vector2(right, top));
        Gizmos.DrawLine(new Vector2(right, top), new Vector2(right, bottom));  
        Gizmos.DrawLine(new Vector2(right, bottom), new Vector2(left, bottom)); 
        Gizmos.DrawLine(new Vector2(left, bottom), new Vector2(left, top));   
    }
}
