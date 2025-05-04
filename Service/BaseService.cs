namespace DataViewerApi.Service;

public abstract class BaseService
{
    protected int ExtractLapNumber(string lap)
    {
        if (string.IsNullOrWhiteSpace(lap))
            throw new ArgumentException("LAP NULL");

        var parts = lap.Split('/');
        if (parts.Length != 2 || !int.TryParse(parts[0], out int lapNumber))
            throw new FormatException("Lap format not valid. Must be 'x/xx' or 'xx/xx'.");

        return lapNumber;
    }
}