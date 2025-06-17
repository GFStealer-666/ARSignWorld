using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizUserInfoSelector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_Dropdown ageDropdown;
    [SerializeField] private Button confirmButton;

    private QuizEnum.QuizUserGender? selectedGender = null; // Nullable

    private void Start()
    {
        ageDropdown.ClearOptions();
        var ages = new System.Collections.Generic.List<string>();
        for (int i = 12; i <= 99; i++) ages.Add(i.ToString());
        ageDropdown.AddOptions(ages);

        confirmButton.interactable = false;
        nameInput.onValueChanged.AddListener(_ => ValidateInput());
        ValidateInput();
    }

    public void ChooseMale() { SetGender(QuizEnum.QuizUserGender.Male); }
    public void ChooseFemale() { SetGender(QuizEnum.QuizUserGender.Female); }
    public void ChooseOther() { SetGender(QuizEnum.QuizUserGender.LGBTQ); }

    private void SetGender(QuizEnum.QuizUserGender gender)
    {
        selectedGender = gender;
        ValidateInput();
    }

    private void ValidateInput()
    {
        bool hasName = !string.IsNullOrWhiteSpace(nameInput.text);
        bool hasGender = selectedGender.HasValue;
        confirmButton.interactable = hasName && hasGender;
    }

    public void OnPersonInformationConfirm()
    {
        QuizDataHolder.Username = nameInput.text;
        QuizDataHolder.Gender = selectedGender.Value;
        QuizDataHolder.Age = int.Parse(ageDropdown.options[ageDropdown.value].text);
        Debug.Log($"User Info: {QuizDataHolder.Username}, {QuizDataHolder.Gender}, {QuizDataHolder.Age}");

        Mainmenu.LoadScene(SceneIndex.QuizGame);
    }
}
