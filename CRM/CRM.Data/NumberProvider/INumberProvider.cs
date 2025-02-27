namespace CRM.Data.NumberProvider
{
    /// <summary>
    /// Template of number provider.
    /// </summary>
    public interface INumberProvider
    {
        string Prefix { get; }
        string GetUniqueId();
        string GetUniqueId(string prefix);
    }
}