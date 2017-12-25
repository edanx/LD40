using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILineParabola : MonoBehaviour {

    public LineRenderer Renderer;

    public Transform PositionA;

    public Transform PositionB;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        var p1 = PositionA.position;
        var p2 = PositionB.position;

        Vector3 dir = p2 - p1;
        float count = 20;
        Vector3 lastP = p1;

        List<Vector3> pos = new List<Vector3>();

        for (float i = 2; i < count ; i++)
        {
            Vector3 p = p1 + (dir / count) * i;
            p.y = Mathf.Sin((i / count) * Mathf.PI) * 1;
            //Gizmos.color = i % 2 == 0 ? Color.blue : Color.green;
            //Gizmos.DrawLine(lastP, p);
            lastP = p;

            pos.Add(p);
            //Renderer.SetPosition((int)i, p);
        }

        Renderer.SetPositions(pos.ToArray());
    }
}
