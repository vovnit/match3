using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fitterScript : MonoBehaviour {
    public LayoutElement layoutElement;
    public List<Button> buttons;
    // Use this for initialization
    void Start() {
        int widthOfScreen = Screen.width;
        layoutElement.preferredWidth = widthOfScreen;
        layoutElement.preferredHeight = widthOfScreen;
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
        float amplitudeY = (float)widthOfScreen/4;
        //float omegaX = 1.0f;
        float omegaY = 2.0f;
        int a = Convert.ToInt32(buttons[0].name);
        for (int i = 0; i < buttons.Count; i++)
        {
           // float x = amplitudeX * Mathf.Cos(omegaX * i);
            float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * i * a));
            buttons[i].transform.position = new Vector2(y,  heightpos);
            buttons[i].transform.localScale = new Vector2(widthOfScreen / 200, widthOfScreen / 200);
            Text btnText = buttons[i].transform.Find("Text").GetComponent<Text>();
            btnText.text = "";
            heightpos -= widthOfScreen / 5;
        }

    }
}
