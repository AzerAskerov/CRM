using CRM.Operation.Attributes;
using CRM.Operation.Enums;
using CRM.Operation.SourceLoaders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Zircon.Core.Attributes;
using Zircon.Core.OperationModel;

namespace CRM.Operation.Models.RequestModels
{
    public class TagContract : IModel
    {

        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; } 
    }
}
