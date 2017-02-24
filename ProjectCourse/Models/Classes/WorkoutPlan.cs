using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectCourse.Models
{
    public partial class WorkoutPlan
    {
        private aspnetEntities db = new aspnetEntities();

        /// <summary>
        /// Description:
        ///     Returns a list of Workouts for the last plan, if there isn't any plan.
        /// History:
        ///     Amir Naji   22/02/2017
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WorkoutPlan> GetLastWorkoutsByUserId(string userId)
        {
            // First getting the plan for the PladId
            var plan = db.Plans.Where(x => x.UserID == userId).OrderByDescending(x => x.PlanDate).ToList();
            if (plan != null)
                return db.WorkoutPlans.Where(x => x.PlanID == plan[0].PlanID).ToList();
            return null;
        }
    }
}