using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Data.Enums
{
    public enum UserAccessScope
    {
        /// <summary>
        /// User can see only own entites policies/invoices
        /// </summary>
        User = 1,
        /// <summary>
        /// User can see only his unit's entites policies/invoices
        /// </summary>
        Unit = 2,
        /// <summary>
        /// User can see only his organization's entites policies/invoices
        /// </summary>
        Organization = 3,
        /// <summary>
        /// User can see all entites policies/invoices
        /// </summary>
        All = 4,

        /// <summary>
        /// User can see channel's entites policies/invoices
        /// </summary>
        Channel = 5



    }
}
