using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 1f;
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    public void LoadStartMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void LoadGame()
    {
        //SceneManager.LoadScene("Game Scene");
        StartCoroutine(LoadLevel(1));
    }

    public void LoadGameOver()
    {
        // SceneManager.LoadScene("Game Over");
        StartCoroutine(LoadLevel(2));
    }

    public void QuitGame()
    {
        Application.Quit();
    }    
}
