namespace SmartHome.BusinessLogic;

public abstract class DateTimeProvider
{
    public static DateTime? ActiveDate { get; set; }

    public static DateTime Now => ActiveDate ?? DateTime.Now;
}
