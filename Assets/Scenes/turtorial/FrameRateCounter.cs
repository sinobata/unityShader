using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display;

    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f;

    public enum DisplayMode { FPS, MS }
    [SerializeField]
    DisplayMode displayMode = DisplayMode.FPS;

    int frame;
    float duration, bestDuration = float.MaxValue, worstDuration;
    void Update()
    {
        float flameDuration = Time.unscaledDeltaTime;
        frame += 1;
        duration += flameDuration;
        if (flameDuration < bestDuration)
        {
            bestDuration = flameDuration;
        }
        if (flameDuration > worstDuration)
        {
            worstDuration = flameDuration;
        }

        if (duration >= sampleDuration)
        {
            if (displayMode == DisplayMode.FPS)
            {
                display.SetText("FPS\n{0:0}\n{1:0}\n{2:0}", frame / duration, 1f / bestDuration, 1f / worstDuration);

            }
            else
            {
                display.SetText("MS\n{0:1}\n{1:1}\n{2:1}", 1000f * duration / frame, 1000f * bestDuration, 1000f * worstDuration);

            }
            frame = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
        }
    }
}
