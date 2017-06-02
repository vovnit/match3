using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    //Menu
    public List<LayoutElement> layoutElements;
    public List<Button> buttons;
    private int LastLevel;
    private int Level;
    public ScrollRect scrollRect;
    //public Button PauseButton;
    public Canvas canvas;

    void Start()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        int? a = PlayerPrefs.GetInt("LastLevel");
        if (a > 0)
        {
            LastLevel = PlayerPrefs.GetInt("LastLevel");
        }
        else
        {
            PlayerPrefs.SetInt("LastLevel", 1);
            LastLevel = PlayerPrefs.GetInt("LastLevel");
        }
        //Call the InitGame function to initialize the first level 
        Menu();
    }


    void loadLevel()
    {
        Constants.CurrentLevel = Level;
        //Constant.Mode устанавливается автоматически
        switch (Level)
        {
            case 1:

                Constants.Score = 10000; //сколько очков нужно
                Constants.RequiredTime = 45; //в секундках
                break;
            case 2:
                Constants.Score = 10000;
                Constants.RequiredMoves = 10; //количество ходов
                break;
            case 3:
                Constants.Score = 10000;
                Constants.RequiredTime = 45;
                break;
            case 4:
                Constants.Score = 10000;
                Constants.AreaForPoints1 = new Vector2(2,2); //область для очков от этой точки
                Constants.AreaForPoints2 = new Vector2(4, 4); //до этой
                break;
            case 5:
                Constants.Score = 10000;
                Constants.AreaForBan1 = new Vector2(2,2); //область запрета на передвижения фишек от этой точки
                Constants.AreaForBan2 = new Vector2(4, 4); //до этой
                Constants.AreaForPoints1 = new Vector2(2, 2);
                Constants.AreaForPoints2 = new Vector2(4, 4);
                break;
            case 6:
                Constants.Score = 10000; //сколько очков нужно
                Constants.RequiredTime = 45; //в секундках
                break;
            case 7:
                Constants.Score = 10000;
                Constants.RequiredMoves = 10; //количество ходов
                break;
            case 8:
                Constants.Score = 10000;
                Constants.RequiredTime = 45;
                break;
            case 9:
                Constants.Score = 10000;
                Constants.AreaForPoints1 = new Vector2(2, 2); //область для очков от этой точки
                Constants.AreaForPoints2 = new Vector2(4, 4); //до этой
                break;
            case 10:
                Constants.Score = 10000;
                Constants.AreaForBan1 = new Vector2(2, 2); //область запрета на передвижения фишек от этой точки
                Constants.AreaForBan2 = new Vector2(4, 4); //до этой
                Constants.AreaForPoints1 = new Vector2(2, 2);
                Constants.AreaForPoints2 = new Vector2(4, 4);
                break;
            case 11:
                Constants.Score = 10000; //сколько очков нужно
                Constants.RequiredTime = 45; //в секундках
                break;
            case 12:
                Constants.Score = 10000;
                Constants.RequiredMoves = 10; //количество ходов
                break;
            case 13:
                Constants.Score = 10000;
                Constants.RequiredTime = 45;
                break;
            case 14:
                Constants.Score = 10000;
                Constants.AreaForPoints1 = new Vector2(2, 2); //область для очков от этой точки
                Constants.AreaForPoints2 = new Vector2(4, 4); //до этой
                break;
            case 15:
                Constants.Score = 10000;
                Constants.AreaForBan1 = new Vector2(2, 2); //область запрета на передвижения фишек от этой точки
                Constants.AreaForBan2 = new Vector2(4, 4); //до этой
                Constants.AreaForPoints1 = new Vector2(2, 2);
                Constants.AreaForPoints2 = new Vector2(4, 4);
                break;
            case 16:
                Constants.Score = 10000; //сколько очков нужно
                Constants.RequiredTime = 45; //в секундках
                break;
            case 17:
                Constants.Score = 10000;
                Constants.RequiredMoves = 10; //количество ходов
                break;
            case 18:
                Constants.Score = 10000;
                Constants.RequiredTime = 45;
                break;
            case 19:
                Constants.Score = 10000;
                Constants.AreaForPoints1 = new Vector2(2, 2); //область для очков от этой точки
                Constants.AreaForPoints2 = new Vector2(4, 4); //до этой
                break;
            case 20:
                Constants.Score = 10000;
                Constants.AreaForBan1 = new Vector2(2, 2); //область запрета на передвижения фишек от этой точки
                Constants.AreaForBan2 = new Vector2(4, 4); //до этой
                Constants.AreaForPoints1 = new Vector2(2, 2);
                Constants.AreaForPoints2 = new Vector2(4, 4);
                break;
            case 21:
                Constants.Score = 10000; //сколько очков нужно
                Constants.RequiredTime = 45; //в секундках
                break;
            case 22:
                Constants.Score = 10000;
                Constants.RequiredMoves = 10; //количество ходов
                break;
            case 23:
                Constants.Score = 10000;
                Constants.RequiredTime = 45;
                break;
            case 24:
                Constants.Score = 10000;
                Constants.AreaForPoints1 = new Vector2(2, 2); //область для очков от этой точки
                Constants.AreaForPoints2 = new Vector2(4, 4); //до этой
                break;
            case 25:
                Constants.Score = 10000;
                Constants.AreaForBan1 = new Vector2(2, 2); //область запрета на передвижения фишек от этой точки
                Constants.AreaForBan2 = new Vector2(4, 4); //до этой
                Constants.AreaForPoints1 = new Vector2(2, 2);
                Constants.AreaForPoints2 = new Vector2(4, 4);
                break;
            default:
                break;

        }
        SceneManager.LoadScene("mainGame");
    }
    void level1()
    {
        Level = 1;
        loadLevel();
    }
    void level2()
    {
        Level = 2;
        loadLevel();
    }
    void level3()
    {
        Level = 3;
        loadLevel();
    }
    void level4()
    {
        Level = 4;
        loadLevel();
    }
    void level5()
    {
        Level = 5;
        loadLevel();
    }
    void level6()
    {
        Level = 6;
        loadLevel();
    }
    void level7()
    {
        Level = 7;
        loadLevel();
    }
    void level8()
    {
        Level = 8;
        loadLevel();
    }
    void level9()
    {
        Level = 9;
        loadLevel();
    }
    void level10()
    {
        Level = 10;
        loadLevel();
    }
    void level11()
    {
        Level = 11;
        loadLevel();
    }
    void level12()
    {
        Level = 12;
        loadLevel();
    }
    void level13()
    {
        Level = 13;
        loadLevel();
    }
    void level14()
    {
        Level = 14;
        loadLevel();
    }
    void level15()
    {
        Level = 15;
        loadLevel();
    }
    void level16()
    {
        Level = 16;
        loadLevel();
    }
    void level17()
    {
        Level = 17;
        loadLevel();
    }
    void level18()
    {
        Level = 18;
        loadLevel();
    }
    void level19()
    {
        Level = 19;
        loadLevel();
    }
    void level20()
    {
        Level = 20;
        loadLevel();
    }
    void level21()
    {
        Level = 21;
        loadLevel();
    }
    void level22()
    {
        Level = 22;
        loadLevel();
    }
    void level23()
    {
        Level = 23;
        loadLevel();
    }
    void level24()
    {
        Level = 24;
        loadLevel();
    }
    void level25()
    {
        Level = 25;
        loadLevel();
    }
    void Menu()
    {
        int widthOfScreen = Screen.width;
        foreach (var layoutElement in layoutElements)
        {
            layoutElement.preferredWidth = widthOfScreen;
            layoutElement.preferredHeight = widthOfScreen;
        }
        //PauseButton.transform.localScale = new Vector2(widthOfScreen / 600, widthOfScreen / 600);
        int heightpos = widthOfScreen / 2;
        //foreach (Button button in buttons)
        //{
        //    heightpos -= widthOfScreen / 10;
        //    button.transform.position = new Vector2(0, heightpos);
        //    button.transform.localScale = new Vector2(widthOfScreen / 200, widthOfScreen / 200);
        //    Text btnText = button.transform.FindChild("Text").GetComponent<Text>();
        //    btnText.text = "";
        //    //if (Convert.ToInt32(button.name) >= 1 && Convert.ToInt32(button.name) <= 5)
        //    //{
        //    //    button.transform.localScale = new Vector2(widthOfScreen / 50, widthOfScreen / 50);
        //    //}
        //}

        heightpos += -widthOfScreen / 200 - widthOfScreen/10;//widthOfScreen / 10 - widthOfScreen / 50;

        // float amplitudeX = (float)widthOfScreen/2;
        float amplitudeY = (float) widthOfScreen / 2;
        //float omegaX = 1.0f;
        float omegaY = 2.0f;
        for (int i = 0; i < buttons.Count; i++)
        {
            // float x = amplitudeX * Mathf.Cos(omegaX * i);
            float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * i));

            buttons[i].transform.position = new Vector2(y - widthOfScreen / 4, heightpos);
            buttons[i].transform.localScale = new Vector2(widthOfScreen / 300, widthOfScreen / 300);
            Text btnText = buttons[i].transform.Find("Text").GetComponent<Text>();
            int stars = PlayerPrefs.GetInt("Level " + Convert.ToInt32(buttons[i].name));
            btnText.text = "" + stars;
            heightpos -= widthOfScreen / 5;
            if (i % 5 == 0)//&&i!=0)
            {
                heightpos = widthOfScreen / 2 - widthOfScreen / 10 - widthOfScreen / 50;
            }
            if (i == 0)
            {
                heightpos -= widthOfScreen / 50;
            }
        }
        buttons[0].onClick.AddListener(level1);
        buttons[1].onClick.AddListener(level2);
        buttons[2].onClick.AddListener(level3);
        buttons[3].onClick.AddListener(level4);
        buttons[4].onClick.AddListener(level5);
        buttons[5].onClick.AddListener(level6);
        buttons[6].onClick.AddListener(level7);
        buttons[7].onClick.AddListener(level8);
        buttons[8].onClick.AddListener(level9);
        buttons[9].onClick.AddListener(level10);
        buttons[10].onClick.AddListener(level11);
        buttons[11].onClick.AddListener(level12);
        buttons[12].onClick.AddListener(level13);
        buttons[13].onClick.AddListener(level14);
        buttons[14].onClick.AddListener(level15);
        buttons[15].onClick.AddListener(level16);
        buttons[16].onClick.AddListener(level17);
        buttons[17].onClick.AddListener(level18);
        buttons[18].onClick.AddListener(level19);
        buttons[19].onClick.AddListener(level20);
        buttons[20].onClick.AddListener(level21);
        buttons[21].onClick.AddListener(level22);
        buttons[22].onClick.AddListener(level23);
        buttons[23].onClick.AddListener(level24);
        buttons[24].onClick.AddListener(level25);

        float YposCamera;
        YposCamera = (1-(float)LastLevel/25)+0.05f;

        scrollRect.verticalNormalizedPosition = YposCamera;
    }
}
