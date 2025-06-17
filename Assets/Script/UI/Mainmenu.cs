using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
public class Mainmenu : MonoBehaviour
{
    [Header("Scene")]
    [Space(2)]
    [SerializeField] private SceneIndex sceneEnum;
    
    [Header("Panel")]
    [Space(2)]
    [SerializeField] private GameObject mainmenuPanel;
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject selectmodePanel;
    [SerializeField] private GameObject currentPanel;

    [SerializeField] private TextMeshProUGUI versionText;
    private Dictionary<ScenePanel, GameObject> panelMap;

    private void Awake()
    {
        panelMap = new Dictionary<ScenePanel, GameObject>
        {
            { ScenePanel.Mainmenu, mainmenuPanel },
            { ScenePanel.Settings, settingsPanel },
            { ScenePanel.Quiz, quizPanel },
            {ScenePanel.SelectMode, selectmodePanel }
        };

        currentPanel = mainmenuPanel; // Set initial panel
        versionText.text = $"V{Application.version}";
    }
    private void Start()
    {
        currentPanel = mainmenuPanel;
    }
    public static void LoadScene(SceneIndex value)
    {
        SceneManager.LoadScene(value.ToString());
    }
    
    public void LoadPanel(ScenePanel scenePanel)
    {
        if(!panelMap.ContainsKey(scenePanel)) return;
        currentPanel.SetActive(false);
        panelMap[scenePanel].SetActive(true);
        currentPanel = panelMap[scenePanel];
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void LoadMainmenuPanel() => LoadPanel(ScenePanel.Mainmenu);
    public void LoadQuizPanel() => LoadPanel(ScenePanel.Quiz);
    public void LoadSettingPanel() => LoadPanel(ScenePanel.Settings);
    public void LoadExitPanel() => LoadPanel(ScenePanel.Exit);
    public void LoadSelectModePanel() =>    LoadPanel(ScenePanel.SelectMode);
    public void QuitApp()
    {
        Application.Quit();

    #if UNITY_ANDROID
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    #endif
    }

}

public enum SceneIndex
{
    Mainmenu,
    ARScenery,
    QuizGame
}

public enum ScenePanel
{
    Mainmenu,
    SelectMode,
    Quiz,
    Settings,
    Exit
}
