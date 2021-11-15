using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
[Obsolete("Not using")]
public class AudioVisualizer : MonoBehaviour
{
    private AudioSource audioSource;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;
    private float currentUpdateTime = 0f;

    public float preLoudness;
    public float clipLoudness;
    private float[] clipSampleData;

    public static float[] sampleBand = new float[8];
    public static float[] sampleBufferBand = new float[8];
    float[] decreaseBuffer = new float[8];

    public float sampleScaleFactor = 10f;

    public float sizeFactor = 1;
    public float minSize = 1;
    public float maxSize = 500f;

    public float visualizeThreshold = 10f;
    public float lerpFactor = 5.0f;
    public VisualEffect[] visualEffect;

    public bool isAbsolute = false;
    void Start()
    {
        clipSampleData = new float[sampleDataLength];
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //currentUpdateTime += Time.deltaTime;
        //if(currentUpdateTime >= updateStep)
		{
            //currentUpdateTime = 0f;

            //AudioListener.GetSpectrumData(clipSampleData, 0, FFTWindow.Blackman);
            //audioSource.clip.GetData(clipSampleData, audioSource.timeSamples);
            
            //clipLoudness = 0f;

            //         foreach(var sample in clipSampleData)
            //{
            //             clipLoudness += Mathf.Abs(sample);
            //}

            //         clipLoudness /= sampleDataLength;
            //         clipLoudness *= sizeFactor;

            //         if(clipLoudness > visualizeThreshold)
            //{
            //             clipLoudness = Mathf.Clamp(clipLoudness, minSize, maxSize);

            //}
            //         else
            //{
            //             if(clipLoudness >= preLoudness)
            //	{
            //                 clipLoudness = Mathf.Lerp(preLoudness, 1, lerpFactor * Time.deltaTime);
            //	}
            //             else
            //	{
            //                 clipLoudness = Mathf.Lerp(clipLoudness, 1, lerpFactor * Time.deltaTime);
            //	}

            //}

            //         preLoudness = clipLoudness;
            //         visualEffect.SetFloat("_particleSize", clipLoudness);
            audioSource.GetSpectrumData(clipSampleData, 0, FFTWindow.Blackman);

            //44100/1024 = 43 hz per sample

            //0 -> 2 = 86 Hertz
            //1 -> 4 = 172Hertz (87 - 258hz)
            //2 -> 8 =  344Hertz (259hz - 602hz)
            //3 -> 16 = 688hertz (603hz - 1290hz)
            //4 -> 32 = 1376Hertz (1291hz - 2666hz)
            //5 -> 64 = 2752hertz (2667hz - 5418 hz)
            //6 -> 128 = 5504 hertz(5419hz - 10922hz)
            //7 -> 770 = 33110 hertz ( 10923hz - 44033hz)

            int count = 0;
            for(int i=0; i< 8; i++)
			{
                float average = 0;
                int sampleCount = (int)Mathf.Pow(2, i + 1);
                if(i==7)
				{
                    sampleCount = 770;
				}

                for(int j=0; j< sampleCount;j++)
				{
                    if(isAbsolute)
					{
                    average += Mathf.Abs(clipSampleData[count]) * (count + 1);
					}
                    else
					{
                        average += clipSampleData[count] * (count + 1);
                    }
                    count++;
				}

                average /= count;

                sampleBand[i] = Mathf.Max(1f,average * sampleScaleFactor);
			}

            //buffer
            for(int i=0; i< 8;i++)
			{
                if(sampleBand[i] > sampleBufferBand[i])
				{
                    sampleBufferBand[i] = sampleBand[i];
                    decreaseBuffer[i] = 0.005f;
				}

                if(sampleBand[i] < sampleBufferBand[i])
				{
                    sampleBufferBand[i] -= decreaseBuffer[i];
                    decreaseBuffer[i] *= 1.2f;
				}
			}

            clipLoudness = sampleBufferBand[1];

            for(int i=0;i<visualEffect.Length;i++)
			{
                visualEffect[i].SetFloat("_particleSize", clipLoudness);
			}
        }
    }
}
