namespace DynamicImageAPI;

public static class CounterManager
{
    private static Dictionary<string, long> _savedCounters = new Dictionary<string, long>() {{"0", 1111110}, {"1", 9999998}, {"2", 111111111111110}};

    public static long GetNewCounterValue(string counterId)
    {
        if (_savedCounters.ContainsKey(counterId))
        {
            _savedCounters[counterId]++;
        }
        else
        {
            _savedCounters[counterId] = 1;
        }
        
        return _savedCounters[counterId];
    }
}