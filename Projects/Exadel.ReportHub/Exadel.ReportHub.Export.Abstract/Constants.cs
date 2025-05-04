namespace Exadel.ReportHub.Export.Abstract;

public static class Constants
{
    public const string DateFormat = "yyyy-MM-dd";

    public static class File
    {
        public static class Extension
        {
            public const string Pdf = ".pdf";
            public const string Csv = ".csv";
            public const string Excel = ".xlsx";
        }

        public static class ContentType
        {
            public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
    }
}
