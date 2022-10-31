using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPanel : MonoBehaviour
{
    public Text Text;
    public RectTransform RectTransform;

    public void ShowData(string text, Vector2 position)
    {
        gameObject.SetActive(true);
        Text.text = text;
        RectTransform.position = position;
    }

    public void HideData()
    {
        gameObject.SetActive(false);
    }
}
