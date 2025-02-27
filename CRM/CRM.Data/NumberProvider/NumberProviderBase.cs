namespace CRM.Data.NumberProvider
{
    /// <summary>
    /// Base of number provider.
    /// </summary>
    public abstract class NumberProviderBase : INumberProvider
    {
        public abstract string Prefix { get; }
        
        public abstract string GetUniqueId();

        public abstract string GetUniqueId(string prefix);

        /// <summary>
        /// Returns formatted 8 character string with value and prefix.
        /// <example> 00000001 </example>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        protected virtual string Format(int value, string prefix = "")
        {
            return $"{prefix}{value:00000000}";
        }
    }
}