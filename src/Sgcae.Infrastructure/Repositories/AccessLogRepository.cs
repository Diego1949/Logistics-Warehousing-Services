using Sgcae.Domain.AccessLogs;
using Sgcae.Infrastructure.Persistence;

namespace Sgcae.Infrastructure.Repositories;

internal sealed class AccessLogRepository(SgcaeDbContext context) : IAccessLogRepository
{
    public void Add(AccessLog accessLog) => context.AccessLogs.Add(accessLog);
}
