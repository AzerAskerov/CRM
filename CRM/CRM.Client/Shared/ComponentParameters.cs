using System.Collections.Generic;


namespace CRM.Client.Shared
{
    public class ComponentParameters
    {
        internal Dictionary<string, object> _parameters;

        public ComponentParameters()
        {
            _parameters = new Dictionary<string, object>();
        }

        public void Add(string parameterName, object value)
        {
            _parameters[parameterName] = value;
        }

        public T Get<T>(string parameterName)
        {
            if (_parameters.TryGetValue(parameterName, out var value))
            {
                return (T)value;
            }

            throw new KeyNotFoundException($"{parameterName} does not exist in modal parameters");
        }

        public T TryGet<T>(string parameterName)
        {
            if (_parameters.TryGetValue(parameterName, out var value))
            {
                return (T)value;
            }

            return default;
        }
    }
}
