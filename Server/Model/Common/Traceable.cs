public abstract class Traceable
{
    private DateTime _updated;

    public DateTime CreatedAt
    {
        get; private set;
    }

    public DateTime UpdatedAt
    {
        get { return _updated; }
        private set { _updated = value; }
    }

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void CreatedNow()
    {
        CreatedAt = DateTime.UtcNow;
        Update();
    }
}