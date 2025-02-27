using CRM.Client.States;

namespace CRM.Client.States
{
    /// <summary>
    /// Parametrized message before localization.
    /// This message can be passed between system parts, written to technical log as is
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets the message text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Message parameters.
        /// </summary>
        public object ObjectMap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="text">The text or resource id. <see cref="DBResources"/></param>
        /// <param name="objectMap">The parameter map.</param>
        public Message(string text, object objectMap = null)
        {
            Text = text;
            ObjectMap = objectMap;
        }

        public Message()
        {

        }

        public override string ToString()
        {
            return Text;
        }
    }
}
