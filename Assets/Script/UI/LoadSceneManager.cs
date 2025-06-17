using UnityEngine;

public class LoadSceneManager : MonoBehaviour
{
    public void LoadMainmenu()
    {
        Mainmenu.LoadScene(SceneIndex.Mainmenu);
    }
    public void LoadAR()
    {
        Mainmenu.LoadScene(SceneIndex.ARScenery);
    }
}
