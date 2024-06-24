using Auxiliars;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GeneralMessenger : MonoBehaviour
{
    private TextMeshProUGUI m_textMesh;
    private SpartanTimer m_fadeOutTimer;

    private void Start()
    {
        this.m_textMesh = GetComponent<TextMeshProUGUI>();
        this.m_fadeOutTimer = new SpartanTimer(TimeMode.Framed);
        this.m_textMesh.text = string.Empty;
        this.m_textMesh.alpha = 0f;
    }

    private void Update()
    {
        if (!this.m_fadeOutTimer.Started) return;
        this.m_textMesh.alpha = SpartanMath.Lerp(this.m_textMesh.alpha, 0f, Time.deltaTime);
        if (this.m_textMesh.alpha <= 0.05f) //Arbitrary threshold
        {
            this.m_fadeOutTimer.Stop();
            this.m_textMesh.alpha = 0f;
        }
    }

    public void SetText(string text, Color color = default)
    {
        this.m_textMesh.text = text;
        this.m_textMesh.color = color;
        this.m_textMesh.alpha = 1f;
        this.m_fadeOutTimer.Reset();
    }

}
