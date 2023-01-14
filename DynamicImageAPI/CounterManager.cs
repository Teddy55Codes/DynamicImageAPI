namespace DynamicImageAPI;

public static class CounterManager
{
    private static Dictionary<string, long> _savedCounters = new Dictionary<string, long>();

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