using System;
using System.Collections.Concurrent;
using CRM.Operation.Models.Login;

namespace CRM.Operation.Cache
{
    /// <summary>
    /// In memory cache.
    /// </summary>
    public static class InMemoryCache
    {
        #region Cached items
        private static ConcurrentDictionary<Guid, UserSummaryShort> Users { get; set; } = new ConcurrentDictionary<Guid, UserSummaryShort>();

        #endregion

        /// <summary>
        /// Tries to retrieve user by given userGuid.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="user"></param>
        /// <returns>True or false</returns>
        public static bool TryGetUser(Guid userGuid, out UserSummaryShort user)
        {
            return Users.TryGetValue(userGuid, out user);
        }

        /// <summary>
        /// Retrieves user by given userGuid.
        /// If the user it not exists in our cache, it's getting user from IMS.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public static UserSummaryShort GetUser(Guid userGuid)
        {
            if (TryGetUser(userGuid, out var user))
                return user;

            GetUserInfoByUserGuidFromWebIms getUserInfoByUserGuidFromWebIms = new GetUserInfoByUserGuidFromWebIms();
            getUserInfoByUserGuidFromWebIms.Execute(userGuid);

            return Users[userGuid] = getUserInfoByUserGuidFromWebIms.UserSummaryShortModel;
        }
    }
}