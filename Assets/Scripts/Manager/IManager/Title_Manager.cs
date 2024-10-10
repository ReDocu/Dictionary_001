using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_Manager : Manager
{
	public override void Enter()
	{
		testElaspedTime = 0;
		if (SceneManager.GetActiveScene().buildIndex != sceneIndex)
		{
			var task =  SceneManager.LoadSceneAsync(sceneIndex).ToUniTask();
			AwaitCompleteLoad(task).Forget();
		}
		else isLoadComplete = true;
		
		Debug.Log("Title Manager 진입");
	}
	public override void Realease()
	{
		isLoadComplete = false;
		Debug.Log("Title Manager 해제");
	}
	
	public override void Update()
	{
		if(!isLoadComplete)
			return;
		
		testElaspedTime += Time.deltaTime;
		if (testElaspedTime >= testSceneChangeTime)
		{
			ChangeState<Ingame_Normal_Manager>();
		}
	}
}
