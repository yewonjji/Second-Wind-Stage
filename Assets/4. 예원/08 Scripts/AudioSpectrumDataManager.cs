using UnityEngine;


[RequireComponent(typeof(AudioListener))]
public class AudioSpectrumDataManager : MonoBehaviour
{
    private float[] spectrumData = new float[1024];

    public static float[] rawSampleBand = new float[8];
    public static float[] rawSampleBufferBand = new float[8];
    private float[] decreaseBuffer = new float[8];

    public float[] highestSampleBand = new float[8];
    public float initialHighest = 0;
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

	private void Start()
	{
        AudioProfile(initialHighest);
	}

    //처음 highest를 조정해준다.
    private void AudioProfile(float _initialHighest)
	{
        for (int i = 0; i < 8; i++)
		{
            highestSampleBand[i] = _initialHighest;
		}
	}
	void Update()
    {
        GetSampleBand();
        GetBufferBand();
        GetAudioBand();
    }

    private void GetSampleBand()
	{
        AudioListener.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);
            if (i == 7)
            {
                sampleCount = 770;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += spectrumData[count] * (count + 1);
                count++;
            }

            average /= count;

            rawSampleBand[i] = average * 10f;
        }
    }

    private void GetBufferBand()
	{
        for(int i=0; i< 8;i++)
		{
            if(rawSampleBand[i] > rawSampleBufferBand[i])
			{
                rawSampleBufferBand[i] = rawSampleBand[i];
                decreaseBuffer[i] = 0.005f;
			}

            if(rawSampleBand[i] < rawSampleBufferBand[i])
			{
                rawSampleBufferBand[i] -= decreaseBuffer[i];
                decreaseBuffer[i] *= 1.2f;
			}
		}
	}

    private void GetAudioBand()
	{
        for(int i=0; i< 8;i++)
		{
            if(rawSampleBand[i] > highestSampleBand[i])
			{
                highestSampleBand[i] = rawSampleBand[i];
			}

            audioBand[i] = rawSampleBand[i] / highestSampleBand[i];
            audioBandBuffer[i] = rawSampleBufferBand[i] / highestSampleBand[i];
		}
	}
}