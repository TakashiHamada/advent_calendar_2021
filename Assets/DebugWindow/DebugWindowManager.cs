using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DebugWindowManager : MonoBehaviour
{
    [SerializeField] [ChildGameObjectsOnly]
    private Text param_0, param_1;

    [SerializeField] [AssetsOnly] private GeneralParameter param;
    void Start()
    {
    }

    void Update()
    {
        param_0.text = param.param_0;
        param_1.text = param.param_1;
    }
}
