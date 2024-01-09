using Firebase.Auth;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Authmanager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    void Awake()
    {
        //�������� ��� � ������� ������������ ��� ����������� ����������� ��� Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //���� ��� ����� ������������
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //��������� ������ ���������� ��������������
        auth = FirebaseAuth.DefaultInstance;
    }

    /// <summary>
    /// ��������� ������ �����
    /// </summary>
    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    /// <summary>
    /// ��������� ������ �����������
    /// </summary>
    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //�������� ������� ����������� Firebase, ������� ����� ����������� ����� � ������
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //����� � �������
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //��������� ������, ���� ��� ����
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "������ �����!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "������� ����� ��. �����";
                    break;
                case AuthError.MissingPassword:
                    message = "������� ������";
                    break;
                case AuthError.WrongPassword:
                    message = "�������� ������";
                    break;
                case AuthError.InvalidEmail:
                    message = "�������� ����� ��. �����";
                    break;
                case AuthError.UserNotFound:
                    message = "������� ������ �� ����������";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //������������ ����� � �������
            //get the result
            User = LoginTask.Result;
            Debug.LogFormat("������������ ������� ����� � �������: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            SceneManager.LoadScene("Game");
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //���� ���� ��� ������������ �����, ����������� ��������������
            warningRegisterText.text = "�� ������� ��� ������������";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //���� ������ �� ���������, ����������� ��������������
            warningRegisterText.text = "������ �� ���������!";
        }
        else
        {
            //�������� ������� ����������� Firebase, ������� ����� ����������� ����� � ������
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "������ �����������!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "�� ������� ��. �����";
                        break;
                    case AuthError.MissingPassword:
                        message = "�� ������ ������";
                        break;
                    case AuthError.WeakPassword:
                        message = "������� ������� ������";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "������������ � ����� ��. ������ ��� ����������";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                // ������������ ������
                User = RegisterTask.Result;

                if (User != null)
                {
                    //�������� ������� ������������ � ������� ��� ������������
                    UserProfile profile = new UserProfile { DisplayName = _username };
                  
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    User.SendEmailVerificationAsync().ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.LogError("SendEmailVerificationAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                            return;
                        }

                        Debug.Log("Email sent successfully.");
                    });

                        if (ProfileTask.Exception != null)
                    {
                        //��������� ������
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "�� ������� ���������� ��� ������������!";
                    }
                    else
                    {
                        //��� ������������ ������ �����������
                        //������ ��������� � ������ ����� � �������
                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                    }
                }
            }
        }

    }
}
