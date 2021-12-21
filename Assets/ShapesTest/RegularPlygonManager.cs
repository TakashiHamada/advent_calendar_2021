using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

[ExecuteAlways]
public class RegularPlygonManager : ImmediateModeShapeDrawer
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
// Sets up all static parameters.
// These are used for all following Draw.Line calls Draw.LineGeometry = LineGeometry.Volumetric3D; Draw.ThicknessSpace = ThicknessSpace.Pixels; Draw.Thickness = 4; // 4px wide
// draw lines
            Draw.Line(Vector3.zero, Vector3.right, Color.red);
            Draw.Line(Vector3.zero, Vector3.up * 5, Color.green);
            Draw.Line(Vector3.zero, Vector3.forward, Color.blue);
        }
    }
}