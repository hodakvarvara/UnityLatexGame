// Основной скрипт игры викторины от Android 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using TMPro;
using System.Linq;

public class GameScript : MonoBehaviour {
    #region Описание переменных
    [HideInInspector] // Позволяет не отображать переменную, которой он назначен, в редакторе
    public QuestionsList[] Questions;
    [HideInInspector]
    public int publicTimeCount = 20;
    [HideInInspector]
    public Color trueCC, falseCC, defaultCC; // Цвет панели при ответе
    [HideInInspector]
    public int multiplierScore = 100;

    public GameObject PanelTrue;
    public GameObject PanelFalse;
    public Text questionText;
    public Button[] answerBttns = new Button[3];
    public Text[] answersText = new Text[3];
    public GameObject[] answersIcons; // 0 - trueIcon; 1 - falseIcon;
    public Image headPanel;
    public GameObject exitPanel;
    public GameObject finalText;
    public Text time;
    public Text recordText;
    public Text scoreText;
    private int timeCount = 20;
    private int score;
    private float scoreForRecord;
    private int currentQ = 1;
    private bool answerClicked;
    public Texture2D editorImg;
    public Image bg;
    private int playTime;
    private bool trueColor, falseColor,defaultColor;
    private int randQ;
    private List<object> qList;
    private QuestionsList crntQ;
    private int ExitFlag = 0;
     //firebase

    DatabaseReference dbRef;
    FirebaseAuth auth;
    int myScore;
    [SerializeField] TMP_Text TextMyScore;
    [SerializeField] TMP_Text TextLeaders;
    #endregion

    private void UpdateMyScoreText()
    {
        TextMyScore.text = myScore.ToString();
    }

    public void ButtonLeave()
    {
        PlayerPrefs.DeleteKey("MyScore");
        auth.SignOut();
    }

