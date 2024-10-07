using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading_Manager : IManager
{
    public override void Enter()
    {
        testElaspedTime = 0;
        Debug.Log("Loading Manager 진입");
        
        var task =  SceneManager.LoadSceneAsync(sceneIndex).ToUniTask();
        AwaitCompleteLoad(task).Forget();
    }
    
    public override void Realease()
    {
        isLoadComplete = false;
        Debug.Log("Loading Manager 해제");
    }
    
    public override void Update()
    {
        if(!isLoadComplete)
            return;
        
        testElaspedTime += Time.deltaTime;
        if (testElaspedTime >= testSceneChangeTime)
        {
            SystemManager.Instance.ChangeScene();
        }
    }
}
