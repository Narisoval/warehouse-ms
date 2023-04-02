using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.DTO.PaginationDtos;

public class PaginationQueryParameters
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be greater than 0.")]
    public int PageIndex { get; set; } = 1;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0.")]
    public int PageSize { get; set; } = 15;
}
