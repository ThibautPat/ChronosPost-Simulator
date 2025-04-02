using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image progressbar;


    public void Exit() { Application.Quit(); }


    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronouslyWithDelay(sceneIndex));
    }

    IEnumerator LoadAsynchronouslyWithDelay(int sceneIndex)
    {
        // Start loading the scene in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // Prevent the scene from activating immediately
        operation.allowSceneActivation = false;

        loadingScreen.SetActive(true);

        // Simulate random loading time between 3 to 6 seconds
        float simulatedLoadingTime = Random.Range(3f, 6f);
        float elapsedSimulatedTime = 0f;

        while (elapsedSimulatedTime < simulatedLoadingTime)
        {
            // Calculate simulated progress based on time
            float simulatedProgress = Mathf.Clamp01(elapsedSimulatedTime / simulatedLoadingTime);

            // Combine simulated progress with the actual operation progress
            float progress = Mathf.Clamp01(Mathf.Min(operation.progress / 0.9f, simulatedProgress));

            progressbar.fillAmount = Mathf.SmoothStep(progressbar.fillAmount, progress, Time.deltaTime * 3f);

            elapsedSimulatedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure progress bar is full
        progressbar.fillAmount = 1f;

        // Allow the scene to activate after the simulated loading is done
        operation.allowSceneActivation = true;

        // Wait until the scene has fully loaded
        while (!operation.isDone)
        {
            yield return null;
        }

        // Hide the loading screen after the new scene has loaded
        loadingScreen.SetActive(false);
    }
}
