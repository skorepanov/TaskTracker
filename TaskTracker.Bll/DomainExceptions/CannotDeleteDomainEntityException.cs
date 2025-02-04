namespace TaskTracker.Bll.DomainExceptions;

public class CannotDeleteDomainEntityException(Type domainEntityType,
                                               int domainEntityId,
                                               string message)
    : Exception(message)
{
    public Type DomainEntityType { get; } = domainEntityType;
    public int DomainEntityId { get; } = domainEntityId;
}