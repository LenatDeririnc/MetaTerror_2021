using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboDisplay : MonoBehaviour
{
    public TMP_Text comboText;

    public Color comboAddColor = Color.green;
    public Color comboBreakColor = Color.red;

    private int lastCombo;
    
    private void Update()
    {
        var currentCombo = RhythmGamePlayer.Instance.Stats.combo;
        
        if (lastCombo != currentCombo)
        {
            comboText.SetText(currentCombo
                .ToString()
                .PadLeft(4, '0'));
            
            comboText.rectTransform.DOKill();
            comboText.DOKill();
            
            if (lastCombo < currentCombo)
            {
                comboText.rectTransform.pivot = new Vector2(0.5f, 0.3f);
                comboText.rectTransform.DOPivot(new Vector2(0.5f, 0.5f), 0.1f);
                comboText.color = comboAddColor;
                comboText.DOColor(Color.white, 0.15f);
            }
            else
            {
                comboText.color = comboBreakColor;
                comboText.DOColor(Color.white, 0.15f);
                comboText.rectTransform.pivot = new Vector2(0.5f, 0.7f);
                comboText.rectTransform.DOPivot(new Vector2(0.5f, 0.5f), 0.1f);
            }

            lastCombo = currentCombo;
        }
    }
}
