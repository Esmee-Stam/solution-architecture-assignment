using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Application.DTOs;
using Ballcom.CustomerService.Domain.Domain;
using Ballcom.CustomerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.CustomerService.Infrastructure.Repository;

public class CustomerRepository(CustomerServiceDbContext context) : ICustomerRepository
{
    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await context.Customers.AddAsync(customer, cancellationToken);
    }

    public async Task<Customer?> GetByIdEntityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Customers.FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await context.Customers.FirstOrDefaultAsync(customer => customer.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Customers
            .Where(c => c.Id == id)
            .Select(c => new CustomerDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.FirstName + " " + c.LastName,
                c.PhoneNumber,
                c.CompanyName,
                c.Address,
                c.IdentityUserId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CustomerDto?> GetCustomerByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await context.Customers
            .Where(c => c.PhoneNumber == phoneNumber)
            .Select(c => new CustomerDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.FirstName + " " + c.LastName,
                c.PhoneNumber,
                c.CompanyName,
                c.Address,
                c.IdentityUserId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<CustomerDto>> GetCustomersAsync(
        int pageNumber,
        int pageSize,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();
            query = query.Where(c =>
                c.FirstName.Contains(normalizedSearch)
                || c.LastName.Contains(normalizedSearch)
                || (c.CompanyName != null && c.CompanyName.Contains(normalizedSearch))
                || (c.PhoneNumber != null && c.PhoneNumber.Contains(normalizedSearch))
                || (c.Address != null && c.Address.Contains(normalizedSearch)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CustomerDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.FirstName + " " + c.LastName,
                c.PhoneNumber,
                c.CompanyName,
                c.Address,
                c.IdentityUserId))
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid? excludedCustomerId = null, CancellationToken cancellationToken = default)
    {
        return await context.Customers.AnyAsync(customer =>
            customer.PhoneNumber == phoneNumber
            && (!excludedCustomerId.HasValue || customer.Id != excludedCustomerId.Value), cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    private static CustomerDto ToDto(Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            $"{customer.FirstName} {customer.LastName}".Trim(),
            customer.PhoneNumber,
            customer.CompanyName,
            customer.Address,
            customer.IdentityUserId);
    }
}
