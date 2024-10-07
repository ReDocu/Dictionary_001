using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 각 씬을 관리할 매니저 추상 클래스
/// </summary>
public abstract class IManager
{
	public int testSceneChangeTime = 2;
	public float testElaspedTime = 0;
	protected int sceneIndex;

	protected bool isLoadComplete;
	public void Init(int sceneIndex) => this.sceneIndex = sceneIndex;

	public abstract void Enter();

	public abstract void Realease();
	public abstract void Update();
	
	protected async UniTaskVoid AwaitCompleteLoad(UniTask task)
	{
		await task;
		await FadeManager.Instance.FadeOut();
		isLoadComplete = true;
	}
	protected void ChangeState<T>() where T : IManager
	{
		SystemManager.Instance.ChangeScene<T>();
	}
}
