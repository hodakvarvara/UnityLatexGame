using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    [Header("Loading Panel")]
    [SerializeField] private Animator loadPanel;
    [SerializeField] private float animationDuration = 0.20f; // Время анимации
    [SerializeField] private bool playAnimationInStart;
    int currentScene;

    private void Start()
    {
        if (playAnimationInStart) // Если playAnimationInStart = true то при загрузке сцены будет проигрыватся анимация(end) открытия у панели
        {
            loadPanel.gameObject.SetActive(true);
            loadPanel.SetTrigger("end");
        } 
    }

    /// <summary>
    ///  Переход на новую сцену
    /// </summary>
    /// <param name="sceneid"></param>
    public void NextLevel(int sceneid) 
    {
        currentScene = sceneid;
        StartCoroutine(LoadCurrentScene()); // Старовать куротину(IEnumerator) LoadCurrentScene()
    }
    IEnumerator LoadCurrentScene()
    {
        loadPanel.gameObject.SetActive(true); // включить панельку

        loadPanel.SetTrigger("start"); // начать анимацию

        // yield определяет возвращаемый элемент
        yield return new WaitForSeconds(animationDuration); // ждать --- секунд

        SceneManager.LoadScene(currentScene); // начать загрузку сцены
    }
}
