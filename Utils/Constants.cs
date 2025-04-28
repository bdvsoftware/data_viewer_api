namespace DataViewerApi.Utils;

public static class Constants
{
    public static readonly string VideoDirectory = Path.Combine(Directory.GetCurrentDirectory(), "videos");

    public static class CameraType
    {
        public static readonly string Onboard = "O";
        public static readonly string Helmet = "H";
    }


    public static class VideoStatus
    {
        public static readonly string Uploaded = "UPLOADED";
        public static readonly string Processing = "PROCESSING";
        public static readonly string Failed = "FAILED";
        public static readonly string Processed = "PROCESSED";
    }
}