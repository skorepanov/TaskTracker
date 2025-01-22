namespace TaskTracker.Bll.DomainExceptions;

public class DomainEntityNotFoundException(Type domainEntityType, string message)
    : Exception(message)
{
    public Type DomainEntityType { get; } = domainEntityType;
}
