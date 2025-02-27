
namespace CRM.Data.Enums
{
    public enum PolicyStatus
    {
        /// <summary>
        /// Draft of a policy.
        /// <para>Value of this enum is <c>1</c>.</para>
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Policy is issued.
        /// <para>Value of this enum is <c>2</c>.</para>
        /// </summary>
        Issued = 2,

        /// <summary>
        /// Policy is annuled.
        /// <para>Value of this enum is <c>4</c>.</para>
        /// </summary>
        Annulled = 4,

        /// <summary>
        /// Policy is terminated.
        /// <para>Value of this enum is <c>5</c>.</para>
        /// </summary>
        Terminated = 5,

        /// <summary>
        /// Policy is waiting to be paid. Then it will be activated and become <see cref="Issued"/>
        /// <para>Value of this enum is <c>7</c>.</para>
        /// </summary>
        PreIssued = 7,

        /// <summary>
        /// Policy was canceled before it became issued.
        /// <para>Value of this enum is <c>8</c>.</para>
        /// </summary>
        Rejected = 8
    }
}
