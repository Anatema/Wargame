using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Text Text;
    public Button Button;
    public void SetButton(string text)
    {
        Text.text = text;
    }
}
