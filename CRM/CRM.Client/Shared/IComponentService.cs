using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Shared
{
    public interface IComponentService
    {
        /// <summary>
        /// Shows a component containing a <typeparamref name="TComponent"/> with the specified <paramref name="title"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="title">Component title</param>
        /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed</param>
        ComponentReference Show<TComponent>(string title, ComponentParameters parameters);

        /// <summary>
        /// Shows a modal containing a <typeparamref name="TComponent"/> with the specified <paramref name="title"/> .
        /// </summary>
        /// <param name="title">Component title</param>
        ComponentReference Show<TComponent>(string title);
    }
}
