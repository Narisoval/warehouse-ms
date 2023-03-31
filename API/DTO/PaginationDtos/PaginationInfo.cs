namespace Warehouse.API.DTO.PaginationDtos;

public class PaginationInfo
{
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalRecords { get; private set; }

    public PaginationInfo(int pageIndex, int pageSize, int totalRecords)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalPages = Math.DivRem(totalRecords, pageSize, out int remainder) + (remainder > 0 ? 1 : 0);
        TotalRecords = totalRecords;
    }
}