using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class AudioVFXVisualee : MonoBehaviour
{
    private VisualEffect visualEffect;

    [Range(0,7)]
    [SerializeField] private int audioBandIndex = -1;
    [SerializeField] private string propertyName;


    public bool isUsingAudioBuffer;
    public float multiplier = 1.0f;
    public float minValue = 1f;
    public float maxValue = 10f;

    [Range(0, 1)]
    [SerializeField] private float threshold = 0;
    private float currentValueForThrehold;
	private void Awake()
	{
        visualEffect = GetComponent<VisualEffect>();
        if(propertyName == "")
		{
            Debug.LogError("No Property Name set");
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (audioBandIndex == -1) return;


        float inputValue = isUsingAudioBuffer ? AudioSpectrumDataManager.audioBandBuffer[audioBandIndex] : AudioSpectrumDataManager.audioBand[audioBandIndex];
        
        if(inputValue > threshold)
		{
            inputValue *= multiplier;
            inputValue = Mathf.Clamp(inputValue, minValue, maxValue);
		}
        else
		{
            float currentValue = visualEffect.GetFloat(propertyName);
            inputValue = Mathf.Lerp(currentValue, minValue, Time.deltaTime * 2f);
		}

        visualEffect.SetFloat(propertyName, inputValue);
    }
}
