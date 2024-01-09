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
        //Проверка что в системе присутствуют все необходимые зависимости для Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //если все верно подключаемся
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
        //Установим объект экземпляра аутентификации
        auth = FirebaseAuth.DefaultInstance;
    }

    /// <summary>
    /// Обработка кнопки входа
    /// </summary>
    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    /// <summary>
    /// Обработка кнопки регистрации
    /// </summary>
    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Вызываем функцию авторизации Firebase, передав адрес электронной почты и пароль
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //вывод в консоль
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //Обработка ошибок, если они есть
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Ошибка Входа!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Введите адрес эл. почты";
                    break;
                case AuthError.MissingPassword:
                    message = "Введите пароль";
                    break;
                case AuthError.WrongPassword:
                    message = "Неверный пароль";
                    break;
                case AuthError.InvalidEmail:
                    message = "Неверный адрес эл. почты";
                    break;
                case AuthError.UserNotFound:
                    message = "Учетная запись не существует";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //пользователь вошел в систему
            //get the result
            User = LoginTask.Result;
            Debug.LogFormat("Пользователь успешно вошел в систему: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            SceneManager.LoadScene("Game");
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //Если поле имя пользователя пусто, отобразится предупреждение
            warningRegisterText.text = "Не указано имя пользователя";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //Если пароль не совпадает, отобразится предупреждение
            warningRegisterText.text = "Пароль не совпадает!";
        }
        else
        {
            //Вызоваем функцию авторизации Firebase, передав адрес электронной почты и пароль
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Ошибка регистрации!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Не указана эл. почта";
                        break;
                    case AuthError.MissingPassword:
                        message = "Не указан пароль";
                        break;
                    case AuthError.WeakPassword:
                        message = "Слишком простой пароль";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Пользователь с такой эл. почтой уже существует";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                // пользователь создан
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Создайте профиль пользователя и укажите имя пользователя
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
                        //Обработка ошибок
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Не удалось установить имя пользователя!";
                    }
                    else
                    {
                        //Имя пользователя теперь установлено
                        //Теперь вернитесь к экрану входа в систему
                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                    }
                }
            }
        }

    }
}
