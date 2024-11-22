namespace SmartHome.BusinessLogic.Models.Arguments.FiltersArguments;

public class PaginationArgs
{
    protected PaginationArgs(int? offset, int? limit)
    {
        ValidatedPaginationOffset(offset);
        ValidatedPaginationLimit(limit);

        Offset = offset!.Value;
        Limit = limit!.Value;
    }

    public int Offset { get; set; }
    public int Limit { get; set; }

    private static void ValidatedPaginationLimit(int? limit)
    {
        switch (limit)
        {
            case null:
                throw new ArgumentNullException(nameof(limit));
            case < 1:
                throw new ArgumentException("Limit must be greater than 0.");
        }
    }

    private static void ValidatedPaginationOffset(int? offset)
    {
        switch (offset)
        {
            case null:
                throw new ArgumentNullException(nameof(offset));
            case < 0:
                throw new ArgumentException("Offset must be greater or equal than 0.");
        }
    }
}
