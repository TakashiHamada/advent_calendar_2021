using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    void Awake()
    {
        foreach (Transform child in this.transform)
        {
            // すべての子供を非アクティブに
            child.gameObject.SetActive(false);
            // 中央に集結
            child.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
    
    void Start()
    {
    }

    public void ActivateSceneByIdx(int idx)
    {
        foreach (Transform child in this.transform)
            child.gameObject.SetActive(false);
        // --
        this.transform.GetChild(idx).gameObject.SetActive(true);
    }
}
