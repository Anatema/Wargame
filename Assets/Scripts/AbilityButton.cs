using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Text Text;
    public Image Image;
    public Button Button;
    public GameObject SelectedOutline;
    public void SetButton(string text, Sprite icon)
    {
        Image.sprite = icon;
        Text.text = text;
    }

    public void SetUnactive()
    {
        SelectedOutline.SetActive(false);
    }
    public void SetActive()
    {
        SelectedOutline.SetActive(true);
    }
}
