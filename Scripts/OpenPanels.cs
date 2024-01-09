using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


public class OpenPanels : MonoBehaviour
{
    // ������� ������ ������ � ������ ������ � � ����������� �� ������� ������ ����� �������� ������ �����
    public Button[] sectionTeoryBttns = new Button[3]; // ������� ������
    //public Text[] sectionTeoryText = new Text[2]; // ����� ������ � ����������� �� �������
    public Sprite[] sectionTeoryImage = new Sprite[3]; //  ����������� � ������ � ����������� �� �������
    public Text TeoryText; // ����� �� ������
    public Image TeoryImage;// ����������� �� ������

    private bool sectionTeoryClicked1 = false; // 1 ������ ������
    private bool sectionTeoryClicked2 = false; // 2 ������ ������
    private bool sectionTeoryClicked3 = false; // 3 ������ ������
    private bool sectionTeoryClicked4 = false; // 4 ������ ������
    private bool sectionTeoryClicked5 = false; // 5 ������ ������
    private bool sectionTeoryClickedIstoch = false; // ������ ����������
    Animator openedPanel;

    /// <summary>
    /// �������� ������ � ������� ������ ��� ������ ������� 
    /// </summary>
    /// <param name="anim"></param>
    public void OpenPanel(Animator anim) // �������� ������ � ������� ������� 
    {
        anim.gameObject.SetActive(true);

        // ��������
        anim.SetTrigger("open");
        openedPanel = anim;

        if (sectionTeoryClicked1) // ���� ������ ������ 1
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\������\\���� 1.txt");
            TeoryImage.enabled = false; // ��������� �����������
            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }

            sectionTeoryClicked1 = false;
        }

        if (sectionTeoryClicked2) // ���� ������ ������ 2
        {

            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\������\\���� 2.txt");

            // ���������� ������
            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }

            TeoryImage.enabled = true; // �������� �����������    
            TeoryImage.sprite = sectionTeoryImage[1]; // ��������� ����������� 

            sectionTeoryClicked2 = false;
        }

        if (sectionTeoryClicked3) // ���� ������ ������ 3
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\������\\���� 3.txt");
            TeoryImage.enabled = false; // ��������� �����������
            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }


            TeoryImage.enabled = true; // �������� �����������    
            TeoryImage.sprite = sectionTeoryImage[2]; // ��������� ����������� 

            sectionTeoryClicked3 = false;
        }

        if (sectionTeoryClicked4) // ���� ������ ������ 4
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\������\\���� 4.txt");

            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }

            TeoryImage.enabled = true; // �������� �����������    
            TeoryImage.sprite = sectionTeoryImage[3]; // ��������� ����������� 
            sectionTeoryClicked4 = false;
        }

        if (sectionTeoryClicked5) // ���� ������ ������ 5
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\������\\���� 5.txt");
            TeoryImage.enabled = false; // ��������� �����������
            string line = "";
            TeoryText.text = "";
            while ((line = sr.ReadLine()) != null)
            {
                TeoryText.text += line + Environment.NewLine;
            }

            sectionTeoryClicked5 = false;
        }

        if (sectionTeoryClickedIstoch) // ���� ������ ������ ���������
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\������\\���������.txt");
            TeoryImage.enabled = false; // ��������� �����������
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
    /// �������� ������ � ������� ������ ��� ������� ������ "�����" 
    /// </summary>
    public void ClosePanel()
    {
        openedPanel.SetTrigger("close");
    }

    /// <summary>
    /// ��������� ������� �� ������ ������ ������� ���� 1
    /// </summary>
    public void sectionBttn1()
    {
        sectionTeoryClicked1 = true;
    }

    /// <summary>
    /// ��������� ������� �� ������ ������ ������� ���� 2
    /// </summary>
    public void sectionBttn2()
    {
        sectionTeoryClicked2 = true;
    }

    /// <summary>
    /// ��������� ������� �� ������ ������ ������� ���� 3
    /// </summary>
    public void sectionBttn3()
    {
        sectionTeoryClicked3 = true;
    }

    /// <summary>
    /// ��������� ������� �� ������ ������ ������� ���� 4
    /// </summary>
    public void sectionBttn4()
    {
        sectionTeoryClicked4 = true;
    }

    /// <summary>
    /// ��������� ������� �� ������ ������ ������� ���� 5
    /// </summary>
    public void sectionBttn5()
    {
        sectionTeoryClicked5 = true;
    }

    /// <summary>
    /// ��������� ������� �� ������ ���������
    /// </summary>
    public void sectionBttnIstochn()
    {
        sectionTeoryClickedIstoch = true;
    }
}
