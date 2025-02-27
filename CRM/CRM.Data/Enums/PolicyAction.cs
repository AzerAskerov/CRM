

namespace CRM.Data.Enums
{
    public enum PolicyAction
    {
        /// <summary>
        /// Policy issue action.
        /// <para>Value of this enum is <c>2</c>.</para>
        /// </summary>
        PolicyIssuance = 2,

        /// <summary>
        /// 
        /// <para>Value of this enum is <c>3</c>.</para>
        /// </summary>
        AppendixIssuance = 3,

        ///// <summary>
        ///// 
        ///// <para>Value of this enum is <c>4</c>.</para>
        ///// </summary>
        //ProposalIssuance = 4,

        /// <summary>
        /// 
        /// <para>Value of this enum is <c>5</c>.</para>
        /// </summary>
        Renewal = 5,

        /// <summary>
        /// 
        /// <para>Value of this enum is <c>6</c>.</para>
        /// </summary>
        DuplicateIssuance = 6,

        /// <summary>
        /// 
        /// <para>Value of this enum is <c>7</c>.</para>
        /// </summary>
        Annulment = 7,

        /// <summary>
        /// Termination of policy
        /// <para>Value of this enum is <c>8</c>.</para>
        /// </summary>
        Termination = 8,

        /// <summary>
        /// 
        /// <para>Value of this enum is <c>9</c>.</para>
        /// </summary>
        Expiration = 9,

        /// <summary>
        /// Termination of policy action
        /// <para>Value of this enum is <c>10</c>.</para>
        /// </summary>
        AppendixTermination = 10,

        /// <summary>
        /// Termination of policy and all its actions.
        /// <para>Value of this enum is <c>11</c>.</para>
        /// </summary>
        CaseTermination = 11
    }
}
