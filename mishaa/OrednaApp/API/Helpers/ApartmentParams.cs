namespace API.Helpers;

public class ApartmentParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    //public int CurrentUserId { get; set; }
    public string? Price { get; set; }
    public string? People { get; set; }
    public string? HasTV { get; set; }
    public string? HasWifi { get; set; }
    public string? IsAvaliable { get; set; }
    //public string Search { get; set; }
}