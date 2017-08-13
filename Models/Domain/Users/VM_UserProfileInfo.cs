using CessInvest.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CessInvest.Models.Domain.Users
{
    public class VM_UserProfileInfo
    {
        #region ПОЛЯ и СВОЙСТВА
        public VM_User User { get; set; }
        #endregion

        public VM_UserProfileInfo()
        {
            User = new VM_User();
            Clear();
        }
        public void Clear()
        {
            User.Clear();
        }
    }
}