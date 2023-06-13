using Assets.Scripts;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GUIMeshText : MonoBehaviour
{
    public string currentText { get; private set; } = "";
    TextMeshProUGUI _text;

    public int Length { get { return currentText.Length; } }

    private void OnEnable()
    {
        _text = GetComponent<TextMeshProUGUI>();
        currentText = _text.text;
    }

    private void Start()
    {
    }

    public void EnableRTL() => _text.isRightToLeftText = true;

    public void DisableRTL() => _text.isRightToLeftText = false;


    public int CountNoSpaces()
    {
        return currentText.Count(s => s != ' ');
    }


    public void UpdateText(string txt, bool rtl = false)
    {
        currentText = txt.Trim();
        _text.text = currentText;

        if (rtl) EnableRTL();
        else DisableRTL();
    }

    public void UpdateText(Word word)
    {
        currentText = word.text.Trim();
        _text.text = currentText;

        if (word.isRTL) EnableRTL();
        else DisableRTL();
    }



    public void ChangeColor(Color color)
    {
        _text.color = color;
    }

    public void ChangeFirstCharColor()
    {
        if (Length > 1)
        {
            string rest = currentText.Substring(1);
            char first = currentText.First();
            _text.text = "<color=\"red\">" + first + "</color>" + rest;
            return;
        }

        _text.text = "<color=\"red\">" + currentText + "</color>";
    }

    public void RemoveFirstChar() => UpdateText(currentText.Remove(0, 1), _text.isRightToLeftText);

    public char FirstChar() => currentText.First();

    public void Clear() => UpdateText("");


}