using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Ridged };
    public FilterType filterType;

    [ConditionalHide("filtertype", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;

    [ConditionalHide("filtertype", 1)]
    public RidgedNoiseSettings ridgedNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1.0f;
        public float baseRoughness = 1.0f;
        public float roughness = 2.0f;
        public float minValue;
        
        [Range(1, 8)]
        public int numLayers = 1;
        public float persistance = 0.5f;
        public Vector3 center;
    }

    [System.Serializable]
    public class RidgedNoiseSettings : SimpleNoiseSettings
    {

        public float weightMultiplier = 0.8f;
    }


}