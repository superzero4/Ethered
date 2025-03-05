using UnityEngine.SceneManagement;

namespace Common.GlobalFlow
{
    public class SceneFlow
    {
        public enum EScene
        {
            MainMenu = 0,
            SquadMenu = 1,
            Battle = 2,
            GameOver = 3
        }

        public static void LoadScene(EScene scene)
        {
            int sceneIndex;
            switch(scene)
            {
                case EScene.MainMenu:
                    sceneIndex = 0;
                    break;
                case EScene.SquadMenu:
                    sceneIndex = 1;
                    break;
                case EScene.Battle:
                    sceneIndex = 2;
                    break;
                case EScene.GameOver:
                    sceneIndex = 3;
                    break;
                default:
                    sceneIndex = 0;
                    break;
            }
            SceneManager.LoadScene(sceneIndex);
        }
    }
}