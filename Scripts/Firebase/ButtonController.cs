using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ButtonController : MonoBehaviour
{
    public TMP_InputField NameInputField; // ввод текста в бд
    public TMP_InputField AgeInputField;

    private DB db;
    void Start()
    {
        db = GetComponent<DB>();
    }

    /* /// <summary>
    /// При нажатии считываются данные из Firebase
    /// </summary>
    public void ButtonGet()
    {
        StartCoroutine(db.LoadData(NameInputField.text));
    }

    /// <summary>
    /// При нажатии сохраняются данные в Firebase
    /// </summary>
    public void ButtonAdd()
    {
        db.SaveData(NameInputField.text);
    }

    /// <summary>
    /// При нажатии удаляются данные из Firebase
    /// </summary>
    public void ButtonRemoveData()
    {
        db.RemoveData(NameInputField.text);
    }

    public void login()
    {
        SceneManager.LoadScene("Game");
    }*/

}