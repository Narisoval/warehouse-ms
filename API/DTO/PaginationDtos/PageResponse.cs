using System.Text.Json.Serialization;

namespace Warehouse.API.DTO.PaginationDtos;

public class PageResponse<T>
{
    public IEnumerable<T> Data { get; init; }
    
    [JsonPropertyName("pagination")]
    public PaginationInfo PaginationInfo { get; init; }

    public PageResponse(IEnumerable<T> data, PaginationInfo paginationInfo)
    {
        Data = data;
        PaginationInfo = paginationInfo;
    }
}