public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings noiseSettings)
    {
        switch(noiseSettings.filterType)
        {
            case NoiseSettings.FilterType.Simple:
                return new SimpleNoiseFilter(noiseSettings.simpleNoiseSettings);
            case NoiseSettings.FilterType.Ridged:
                return new RidgedNoiseFilter(noiseSettings.ridgedNoiseSettings);
            default:
                return null;
        }
    }
}