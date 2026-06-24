namespace RealEstate.Shared.Common
{
    public sealed class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = [];
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasNext => Page < TotalPages;
        public bool HasPrevious => Page > 1;

        public static PagedResult<T> Create(IReadOnlyList<T> items, int totalCount, int page, int pageSize) =>
            new()
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

        public static PagedResult<T> Empty(int page, int pageSize) =>
            new() { Items = [], TotalCount = 0, Page = page, PageSize = pageSize };
    }
}