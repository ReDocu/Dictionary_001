using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Dic.Singleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 모든 씬 관리하는 매니저 클래스
/// </summary>
public class SystemManager : DontDestroySingleton<SystemManager>
{
	private Manager curManager;
	private Manager targetScene;
	private Dictionary<Type, Manager> sceneDictionary = new Dictionary<Type, Manager>();

	public Manager TargetScene => targetScene;

	protected override void Init()
	{
		RegisterScenesToDictionary();

		curManager = sceneDictionary[typeof(Title_Manager)];
		curManager.Enter();
	}

	private void Update()
	{
		curManager?.Update();
	}

	public async UniTaskVoid ChangeScene<T>() where T : Manager
	{
		// 현재 씬 종료
		curManager.Realease();
		
		targetScene = sceneDictionary[typeof(T)];
		curManager = sceneDictionary[typeof(Loading_Manager)];

		FadeIn().Forget();
	}

	private async UniTaskVoid FadeIn()
	{
		await FadeManager.Instance.FadeIn();
		
		// 씬 초기화
		curManager.Enter();
	}
	
	public void ChangeScene()
	{
		curManager.Realease();
		curManager = targetScene;
		targetScene = null;
		
		FadeIn().Forget();
	}


	/// <summary>
	/// 빌드에 등록된 씬 목록을 불러와
	/// 같은 이름의 스크립트를 찾아서 딕셔너리에 저장
	/// </summary>
	private void RegisterScenesToDictionary()
	{
		Assembly assembly = Assembly.GetExecutingAssembly();

		//build 목록에 들어있는 씬 리스트 
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
			string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
			string scriptName = sceneName.Replace("_Scene", "_Manager");

			Type t = assembly.GetType(scriptName);

			if (t == null)
				continue;

			object obj = Activator.CreateInstance(t);
			if (obj == null)
			{
				Debug.LogError("씬 등록 error");
				return;
			}

			var iManager = obj as Manager;
			try
			{
				sceneDictionary.TryAdd(iManager.GetType(), iManager);
				iManager.Init(i);
				Debug.LogSuccess($"{sceneName} 등록 완료");
			}
			catch (Exception ex)
			{
				Debug.Assert(false, $"{ex.Message} {sceneName} 씬 등록 실패");
				throw;
			}
		}
	}
}