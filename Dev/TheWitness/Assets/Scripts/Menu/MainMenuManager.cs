namespace MainMenu
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenuManager : Singleton<MainMenuManager>
    {
        public void Exit()
        {
            Application.Quit();
        }

        public void ChangeScene(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}