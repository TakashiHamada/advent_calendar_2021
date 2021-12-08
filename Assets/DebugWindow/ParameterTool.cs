using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/ParameterTool")]
public class ParameterTool: ScriptableObject
{
    [PropertyOrder(1)]
    [HideLabel] [DisplayAsString] public string message;

    [FormerlySerializedAs("paramGeneral")] [PropertyOrder(99)] [AssetsOnly] [Required]
    public GeneralParameter generalParameter;

    private void Request(string key)
    {
        var access = new Dictionary<string, string>()
        {
            {
                "GetTestValues",
                "https://script.google.com/macros/s/AKfycbxXDOS7W9ChPktR3UFh8wf_4Phdadblpjn6Hg9OXwNVB5sSAGZUc9C8uhvB2Ab3JBrg/exec?mode=test"
            }
        };

        // --
        // UnityWebRequestを使わないのは、staticで使えないから
        ObservableWWW.Get(access[key])
            .Subscribe(x =>
            {
                this.message = "Done!";
                
                var data = JsonUtility.FromJson<WebData>(x);
                this.generalParameter.param_0 = data.key_0;
                this.generalParameter.param_1 = data.key_1;

                Observable.Timer(TimeSpan.FromSeconds(2f))
                    .Subscribe(_ => this.message = "");
            }, ex => this.message = ex.ToString());
    }
    
    [PropertyOrder(0)] [LabelText("Spreadsheetを反映")]
    [Button] [GUIColor(0, 1, 1)]
    private void UpdateSpeechScore()
    {
        string alertText = "GeneralParameterを更新します";
        if (!EditorUtility.DisplayDialog("実行する？", alertText, "OK", "Cancel")) return;
        
        this.message = "loading";
        Observable.Interval(TimeSpan.FromSeconds(0.25f))
            .Take(10) // はみ出さないように
            .TakeWhile(_ => this.message != "Done!")
            .Subscribe(_ => this.message += "."); // 動いている演出
        
        Request("GetTestValues"); // Webから取ってくる
    }
}

[Serializable]
class WebData
{
    public string key_0;
    public string key_1;
}
