using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int sceneIndex;
    [SerializeField] SceneSwitcher sceneSwitcher;

    [SerializeField] GameObject main;
    [SerializeField] GameObject settings;
    public void Exit() { Application.Quit(); }
    public void Play() { sceneSwitcher.LoadScene(sceneIndex); }
    public void ShowSettings()
    {
        main.SetActive(false);
        settings.SetActive(true);
    }

    public void ShowMain()
    {
        main.SetActive(true);
        settings.SetActive(false);
    }
}