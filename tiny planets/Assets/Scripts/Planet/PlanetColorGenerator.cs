using UnityEngine;

public class PlanetColorGenerator
{
    PlanetColorSettings colorSettings;
    Texture2D texture;
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(PlanetColorSettings colorSettings)
    {
        this.colorSettings = colorSettings;
        if (texture == null || texture.height != colorSettings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution * 2, colorSettings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(colorSettings.biomeColorSettings.noiseSettings);
    }

    public void UpdateElevation(PlanetMinMax elevationMinMax)
    {
        colorSettings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max, 0.0f, 0.0f));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1.0f) / 2.0f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - colorSettings.biomeColorSettings.noiseOffset) * colorSettings.biomeColorSettings.noiseStrength;
        float biomeIndex = 0.0f;
        int numBiomes = colorSettings.biomeColorSettings.biomes.Length;
        float blendRange = colorSettings.biomeColorSettings.blendAmount / 2.0f + 0.001f;


        for (int i = 0; i < numBiomes; i++)
        {
            float distance = heightPercent - colorSettings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.Lerp(-blendRange, blendRange, distance);
            biomeIndex *= 1.0f - weight;
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;

        foreach (PlanetColorSettings.BiomeColorSettings.Biome biome in colorSettings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < textureResolution * 2; i++)
            {
                Color gradientColor;

                if (i < textureResolution)
                {
                    gradientColor = colorSettings.biomeColorSettings.oceanGradient.Evaluate(i / (textureResolution - 1.0f));
                }
                else
                {
                    gradientColor = biome.biomeGradient.Evaluate((i - textureResolution) / (textureResolution - 1.0f));
                }
                Color tintColor = biome.biomeTint;
                colors[colorIndex] = gradientColor * (1.0f - biome.biomeTintPercent) + tintColor * biome.biomeTintPercent;
                colorIndex++;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        colorSettings.planetMaterial.SetTexture("_planetTexture", texture);
    }
}