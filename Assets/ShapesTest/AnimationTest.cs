using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public class AnimationTest : ImmediateModeShapeDrawer
{
    public Camera cam;
    
    void Start()
    {
    }

    void Update()
    {
    }


    public override void DrawShapes(Camera cam)
    {
        if (cam != this.cam) // only draw in the player camera
            return;

        using (Draw.Command(cam))
        {
            Draw.Line(Vector3.zero, Vector3.left);
            
        }
    }
}