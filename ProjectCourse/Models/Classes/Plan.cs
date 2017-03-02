using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectCourse.Models
{
    public partial class Plan
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
        public bool HaveUnfinishedPlan(string userId)
        {
            return db.Plans.FirstOrDefault(x => x.UserID == UserID && x.FinishDate != null) == null ? true : false;
        }
    }
}