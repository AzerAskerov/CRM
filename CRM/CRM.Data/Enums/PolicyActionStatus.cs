

namespace CRM.Data.Enums
{
    public enum PolicyActionStatus
    {
        /// <summary>
        /// Policy action is new.
        /// <para>Value of this enum is <c>1</c>.</para>
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Policy is created, but not in force yet, becuse it 
        /// </summary>
        PreIssued = 2,

        /// <summary>
        /// Policy action is active (valid).
        /// <para>Value of this enum is <c>3</c>.</para>
        /// </summary>
        Issued = 3,

        /// <summary>
        /// Policy was canceled(rejected), before it became issued.
        /// <para>Value of this enum is <c>5</c>.</para>
        /// </summary>
        Rejected = 5,

        /// <summary>
        /// 
        /// <para>Value of this enum is <c>6</c>.</para>
        /// </summary>
        Annuled = 6,

        /// <summary>
        /// 
        /// <para>Value of this enum is <c>7</c>.</para>
        /// </summary>
        Terminated = 7,
    }
}
