namespace CRM.Data.Enums
{
    public enum SurveyStatusEnum : byte
    {
        /// <summary>
        /// Policy is issued, but vehicle is awaiting to visual survey.
        /// </summary>
        AwaitingSurvey = 1,

        /// <summary>
        /// Vehicle visual survey is not needed
        /// </summary>
        SurveyNotNeeded = 2,

        /// <summary>
        /// Survey was performed and vehicle is considered insurable
        /// </summary>
        Successful = 3,

        /// <summary>
        /// Survey was performed, but vehicle is declared not fit for a given insurance
        /// </summary>
        Rejected = 4,

        ///<summary>
        ///policy is preissued and waiting for to be issued in order to set survey status
        /// </summary>

        NotSetYet = 5,
    }
}
