using UnityEngine;

public class RidgedNoiseFilter : INoiseFilter
{
    private NoiseSettings.RidgedNoiseSettings noiseSettings;
    private Noise noise = new Noise();

    public RidgedNoiseFilter(NoiseSettings.RidgedNoiseSettings noiseSettings)
    {
        this.noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0.0f;
        float frequency = noiseSettings.baseRoughness;
        float amplitude = 1.0f;
        float weight = 1.0f;

        for (int i = 0; i < noiseSettings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + noiseSettings.center));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * noiseSettings.weightMultiplier);

            noiseValue += v * amplitude;
            frequency *= noiseSettings.roughness;
            amplitude *= noiseSettings.persistance;
        }

        // noiseValue = Mathf.Max(0.0f, noiseValue - noiseSettings.minValue);
        noiseValue = noiseValue - noiseSettings.minValue;
        return noiseValue * noiseSettings.strength;
    }
}