namespace Ballcom.CustomerService.Application.DTOs;

public record CustomerOverviewDto(
    CustomerDto Customer,
    List<CustomerOrderDto> Orders,
    List<CustomerShipmentDto> Shipments
);
