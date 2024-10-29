using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject progressBarGameObject;
    private IShowProgressBar progressBarInfo;
    [SerializeField] private Image bar;

    void Awake()
    {
        progressBarInfo = progressBarGameObject.GetComponent<IShowProgressBar>();
        Assert.IsNotNull(progressBarInfo);
        progressBarInfo.OnValueChanged += UpdateBar;
    }

    void UpdateBar(float current, float max)
    {
        // Debug.Log($"{current}/{max}={current / max}");
        bar.fillAmount = current / max;
    }

    void OnDestroy()
    {
        if(progressBarInfo != null)
            progressBarInfo.OnValueChanged -= UpdateBar;
    }
}
