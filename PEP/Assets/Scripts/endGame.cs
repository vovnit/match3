using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class endGame : MonoBehaviour {
    public static endGame instance = null;

    public Image background;
    public List<Sprite> images;
    public Text text;

    public Button menu;

    public Button reset;
    // Use this for initialization
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

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);
        menu.onClick.AddListener(GoToMenu);
        reset.onClick.AddListener(Reset);
        background.sprite = images[Constants.CurrentLevel / 5];
        int currentLevel = Constants.CurrentLevel;
        int score= PlayerPrefs.GetInt("Score" + currentLevel);
        int need = Constants.Score;
        int stars;
        if (score >= need)
        {
            stars = 3;
        }
        else if (score >= need/2)
        {
            stars = 2;
        } else if (score >= need / 3)
        {
            stars = 1;
        }
        else
        {
            stars = 0;
        }
        if (stars != 0)
        {
            text.text = String.Format("Поздравляю!\nТы успешно прошёл уровень {0}\nТы заработал {1} очков и {2} звёзд.",
                currentLevel, score, stars);
        }
        else
        {
            text.text = String.Format(
                "К сожалению твоего счёта {0} недостаточно, чтобы засчитать данный уровень. Тебе нужно набрать хотя бы {1}",
                score, need / 3);
        }
    }

    void GoToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

    void Reset()
    {
        SceneManager.LoadScene("mainGame");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
