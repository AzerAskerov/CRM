using System;
using System.Xml;
using System.Xml.Serialization;

namespace CRM.Operation.Queue
{
    /// <summary>
    /// Helper functions for queue message serialization
    /// </summary>
    public static class QueueMessageSerializer
    {
        /// <summary>
        /// Serializes a message of any type
        /// </summary>
        /// <param name="message">The message to be serialized</param>
        /// <returns>serialized string</returns>
        /// <exception cref="System.ArgumentNullException">message is null</exception>
        public static XmlDocument SerializeMessage(object message)
        {
            if (message == null) throw new ArgumentNullException("message");
            var xmlContent = new XmlDocument();
            var nav = xmlContent.CreateNavigator();
            using (var writer = nav.AppendChild())
            {
                var serializer = new XmlSerializer(message.GetType());
                serializer.Serialize(writer, message);
            }
            return xmlContent;
        }

        /// <summary>
        /// Deserializing message body into a custom object specified by parameters
        /// </summary>
        public static object DeserializeMessage(XmlDocument messageContent, Type messageType)
        {
            if (messageContent == null) return null;

            using (var reader = new XmlNodeReader(messageContent))
            {
                var serializer = new XmlSerializer(messageType);
                if (!serializer.CanDeserialize(reader)) return null;
                var eventDataObject = serializer.Deserialize(reader);
                return eventDataObject;
            }
        }
    }
}
