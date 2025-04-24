namespace DataViewerApi.Utils;

public static class Constants
{
    public static readonly string VideoDirectory = Path.Combine(Directory.GetCurrentDirectory(), "videos");

    public static class CameraType
    {
        public static readonly string Onboard = "O";
        public static readonly string Helmet = "H";
    }
}