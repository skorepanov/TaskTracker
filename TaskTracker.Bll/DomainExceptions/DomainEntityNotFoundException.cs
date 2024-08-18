namespace TaskTracker.Bll.DomainExceptions;

public class DomainEntityNotFoundException : Exception
{
    public Type DomainEntityType { get; }

    public DomainEntityNotFoundException(Type domainEntityType, string message)
        : base(message)
    {
        this.DomainEntityType = domainEntityType;
    }
}
