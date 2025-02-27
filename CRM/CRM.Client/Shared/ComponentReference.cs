using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Client.Shared
{
    public class ComponentReference
    {
        public string Title { get; set; }
        public ComponentReference(Guid modalInstanceId, RenderFragment modalInstance)
        {
            Id = modalInstanceId;
            ModalInstance = modalInstance;
          
        }

        internal Guid Id { get; }
        internal RenderFragment ModalInstance { get; }

    }
}
