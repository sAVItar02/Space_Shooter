using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 1f;
    IEnumerator LoadLevel(int levelIndex, float transitionTime, float waitTime)
    {
        StartCoroutine(SetTrigger(waitTime));
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    public void LoadStartMenu()
    {
        StartCoroutine(LoadLevel(0, transitionTime, 0f));
    }

    public void LoadGame()
    {
        //SceneManager.LoadScene("Game Scene");
        StartCoroutine(LoadLevel(1, transitionTime, 0f));
    }

    public void LoadGameOver()
    {
        StartCoroutine(LoadLevel(2, transitionTime, 2f));
    }

    IEnumerator SetTrigger(float time)
    {
        yield return new WaitForSeconds(time);
        transition.SetTrigger("Start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }    
}
