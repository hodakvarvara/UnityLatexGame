using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


public class OpenPanels : MonoBehaviour
{
    // Создаем массив кнопок и массив текста и в зависимости от нажатия кнопки будем выводить разный тескт
    public Button[] sectionTeoryBttns = new Button[3]; // разделы теории
    //public Text[] sectionTeoryText = new Text[2]; // текст теории в зависимости от раздела
    public Sprite[] sectionTeoryImage = new Sprite[3]; //  изображения в теории в зависимости от раздела
    public Text TeoryText; // текст на панели
    public Image TeoryImage;// изображение на панели

    private bool sectionTeoryClicked1 = false; // 1 раздел теории
    private bool sectionTeoryClicked2 = false; // 2 раздел теории
    private bool sectionTeoryClicked3 = false; // 3 раздел теории
    private bool sectionTeoryClicked4 = false; // 4 раздел теории
    private bool sectionTeoryClicked5 = false; // 5 раздел теории
    private bool sectionTeoryClickedIstoch = false; // раздел источников
    Animator openedPanel;

    /// <summary>
    /// Открытие панели с текстом теории при выборе раздела 
    /// </summary>
    /// <param name="anim"></param>
    public void OpenPanel(Animator anim) // открытие панели с текстом теорией 
    {
        anim.gameObject.SetActive(true);

        // Анимация
        anim.SetTrigger("open");
        openedPanel = anim;

        if (sectionTeoryClicked1) // Если выбран раздел 1
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Теория\\Тема 1.txt");
            TeoryImage.enabled = false; // Выключаем изображение
            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }

            sectionTeoryClicked1 = false;
        }

        if (sectionTeoryClicked2) // Если выбран раздел 2
        {

            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Теория\\Тема 2.txt");

            // Заполнение текста
            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }

            TeoryImage.enabled = true; // Включаем изображение    
            TeoryImage.sprite = sectionTeoryImage[1]; // Назначаем изображение 

            sectionTeoryClicked2 = false;
        }

        if (sectionTeoryClicked3) // Если выбран раздел 3
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Теория\\Тема 3.txt");
            TeoryImage.enabled = false; // Выключаем изображение
            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }


            TeoryImage.enabled = true; // Включаем изображение    
            TeoryImage.sprite = sectionTeoryImage[2]; // Назначаем изображение 

            sectionTeoryClicked3 = false;
        }

        if (sectionTeoryClicked4) // Если выбран раздел 4
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Теория\\Тема 4.txt");

            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }

            TeoryImage.enabled = true; // Включаем изображение    
            TeoryImage.sprite = sectionTeoryImage[3]; // Назначаем изображение 
            sectionTeoryClicked4 = false;
        }

        if (sectionTeoryClicked5) // Если выбран раздел 5
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Теория\\Тема 5.txt");
            TeoryImage.enabled = false; // Выключаем изображение
            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }

            sectionTeoryClicked5 = false;
        }

        if (sectionTeoryClickedIstoch) // Если выбран раздел источники
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Теория\\Источники.txt");
            TeoryImage.enabled = false; // Выключаем изображение
            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }

            sectionTeoryClickedIstoch = false;
        }
    }

    /// <summary>
    /// Закрытие панель с текстом теории при нажатии кнопки "Назад" 
    /// </summary>
    public void ClosePanel()
    {
        openedPanel.SetTrigger("close");
    }

    /// <summary>
    /// Обработка нажатия на кнопку выбора раздела темы 1
    /// </summary>
    public void sectionBttn1()
    {
        sectionTeoryClicked1 = true;
    }

    /// <summary>
    /// Обработка нажатия на кнопку выбора раздела темы 2
    /// </summary>
    public void sectionBttn2()
    {
        sectionTeoryClicked2 = true;
    }

    /// <summary>
    /// Обработка нажатия на кнопку выбора раздела темы 3
    /// </summary>
    public void sectionBttn3()
    {
        sectionTeoryClicked3 = true;
    }

    /// <summary>
    /// Обработка нажатия на кнопку выбора раздела темы 4
    /// </summary>
    public void sectionBttn4()
    {
        sectionTeoryClicked4 = true;
    }

    /// <summary>
    /// Обработка нажатия на кнопку выбора раздела темы 5
    /// </summary>
    public void sectionBttn5()
    {
        sectionTeoryClicked5 = true;
    }

    /// <summary>
    /// Обработка нажатия на кнопку источники
    /// </summary>
    public void sectionBttnIstochn()
    {
        sectionTeoryClickedIstoch = true;
    }
}
