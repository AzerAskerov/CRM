namespace CRM.Data.Enums
{
    public enum SurveyProcessEnum
    {
        /// <summary>
        /// Survey is not needed
        /// </summary>
        NotNeeded = 1,
        /// <summary>
        /// Survey is performed by dedicated survey user
        /// </summary>
        ByExpert = 2, // Surveyer
        /// <summary>
        /// Survey can be performed by mediator 
        /// </summary>
        ByMediator = 3, //Seller
        /// <summary>
        /// Survey can be performed by client 
        /// </summary>
        ByClient = 4
    }
}
