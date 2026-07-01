namespace Ballcom.CustomerService.Application.Queries.GetCustomers;

public record GetCustomersQuery(int PageNumber = 1, int PageSize = 50, string? Search = null);
