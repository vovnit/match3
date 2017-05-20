using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    //Menu
    public List<LayoutElement> layoutElements;
    public List<Button> buttons;
    public int LastLevel;
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

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
        Menu();
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
        heightpos += -widthOfScreen / 10 - widthOfScreen / 50;
        // float amplitudeX = (float)widthOfScreen/2;
        float amplitudeY = (float) widthOfScreen / 2;
        //float omegaX = 1.0f;
        float omegaY = 2.0f;
        for (int i = 0; i < buttons.Count; i++)
        {
            // float x = amplitudeX * Mathf.Cos(omegaX * i);
            float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * i));

            buttons[i].transform.position = new Vector2(y - widthOfScreen / 4, heightpos);
            buttons[i].transform.localScale = new Vector2(widthOfScreen / 200, widthOfScreen / 200);
            Text btnText = buttons[i].transform.Find("Text").GetComponent<Text>();
            btnText.text = "";
            heightpos -= widthOfScreen / 5;
            if (i % 5 == 0&&i!=0)
            {
                heightpos = widthOfScreen / 2 - widthOfScreen / 10 - widthOfScreen / 50;
            }
        }
        float YposCamera;
        YposCamera = (1-(float)LastLevel/25)+0.05f;
        scrollRect.verticalNormalizedPosition = YposCamera;
    }
}
