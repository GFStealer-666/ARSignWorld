using UnityEngine;

public class QuizUsertypeSelector : MonoBehaviour
{
    [SerializeField] private GameObject normalPanel;
    [SerializeField] private GameObject cripplePanel;

    public void ChooseNormalUser()
    {
        QuizDataHolder.selectedUserType = QuizEnum.QuizUserType.Normal;
        normalPanel.SetActive(true);
        cripplePanel.SetActive(false);
    }

    public void ChooseCrippleUser()
    {
        QuizDataHolder.selectedUserType = QuizEnum.QuizUserType.Cripple;
        normalPanel.SetActive(false);
        cripplePanel.SetActive(true);
    }
}
