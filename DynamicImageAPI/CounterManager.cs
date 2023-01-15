using System.Reflection;

namespace DynamicImageAPI;

public static class DataManager
{
    public static long GetNewCounterValue(string counterId)
    {
        var counterDb = SQLiteDBAccess.SQLiteDBAccess.Instance("Images", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

        if (!counterDb.CheckForExistingElementByAttribute("Counters", "Name", counterId))
        {
            counterDb.Insert("Counters", "Name, Count", $"'{counterId}', 0");
        }
        var reader = counterDb.GetByAttribute("Counters", "Name", counterId);
        if (!reader.Read()) return 0;
        long? counterPrimaryKey = reader.GetInt64(0);
        long? counterValue = reader.GetInt64(2);
        counterDb.CloseDBFile(reader);
        counterValue++;
        counterDb.UpdateSingle("Counters", "Id", counterPrimaryKey.ToString(), "Count", counterValue.ToString());
        return counterValue ?? 0;
    }
}