namespace Sgcae.Domain.AccessLogs;

public interface IAccessLogRepository
{
    void Add(AccessLog accessLog);
}
