using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;
using UniRx;

[CreateAssetMenu(menuName = "Param/Parameters")]
public class Parameters: ScriptableObject
{

    [PropertyOrder(2)]
    [HideLabel] [DisplayAsString] public string message;
    
    [PropertyOrder(2)]
    [InfoBox("【元栓】デバッグ機能を有効にするか")] public bool DebugMode;

    [EnumToggleButtons] [ShowIf("DebugMode")] [InfoBox("プロモードにするか？ONにした場合、ずっとプロモード")]
    public bool DebugProMode;
    
    [FormerlySerializedAs("initCutIdx")] [EnumToggleButtons] [ShowIf("DebugMode")] [InfoBox("初期CutIdx, Speechから始める場合")]
    public int debugInitCutIdx = -1;
    
    [EnumToggleButtons] [ShowIf("DebugMode")] [InfoBox("速度レベル, 0, 1, 2 => Max")]
    public int debugSpeedLevel = 0;


    [PropertyOrder(99)] [AssetsOnly] [Required]
    public ParamGeneral paramGeneral;

    private void Request(string key)
    {
        var access = new Dictionary<string, string>()
        {
            {
                "Test",
                "https://script.google.com/macros/s/AKfycbxXDOS7W9ChPktR3UFh8wf_4Phdadblpjn6Hg9OXwNVB5sSAGZUc9C8uhvB2Ab3JBrg/exec?mode=test"
            }
        };

        // --
        // UnityWebRequestを使わないのは、staticで使えないから
        ObservableWWW.Get(access[key])
            .Subscribe(x =>
            {
                Debug.Log(x);
                
                this.message = "Done!";
                
                var data2 = JsonUtility.FromJson<Data4>(x);
                this.paramGeneral.param = data2.hoge_key;
                this.paramGeneral.param2 = data2.hoge_key2;

                Observable.Timer(TimeSpan.FromSeconds(2f))
                    .Subscribe(_ => this.message = "");
            }, ex => this.message = ex.ToString());
    }
    
    [PropertyOrder(0)] [LabelText("Update!")]
    [Button] [GUIColor(0, 1, 1)]
    private void UpdateSpeechScore()
    {
        this.message = "processing...";

        Observable.Interval(TimeSpan.FromSeconds(1f))
            .TakeWhile(_ => this.message != "Done!")
            .Subscribe(_ => this.message = _.ToString());
        
        string alertText = "一般的な文字データを更新します";
        if (!EditorUtility.DisplayDialog("override", alertText, "OK", "Cancel")) return;
        Request("Test"); // <= 実行
    }
}

[Serializable]
class Data4
{
    public string hoge_key;
    public string hoge_key2;
}
