using Invoicer.Core.Data.Models;
using Invoicer.Core.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Api.Features.BillableRecordFeature;

public class BillableRecordService
{
    private readonly IRepository<BillableRecord> _repository;

    public BillableRecordService(
        IRepository<BillableRecord> billableRecordRepository )
    {
        _repository = billableRecordRepository;
    }

    public async Task<IEnumerable<BillableRecord>> GetBillableRecordsForClient(
        Client client, 
        User? user = null )
    {
        return await _repository.Query.Where( r => r.ClientId == client.Id )
            .ToListAsync();
    }
}
