using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : SingletonPersistent<SceneSwitcher>
{
    public Animator animator;
    private PossibleScenes sceneToLoad;
    private Action onTransitionComplete;
    private void Start() 
    {
        animator.enabled = false;
    }

    public void TransitionToScene(PossibleScenes sceneName, Action onTransitionComplete)
    {
        this.onTransitionComplete = onTransitionComplete;
        animator.enabled = true;
        sceneToLoad = sceneName;
        animator.SetTrigger("FadeToBlack");
    }

    public void OnTransitionComplete()
    {
        StartCoroutine(LoadLevel());
    }
    private IEnumerator LoadLevel()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneToLoad.ToString());
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        if (asyncLoadLevel.isDone)
        {
            animator.SetTrigger("FadeToLevel");
            if(this.onTransitionComplete != null) 
            {
                this.onTransitionComplete();
                this.onTransitionComplete = null;
            } 
        }
    }
}
