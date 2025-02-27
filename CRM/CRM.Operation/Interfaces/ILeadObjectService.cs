using CRM.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Operation.Interfaces
{
    public interface ILeadObjectService
    {
        void CreateLeadObject(ClientRef clientIncoming, ClientRef clientFound);
        void MergeLeadObject(List<LeadObject> incomingLead, List<LeadObject> existingLead, ClientRef clientRef);
    }
}