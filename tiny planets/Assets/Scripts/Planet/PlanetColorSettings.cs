using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlanetColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;

    [System.Serializable]
    public class BiomeColorSettings
    {
        public Biome[] biomes;
        public NoiseSettings noiseSettings;
        public float noiseOffset;
        public float noiseStrength;

        [Range(0.0f, 1.0f)]
        public float blendAmount;
        public Gradient oceanGradient;

        [System.Serializable]
        public class Biome
        {
            public Gradient biomeGradient;

            [Range(0.0f, 1.0f)]
            public float startHeight;
            public Color biomeTint;

            [Range(0.0f, 1.0f)]
            public float biomeTintPercent;
        }
    }
}
