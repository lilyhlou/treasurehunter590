using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateTarget : MonoBehaviour
{
    /*public float delta = 1.5f;  // Amount to move left and right from the start point
    public float speed = 2.0f; 
    private Vector3 startPos;*/
    // Start is called before the first frame update
    //Vector3 pointA = new Vector3(0, 0, 0);
    //Vector3 pointB = new Vector3(1, 1, 1);
    public Vector3 pointA;
    public Vector3 pointB;
    void Start()
    {
        //setPoints2(new Vector3(0, 0, 0),new Vector3(1, 1, 1));
        //startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 v = startPos;
         v.x += delta * Mathf.Sin (Time.time * speed);
         transform.position = v;*/
         transform.position = Vector3.Slerp(pointA, pointB, Mathf.PingPong(Time.time, 1));
    }

    public void setPoints2(Vector3 A, Vector3 B){
        pointA = A;
        pointB = B;
    }

    public void setPoints(Vector3[] points){
        pointA = points[0];
        pointB = points[1];
    }
}