using TMPro;
using UnityEngine;

public class StatusDisplay : MonoBehaviour
{
    public TMP_Text mainText;
    public TMP_Text percentText;
    public RectTransform fillBar;
    
    public void SetStatus(string status, float percent)
    {
        mainText.text = status;
        percentText.text = $"{Mathf.Clamp01(percent) * 100:F1}%";
        fillBar.localScale = new Vector3(Mathf.Clamp01(percent), 1, 1);
    }
}