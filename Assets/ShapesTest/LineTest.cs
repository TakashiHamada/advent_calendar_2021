using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Line>().DashOffset = Time.time;

        GetComponent<Line>().End = Vector3.right * Time.time;

    }
}