    /// <summary>
    ///  Вызывается один раз за кадр
    ///  Это основное событие для прорисовки кадра.
    /// </summary>
    void Update ()
    {
        myScore = PlayerPrefs.GetInt("MyScore");
        UpdateMyScoreText();

        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;


        scoreText.text = string.Format("Ваш счёт: {0:0}", score);
        scoreForRecord = PlayerPrefs.GetInt("score");
        recordText.text = string.Format("Ваш рекорд: {0:0}", scoreForRecord);

        // Назначение цвета панели в зависимости от ответа
        if (defaultColor)
        {
            headPanel.color = Color.Lerp(headPanel.color, defaultCC, 8 * Time.deltaTime);
        }
        else if (trueColor)
        {
            headPanel.color = Color.Lerp(headPanel.color, trueCC, 8 * Time.deltaTime);
        }
        else if (falseColor)
        {
            headPanel.color = Color.Lerp(headPanel.color, falseCC, 8 * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !exitPanel.activeSelf) 
        {
            exitPanel.SetActive(true); Time.timeScale = 0; 
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && exitPanel.activeSelf) 
        { 
            exitPanel.SetActive(false); Time.timeScale = 1;
        }
        
    }

    /// <summary>
    /// Обработка нажатия кнопки "Тест" в главном меню
    /// </summary>
    public void playBttn(int time)
    {
        playTime = time;
        timeCount = playTime;
        qList = new List<object>(Questions);    // инициализируем список вопросов
        generateQuestion();
        headPanel.GetComponent<Animation>().Play("HeadAnim");
        score = 0;
        myScore = 0;
        finalText.SetActive(false);
    }

    /// <summary>
    /// Генерация нового вопроса
    /// </summary>
    void generateQuestion()
    {
        if (qList.Count > 0)
        {
            if (scoreText.gameObject.activeSelf) scoreText.GetComponent<Animation>().Play("Bubble_Close_3");
            randQ = Random.Range(0, qList.Count);
            crntQ = qList[randQ] as QuestionsList;
            if (crntQ != null)
            {
                questionText.text = crntQ.Question;
                questionText.GetComponent<Animation>().Play("Bubble_Open_1");
                List<string> answers = new List<string>(crntQ.answers);
                for (int i = 0; i < crntQ.answers.Length; i++)
                {
                    int randA = Random.Range(0, answers.Count);
                    answersText[i].text = answers[randA];
                    answers.RemoveAt(randA);
                }
            }
            StartCoroutine(answersBttnsInAnim()); // куротин - выполнение кода через определенное время
            timeCount = playTime;
            currentQ++;
        }
        else StartCoroutine(final());
    }
    public void answerBttn(int index)
    {
        answerClicked = true;
        StartCoroutine(trueOrFalse(answersText[index].text == crntQ.answers[0]));
    }
    IEnumerator final()
    {
        finalText.SetActive(true);
        yield return new WaitForSeconds(2);
        trueColor = false;
        defaultColor = true;
        headPanel.GetComponent<Animation>().Play("HeadAnimOut");
        scoreText.GetComponent<Animation>().Play("Bubble_Close_3");
        finalText.GetComponent<Animation>().Play("Bubble_Close_3");
        if (score > PlayerPrefs.GetInt("score")) PlayerPrefs.SetInt("score", score);
    }
    IEnumerator timer()
    {
        answerClicked = false;
        if (!time.gameObject.activeSelf) time.gameObject.SetActive(true);
        else time.GetComponent<Animation>().Play("Bubble_Open_3");
        while (timeCount > -1)
        {
            if (!answerClicked)
            {
                time.text = timeCount.ToString();
                timeCount--;
                yield return new WaitForSeconds(1);
            }
            else yield break;
        }
        foreach (Button t in answerBttns) t.interactable = false;
        if (!answerClicked) StartCoroutine(timeOut());
    }
    IEnumerator answersBttnsInAnim()
    {
        foreach (Button t in answerBttns)
        {
            t.interactable = false; 
        }
        int i = 0;
        yield return new WaitForSeconds(1);
        while (i < answerBttns.Length)
        {
            if (!answerBttns[i].gameObject.activeSelf)
            {
                answerBttns[i].gameObject.SetActive(true);
            }
            else
            {
                answerBttns[i].GetComponent<Animation>().Play("Bubble_Open_2");
            }

            i++;
            yield return new WaitForSeconds(1);
        }
        foreach (Button t in answerBttns) t.interactable = true;
        yield return StartCoroutine(timer()); // 20 сек отсчета при выборе ответа
    }
    IEnumerator timeOut()
    {
        foreach (Button t in answerBttns)
        {
            t.GetComponent<Animation>().Play("Bubble_Close_2");
        }
        falseColor = true;
        PanelFalse.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        if (!answersIcons[2].activeSelf)
        {
            answersIcons[2].SetActive(true);
        }
        else
        {
            answersIcons[2].GetComponent<Animation>().Play("Bubble_Open_3");
        }
        questionText.GetComponent<Animation>().Play("Bubble_Close_1");
        yield return new WaitForSeconds(0.5f);
        if (!scoreText.gameObject.activeSelf)
        {
            scoreText.gameObject.SetActive(true);
        }
        else
        {
            scoreText.GetComponent<Animation>().Play("Bubble_Open_3");
        }
        yield return new WaitForSeconds(2);
        answersIcons[2].GetComponent<Animation>().Play("Bubble_Close_3");
        time.GetComponent<Animation>().Play("Bubble_Close_3");
        falseColor = false;
        defaultColor = true;
        PanelFalse.SetActive(false);
        headPanel.GetComponent<Animation>().Play("HeadAnimOut");
        if (score > PlayerPrefs.GetInt("score"))
        {
            PlayerPrefs.SetInt("score", score);
        }
    }
    /// <summary>
    /// BoardLeader
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetLeaders()
    {
        var leaders = dbRef.Child("LeaderBoard").OrderByChild("MyScore").GetValueAsync();

        yield return new WaitUntil(predicate: () => leaders.IsCompleted);

        if (leaders.Exception != null)
        {
            Debug.LogError("ERROR: " + leaders.Exception);
        }
        else if (leaders.Result.Value == null)
        {
            Debug.LogError("Result.Value == null");
        }
        else
        {
            DataSnapshot snapshot = leaders.Result;

            int num = 1;
            foreach (DataSnapshot dataChildSnapshot in snapshot.Children.Reverse())
            {
                TextLeaders.text += "\n" + num + ". " + dataChildSnapshot.Child("Email").Value.ToString() + " : " +
                    dataChildSnapshot.Child("MyScore").Value.ToString();
                num++;
            }
        }
    }

    public void openLeaderBoard()
    {
        StartCoroutine(GetLeaders());
    }


    public void BackButton()
    {
        TextLeaders.text = "";
    }


    /// <summary>
    /// Проверка правильный ли ответ был в тесте
    /// </summary>
    /// <param name="check"></param>
    /// <returns></returns>
    IEnumerator trueOrFalse(bool check)
    {
        defaultColor = false;
        foreach (Button t in answerBttns)
        {
            t.interactable = false;
        }
        yield return new WaitForSeconds(1);
        if (check)
        {
            //score = score + (multiplierScore * currentQ) + (timeCount * multiplierScore);
            score = score + 10; // Количество баллов за каждый правильный вариант ответа


            //Таблица лидеров
            myScore = myScore + 10;
            UpdateMyScoreText();
            PlayerPrefs.SetInt("MyScore", myScore);

            dbRef.Child("LeaderBoard").Child(auth.CurrentUser.Email.Replace(".", "")).Child("MyScore").SetValueAsync(myScore);
            dbRef.Child("LeaderBoard").Child(auth.CurrentUser.Email.Replace(".", "")).Child("Email").SetValueAsync(auth.CurrentUser.Email);


            foreach (Button t in answerBttns)
            {
                t.GetComponent<Animation>().Play("Bubble_Close_2");
            }
            trueColor = true;
            PanelTrue.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            if (!answersIcons[0].activeSelf)
            {
                answersIcons[0].SetActive(true);
            }  
            else
            {
                answersIcons[0].GetComponent<Animation>().Play("Bubble_Open_3");
            }  
            questionText.GetComponent<Animation>().Play("Bubble_Close_1");
            yield return new WaitForSeconds(0.5f);
            time.GetComponent<Animation>().Play("Bubble_Close_3");
            qList.RemoveAt(randQ); // удаляем текущий вопрос
            if (!scoreText.gameObject.activeSelf)
            {
                scoreText.gameObject.SetActive(true);
            }
            else
            {
                scoreText.GetComponent<Animation>().Play("Bubble_Open_3");
            }
            yield return new WaitForSeconds(1);
            answersIcons[0].GetComponent<Animation>().Play("Bubble_Close_3");
            trueColor = false;
            defaultColor = true;
            PanelTrue.SetActive(false);
            generateQuestion(); // генерация нового вопроса
        }
        else
        {
            foreach (Button t in answerBttns) t.GetComponent<Animation>().Play("Bubble_Close_2");
            falseColor = true;
            PanelFalse.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            if (!answersIcons[1].activeSelf) answersIcons[1].SetActive(true);
            else answersIcons[1].GetComponent<Animation>().Play("Bubble_Open_3");
            questionText.GetComponent<Animation>().Play("Bubble_Close_1");
            yield return new WaitForSeconds(0.5f);
            if (!scoreText.gameObject.activeSelf) scoreText.gameObject.SetActive(true);
            else scoreText.GetComponent<Animation>().Play("Bubble_Open_3");
            yield return new WaitForSeconds(1);
            answersIcons[1].GetComponent<Animation>().Play("Bubble_Close_3");
            time.GetComponent<Animation>().Play("Bubble_Close_3");
            falseColor = false;
            defaultColor = true;
            PanelFalse.SetActive(false);
            headPanel.GetComponent<Animation>().Play("HeadAnimOut");
            scoreText.GetComponent<Animation>().Play("Bubble_Close_3");
            if (score > PlayerPrefs.GetInt("score")) PlayerPrefs.SetInt("score", score);
            yield return new WaitForSeconds(1.5f);
            scoreText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Обработка выхожа когда идет тест
    /// выход на главный экран
    /// 0 - да
    /// 1 - нет
    /// </summary>
    /// <param name="bttn"></param>
    public void exitPan(int bttn)
    {
        if (ExitFlag == 1) // нужно выйти на окно регистрации?
        {
            if (bttn == 0)
            {
                SceneManager.LoadScene("LogIn");
            }
            else { exitPanel.SetActive(false); Time.timeScale = 1; }
            ExitFlag = 0;
        }
        else
        {
            if (bttn == 0)
            {
                if (score > PlayerPrefs.GetInt("score")) PlayerPrefs.SetInt("score", score);
                Application.Quit();
            }
            else { exitPanel.SetActive(false); Time.timeScale = 1; }
        }
       
    }

    /// <summary>
    /// Обработка выхода из игры на главном экране
    /// переходим на окно регистрации
    /// </summary>
    public void Exitgame()
    {
        exitPanel.SetActive(true);
        ExitFlag = 1;
    }
}

[System.Serializable]
public class QuestionsList
{
    public string Question;
    public string[] answers = new string[3];
}