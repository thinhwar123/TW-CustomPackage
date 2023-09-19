using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TW.Utility.CustomComponent;
using TW.Utility.Extension;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup))]
public class AUIPanel : AwaitableCachedMonoBehaviour
{
    [field: SerializeField] public AUIPanelConfig AUIPanelConfig {get; private set;}
    [field: SerializeField] public CanvasGroup CanvasGroup {get; private set;}
    [field: SerializeField] public RectTransform MainView {get; private set;}
    public UnityEvent OnStartOpenPanel { get; private set; } = new UnityEvent();
    public UnityEvent OnCompletedOpenPanel { get; private set; } = new UnityEvent();
    public UnityEvent OnStartClosePanel { get; private set; } = new UnityEvent();
    public UnityEvent OnCompletedClosePanel { get; private set; } = new UnityEvent();
    public List<Tween> AnimTween { get; private set; } = new List<Tween>();
    private AWaiter AnimWaiter { get; set; }
    private float OpenAnimDuration => AUIPanelConfig?.OpenAnimDuration ?? 0;
    private float CloseAnimDuration => AUIPanelConfig?.CloseAnimDuration ?? 0;
    #region Unity Function

    protected virtual void Awake()
    {
        Init();
        
    }

    protected virtual void Init()
    {
        OnStartOpenPanel.AddListener(() =>
        {
            gameObject.SetActive(true);
            SetInteractable(false);
        });
        OnCompletedOpenPanel.AddListener(() =>
        {
            SetInteractable(true);
        });
        OnStartClosePanel.AddListener(() =>
        {
            SetInteractable(false);
        });
        OnCompletedClosePanel.AddListener(() =>
        {
            SetInteractable(true);
            gameObject.SetActive(false);
        });

        if (AUIPanelConfig == null) return;
        AUIPanelConfig.SetupAnimEffect(this);
    }

    protected virtual void Reset()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        GetComponent<Canvas>().overrideSorting = true;
        GetComponent<Canvas>().sortingOrder = 0;
    }

    #endregion

    #region Panel Function

    public virtual T OpenPanel<T>() where T : AUIPanel
    {
        OnStartOpenPanel?.Invoke();
        AnimWaiter?.Kill();
        AnimWaiter = new AWaiter().OnComplete(() =>
        {
            OnCompletedOpenPanel?.Invoke();
        }).DelayCall(OpenAnimDuration);
        return this as T;
    }
    public virtual T ClosePanel<T>() where T : AUIPanel
    {
        OnStartClosePanel?.Invoke();
        AnimWaiter?.Kill();
        AnimWaiter = new AWaiter().OnComplete(() =>
        {
            OnCompletedClosePanel?.Invoke();
        }).DelayCall(CloseAnimDuration);
        return this as T;
    }

    private void SetInteractable(bool interactable)
    {
        CanvasGroup.interactable = interactable;
    }

    #endregion

    #region Debug

    [Button, FoldoutGroup("Debug Functions")]
    public void TestOpenAnim()
    {
        OpenPanel<AUIPanel>();
    }
    [Button, FoldoutGroup("Debug Functions")]
    public void TestCloseAnim()
    {
        ClosePanel<AUIPanel>();
    }

    #endregion
}
