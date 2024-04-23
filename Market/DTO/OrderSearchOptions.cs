namespace Market.DTO;

public record OrderSearchOptions(
    Guid SellerId,
    bool IncludeAll
);

public enum OrderSearchType
{
    OnlyActive,
    All
}
