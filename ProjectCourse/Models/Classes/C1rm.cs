using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectCourse.Models
{
    public partial class C1RM
    {
        private aspnetEntities db = new aspnetEntities();

        /// <summary>
        /// Description:
        ///     Returns true if there is a unfinished plan for the user, and false if all the plans are finished.
        /// History:
        ///     Amir Naji   27/02/2017
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool HaveAlreadyWeight(string userId)
        {
            //return db.C1RM.FirstOrDefault(x => x.UserID == userId && x.RMDate == DateTime.Now.Date) != null ? true : false;
            return db.C1RM.Where(x => x.UserID == userId && DbFunctions.TruncateTime(x.RMDate) == DateTime.Now.Date) != null ? true : false;
        }
    }
}