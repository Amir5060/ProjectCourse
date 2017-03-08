using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace ProjectCourse.Models
{
    public partial class C1RMWorkout
    {
        private aspnetEntities db = new aspnetEntities();

        /// <summary>
        /// Description:
        ///     Suggest some workouts to a user to find his 1RMs. 
        /// History:
        ///     Amir Naji   22/02/2017
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>0: There is a unfinished plan. 1: There is no plan. 2: There is a finished plan.</returns>
        public int SuggestWorkoutFor1RM(string userId)
        {
            // 1. Check if user has an on going workout in table.
            // 2. Check if user has 1RM.
            // 3. If he doesn't have any of above then it can suggest.
            var vPlan = db.Plans.Where(x => x.UserID == userId);
            //The user does not have any plan. First time.
            if (vPlan.Count() == 0)
            {
                var vWorkout = db.Workouts.Where(x => x.Difficulty == 1).ToList();
                var v1Rm = db.C1RM.FirstOrDefault(x => x.UserID == userId);
                C1RMWorkout cWorkout = new C1RMWorkout();
                foreach (var v in vWorkout)
                {
                    cWorkout.RMID = v1Rm.RMID;
                    cWorkout.WorkoutID = v.WorkoutID;
                    db.C1RMWorkout.Add(cWorkout);
                    db.SaveChanges();
                }
                return 1;
            }
            else
            {
                /// 1. If there is any finished plan.
                /// 2. If the date is not more than 2 weeks old(optional).
                /// 3. Check the last months 1RMs.
                /// 4. Compare it with statistics, and last month.
                /// 5. Suggest new workouts.
                var vLastPlan = db.Plans.Where(x => x.UserID == userId).OrderByDescending(x => x.PlanDate).ToList()[0];
                // The condition has to change if the number 2 wanted to happen.
                if (vLastPlan.FinishDate != null)
                {
                    var vWorkoutPlan = db.WorkoutPlans.Where(x => x.PlanID == vLastPlan.PlanID);
                    var v1RMWorkout = db.C1RMWorkout.Where(x => x.RMPlanId == vLastPlan.PlanID);
                    return 2;
                }                
            }
            return 0;
        }

        

        

        /// <summary>
        /// It is better to have multiple queries than manipulating a list. ++
        /// It seems Lambda a little faster than LINQ. +
        /// Better to to SELECT some columns than just having all of columns. It's faster. +++
        /// </summary>
        public void TestTest()
        {
            var vWorkoutPlans = db.WorkoutPlans.Where(x => x.PlanID == 1);
            var vUser = db.EWPUsers.FirstOrDefault(x => x.UserID == "098e28c3-81c3-4868-8573-4a29ba50f150");
            //var vRelative = db.RelativeStrengthTs.Where(x => x.Sex == vUser.Gender).OrderByDescending(x => x.RelativeStrengthPoint);
            List<FirstCompareClass> compareProp = new List<FirstCompareClass>();
            //FirstCompareClass compareCl;
            for (var i = 0; i < 5000; i++)
            {
                foreach (var v in vWorkoutPlans)
                {
                    //var vTemp = from d in db.RelativeStrengthTs
                    //            where d.Sex == vUser.Gender && d.WorkoutID == v.WorkoutID
                    //            orderby d.RelativeStrengthPoint descending
                    //            select d;
                    var vTemp = db.RelativeStrengthTs.Where(x => x.Sex == vUser.Gender && x.WorkoutID == v.WorkoutID).OrderByDescending(x => x.RelativeStrengthPoint);
                    //var vTemp = vRelative.Where(x => x.WorkoutID == v.WorkoutID);
                    //if (vTemp != null)
                    //{
                    //    foreach (var vv in vTemp)
                    //    {
                    //        if (vv.RelativeStrengthValue > v.WorkoutPlanWeight)
                    //        {
                    //            compareCl = new FirstCompareClass();
                    //            compareCl.Weight = v.WorkoutPlanWeight;
                    //            compareCl.WorkoutId = v.WorkoutID;
                    //            compareCl.WorkoutPoint = vv.RelativeStrengthPoint;
                    //            compareProp.Add(compareCl);
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
        }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime Due { get; set; }
    }

    public class CompareClass
    {
        public Single Plan1Weight { get; set; }
        public Single Plan2Weight { get; set; }
        public Single WeightDifferent { get; set; }
        public int WorkoutId { get; set; }
    }

    public class FirstCompareClass
    {
        public Single Weight { get; set; }
        public int WorkoutPoint { get; set; }
        public int WorkoutId { get; set; }

    }

    public class JsonCycle
    {
        public Dictionary<String, CyclesFlowData> CyclesFlow { get; set; }
        public List<CycleStepsData> CycleSteps { get; set; }
    }

    public class CyclesFlowData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Sets { get; set; }
        public int Weight { get; set; }
        public string Rest { get; set; }
        public int Repetition { get; set; }
        public int Week { get; set; }
        public string Cycle { get; set; }
    }

    public class CycleStepsData
    {
        public string CycleName { get; set; }
        public string NextCycle { get; set; }
    }
}