using DataViewerApi.Utils;

namespace DataViewerApi.Service;

public abstract class BaseService
{
    protected int ExtractLapNumber(string lap)
    {
        if (string.IsNullOrWhiteSpace(lap))
            throw new ArgumentException("LAP NULL");

        if (Constants.NOT_DETECTED_LAP.Equals(lap))
        {
            return 0;
        }

        var parts = lap.Split('/');
        if (parts.Length != 2 || !int.TryParse(parts[0], out int lapNumber))
            throw new FormatException("Lap format not valid.");

        return lapNumber;
    }
}