using UnityEngine;

public class PlanetShapeGenerator
{
    private PlanetShapeSettings settings;
    private INoiseFilter[] noiseFilters;
    public PlanetMinMax elevationMinMax;

    public void UpdateSettings(PlanetShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }

        elevationMinMax = new PlanetMinMax();
    }

    public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0.0f;
        float elevation = 0.0f;

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1.0f;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        // elevation = 1 + elevation;
        elevationMinMax.AddValue(elevation);
        return  elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        float elevation = Mathf.Max(0.0f, unscaledElevation);
        elevation = settings.planetRadius * (1.0f + elevation);
        return elevation;
    }
}