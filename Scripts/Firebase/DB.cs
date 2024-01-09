using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using System.Collections;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class DB : MonoBehaviour
{
    public TMP_Text text; // ����� ������ �� ��
    DatabaseReference dbRef;
    FirebaseAuth auth;

    public TMP_InputField email; // �����, ��������� �������������
    public TMP_InputField password; // ������,��������� �������������
    public TMP_Text InfoText; // ����� ���. ���������� ��� �����������

    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += Auth_StateChanged;
        Auth_StateChanged(this, null);
        auth.SignOut(); // ����� �� ���� ��������������� �����
    }

    private void Auth_StateChanged(object sender, System.EventArgs e)
    {
        if(auth.CurrentUser != null)
        {
            InfoText.text = "���� �������� " + auth.CurrentUser.Email;
            SceneManager.LoadScene("Game");
        }
        else
        {
            InfoText.text = "�� �������, ��� Email � password ������� �����?";
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void LoginButton()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text);
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void SignInButton()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text);
    }
    
    #region Add_Load_Remove_Firebase
    /// <summary>
    /// ���������� ������ � ��
    /// </summary>
    /// <param name="str"></param>
    public void SaveData(string name)
    {
        User user = new User(name, 20, "offline");
        string json = JsonUtility.ToJson(user);
        dbRef.Child("users").Child(name).SetRawJsonValueAsync(json); // user - ����� ������� � ��
    }

    /// <summary>
    /// ���������� ������ �� ��
    /// </summary>
    /// <param name="str"></param>
    public IEnumerator LoadData(string str)
    {
        var user = dbRef.Child("users").Child(str).GetValueAsync();

        yield return new WaitUntil(predicate: () => user.IsCompleted);
        // �������� �� ������
        if (user.Exception != null)
        {
            Debug.LogError(user.Exception);
        }
        else if (user.Result == null) // ��� ������ � user
        {
            Debug.Log("Null");
        }
        else
        {
            DataSnapshot snapshot = user.Result; // ��� ������ ������ �������� snapshot
            Debug.Log(snapshot.Child("age").Value.ToString() + snapshot.Child("name").Value.ToString());
            text.text = snapshot.Child("age").Value.ToString();
        }
    }

    /// <summary>
    /// ��������� ������ �� �� 
    /// </summary>
    public void RemoveData(string str)
    {
        dbRef.Child("users").Child(str).RemoveValueAsync();
    }
    #endregion Add_Load_Remove_Firebase
}

public class User
{
    public string name;
    public int age;
    public string status;

    public User (string name, int age, string status)
    {
        this.name = name;
        this.age = age;
        this.status = status;
    }
}
