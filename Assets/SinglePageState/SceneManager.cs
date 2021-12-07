using UniRx;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private GameObject targetState;

    void Awake()
    {
        foreach (Transform child in this.transform)
        {
            // すべての子供を非アクティブに
            child.gameObject.SetActive(false);
            // レイアウトを中央に集結
            child.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    void Start()
    {
        // 最初の状態を指定
        ActivateState("STATE_A");
        
        // 各状態が終わったときの遷移先を指定
        Observable.EveryFixedUpdate()
            .Select(_ => this.targetState.activeInHierarchy)
            .DistinctUntilChanged()
            .Where(active => !active)
            .Select(_ => this.targetState.name)
            .Subscribe(stateName =>
            {
                switch (stateName)
                {
                    case "STATE_A" :
                        ActivateState("STATE_B");
                        break;
                    case "STATE_B" :
                        ActivateState("STATE_C");
                        break;
                    case "STATE_C" :
                        ActivateState("STATE_A");
                        break;
                    default:
                        break;
                }
            })
            .AddTo(this);
        // --
        // ポーズ系（TimeScale=0のときは、動作せず）
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => SwitchPause()) // Pauseは独立したStateでないので特別扱い
            .AddTo(this);
    }

    private void ActivateState(string stateName)
    {
        var state = this.transform.Find(stateName).gameObject;
        state.SetActive(true);
        // 監視対象に
        this.targetState = state;
    }
    
    private void SwitchPause()
    {
        var pause = this.transform.Find("PAUSE").gameObject;
        pause.SetActive(!pause.activeInHierarchy);
    }
}