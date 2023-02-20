using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StyleManager : Singleton<StyleManager>
{
    [SerializeField] Image bar;
    [SerializeField] TextMeshProUGUI grade;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] string[] grades;
    [SerializeField] Color[] colors;
    
    [SerializeField] float maxStyle;
    private float style = 0;
    void Start()
    {
        UpdateVisuals();
    }

    void FixedUpdate() {
        if (style < maxStyle)
            ChangeStyle(-1);
    }

    private void UpdateVisuals() {
        int index = GetGrade();
        grade.text = grades[index].Substring(0, 1);
        text.text = grades[index].Substring(1, grades[index].Length - 1);
        grade.color = colors[index];
        text.color = colors[index];
        bar.fillAmount = style / maxStyle;
    }

    public int GetGrade() {
        return (int)Mathf.Floor(style / maxStyle * (grades.Length - 1));
    }

    public void ChangeStyle(float amt) {
        style += amt;
        if (style < 0)
            style = 0;
        if (style > maxStyle)
            style = maxStyle;
        UpdateVisuals();
    }
}
