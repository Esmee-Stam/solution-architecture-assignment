# Run this from the repository root: C:\dev\solution-architecture-assignment
# This removes leftover public customer write commands and switches the import consumer
# back to the internal SyncCustomerService, so CustomerService stays read-only over HTTP.

$ErrorActionPreference = "Stop"

$repo = Get-Location
Write-Host "Running CustomerService read-only cleanup in: $repo"

# 1. Remove obsolete command folders left behind by drag/drop copy.
$commandRoot = ".\src\Ballcom.CustomerService.Application\Commands"
$obsoletePaths = @(
    "$commandRoot\CreateCustomer",
    "$commandRoot\UpdateCustomer",
    "$commandRoot\UpsertImportedCustomer",
    ".\src\Ballcom.CustomerService.WebAPI\Models\CreateCustomerModel.cs",
    ".\src\Ballcom.CustomerService.WebAPI\Models\UpdateCustomerModel.cs"
)

foreach ($path in $obsoletePaths) {
    if (Test-Path $path) {
        Remove-Item $path -Recurse -Force
        Write-Host "Removed $path"
    }
}

if ((Test-Path $commandRoot) -and -not (Get-ChildItem $commandRoot -Force)) {
    Remove-Item $commandRoot -Force
    Write-Host "Removed empty $commandRoot"
}

# 2. Remove UpsertImportedCustomerHandler registration/using from Program.cs.
$programPath = ".\src\Ballcom.CustomerService.WebAPI\Program.cs"
if (Test-Path $programPath) {
    $program = Get-Content $programPath
    $program = $program | Where-Object {
        $_ -notmatch "Ballcom\.CustomerService\.Application\.Commands\.UpsertImportedCustomer" -and
        $_ -notmatch "UpsertImportedCustomerHandler"
    }
    $program | Set-Content $programPath
    Write-Host "Cleaned Program.cs"
}

# 3. Restore CustomerImportedConsumer to use SyncCustomerService instead of command handler.
$consumerPath = ".\src\Ballcom.CustomerService.Infrastructure\Messaging\CustomerImportedConsumer.cs"
$consumerContent = @'
using Ballcom.CustomerService.Application.Services;
using Ballcom.CustomerService.Domain.Domain;
using Events.CustomerServiceEvents;
using MassTransit;

namespace Ballcom.CustomerService.Infrastructure.Messaging;

public class CustomerImportedConsumer(SyncCustomerService service) : IConsumer<CustomerImportedEvent>
{
    public async Task Consume(ConsumeContext<CustomerImportedEvent> context)
    {
        var message = context.Message;

        await service.UpsertCustomerAsync(new Customer
        {
            FirstName = message.FirstName,
            LastName = message.LastName,
            CompanyName = message.CompanyName,
            PhoneNumber = message.PhoneNumber,
            Address = message.Address,
            IdentityUserId = message.IdentityUserId
        });
    }
}
'@

$consumerContent | Set-Content $consumerPath
Write-Host "Updated CustomerImportedConsumer.cs"

Write-Host "Cleanup complete. Now run: dotnet build .\Solution-Architecture-Assignment.slnx"
