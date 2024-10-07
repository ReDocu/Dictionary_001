using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dic.Singleton;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeManager : DontDestroySingleton<FadeManager>
{
    private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float duration = 1f;
    
    protected override void Init()
    {
        fadeCanvasGroup = GetComponent<CanvasGroup>();
    }
    

    public async UniTask FadeAsync(UniTask onFadeState,float duration = 1f, UniTask onBegin = default, UniTask onEnd = default)
    {
        await onBegin;
        fadeCanvasGroup.gameObject.SetActive(true);
        await fadeCanvasGroup.DOFade(1f, duration).SetUpdate(true).AsyncWaitForCompletion();
        await onFadeState;
        await fadeCanvasGroup.DOFade(0f, duration).SetUpdate(true).AsyncWaitForCompletion();
        fadeCanvasGroup.gameObject.SetActive(false);
        await onEnd;
    }

    /// <summary>
    /// 페이드 인
    /// </summary>
    /// <param name="duration"></param>
    public async UniTask FadeIn(float duration = 1f)
    {
        //fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0f;
        
        await fadeCanvasGroup.DOFade(1f, duration).SetUpdate(true).AsyncWaitForCompletion();
       // fadeCanvasGroup.gameObject.SetActive(false);
    }
    
    public async UniTask FadeOut(float duration = 1f)
    {
        //fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 1f;
        await fadeCanvasGroup.DOFade(0f, duration).SetUpdate(true).AsyncWaitForCompletion();
        // fadeCanvasGroup.gameObject.SetActive(false);
    }
}
