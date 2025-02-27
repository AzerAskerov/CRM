using System;

using Microsoft.AspNetCore.Components;


namespace CRM.Client.Shared
{
    public class ComponentService : IComponentService
    {
        internal event Action<ComponentReference> OnModalInstanceAdded;
        /// internal event Action<ComponentReference, ModalResult> OnModalCloseRequested;

        /// <summary>
        /// Shows the component with the component type using the specified title.
        /// </summary>
        /// <param name="title">Component title.</param>
        public ComponentReference Show<T>(string title)
        {
            return Show<T>(title, new ComponentParameters());
        }

        /// <summary>
        /// Shows the component with the component type using the specified <paramref name="title"/>,
        /// passing the specified <paramref name="parameters"/>.
        /// </summary>
        /// <param name="title">Component title.</param>
        /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
        public ComponentReference Show<T>(string title, ComponentParameters parameters)
        {
            return Show(typeof(T), title, parameters);
        }

        public ComponentReference Show(Type contentComponent, string title, ComponentParameters parameters)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(contentComponent))
            {
                throw new ArgumentException($"{contentComponent.FullName} must be a Blazor Component");
            }

            var modalInstanceId = Guid.NewGuid();
            var modalContent = new RenderFragment(builder =>
            {
                var i = 0;
                builder.OpenComponent(i++, contentComponent);
                foreach (var parameter in parameters._parameters)
                {
                    builder.AddAttribute(i++, parameter.Key, parameter.Value);
                }
                builder.CloseComponent();
            });

            var modalInstance = new RenderFragment(builder =>
            {
                builder.OpenComponent<ComponentInstance>(0);
                builder.AddAttribute(1, "Content", modalContent);
                builder.AddAttribute(2, "Id", modalInstanceId);
                builder.CloseComponent();
            });

            var modalReference = new ComponentReference(modalInstanceId, modalInstance);

            OnModalInstanceAdded?.Invoke(modalReference);

            return modalReference;
        }
    }
}
