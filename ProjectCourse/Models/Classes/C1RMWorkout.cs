using System;
using System.Collections.Generic;
using System.Linq;
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

        public void WorkoutsForPlan(string userId)
        {
            var vPlan = db.Plans.Where(x => x.UserID == userId).ToList();
            if (vPlan.Count() > 0)
            {
                var vUnfinishedPlan = db.Plans.FirstOrDefault(x => x.UserID == userId && x.FinishDate == null);
                // If there is anything to compare.
                // There can't be any unfinished plan.
                var vLastPlans = GetLastTwoPlans(userId);
                if (vLastPlans.Count() > 1)
                {
                    var vCompareWorkout = CompareWorkouts(vLastPlans);
                }
                else
                {
                    var vCompareWorkout = CompareWorkouts(userId);
                    WorkoutPlan newWorkout = new WorkoutPlan();
                    foreach (var item in vCompareWorkout)
                    {
                        for (var i = 0; i < 6; i++)
                        {
                            newWorkout.PlanID = vUnfinishedPlan.PlanID;
                            newWorkout.Repetition = 0;
                            newWorkout.WorkoutPlanSet = 0;
                            newWorkout.WorkoutPlanWeight = 0;
                            newWorkout.Rest = 0;
                            newWorkout.WorkoutID = item.WorkoutId;
                        }
                    }
                    
                }

            }
        }        


        /// <summary>
        /// Description:
        ///     Return the list of last two plans.
        /// History:
        ///     Amir Naji   23/02/2017
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<Plan> GetLastTwoPlans(string userId)
        {           
            return (from p in db.Plans
                    where p.UserID == userId && p.FinishDate != null
                    orderby p.FinishDate descending
                    select p).Take(2).ToList();
        }

        /// <summary>
        /// Description:
        ///     Compare the either 1RM, or the last finished plan with the Statistics.
        /// History:
        ///     Amir Naji   23/02/2017
        ///     Amir Naji   24/02/2017
        /// </summary>
        /// <param name="userId"></param>
        private List<FirstCompareClass> CompareWorkouts(string userId)
        {
            var vLastPlans = GetLastTwoPlans(userId);
            // When there are at least two plans, so they can be compared with each other.
            //if (vLastPlans.Count == 2)
            //{
            //    //var vWorkoutPlans1 = db.WorkoutPlans.Where(x => x.PlanID == vLastPlans[0].PlanID);
            //    //var vWorkoutPlans2 = db.WorkoutPlans.Where(x => x.PlanID == vLastPlans[1].PlanID);
            //    //List<CompareClass> comparePlans = new List<CompareClass>();
            //    //CompareClass comparePlan = null;
            //    //foreach(var v in vWorkoutPlans1)
            //    //{                    
            //    //    var v2 = vWorkoutPlans2.FirstOrDefault(x => x.WorkoutID == v.WorkoutID);
            //    //    if (v2 != null)
            //    //    {
            //    //        comparePlan = new CompareClass();                        
            //    //        comparePlan.Plan1Weight = v.WorkoutPlanWeight;
            //    //        comparePlan.Plan2Weight = v2.WorkoutPlanWeight;
            //    //        comparePlan.WorkoutId = v.WorkoutID;
            //    //        comparePlan.WeightDifferent = comparePlan.Plan1Weight - comparePlan.Plan2Weight;
            //    //        comparePlans.Add(comparePlan);
            //    //    }
            //    //}
            //    //return comparePlan;
            //    // Comparision Finishes

            //}
            //else // We have to compare the last one with the statistics.
            //{
                // Plan is finished.
            if (db.Plans.Where(x => x.UserID == userId && x.FinishDate == null).Count() == 0)
            {
                var vWorkoutPlans = db.WorkoutPlans.Where(x => x.PlanID == vLastPlans[0].PlanID);
                var vUser = db.EWPUsers.FirstOrDefault(x => x.UserID == userId);
                var vRelative = db.RelativeStrengthTs.Where(x => x.Sex == vUser.Gender).OrderByDescending(x => x.RelativeStrengthPoint);
                List<FirstCompareClass> compareProp = new List<FirstCompareClass>();
                FirstCompareClass compareCl = null;
                foreach (var v in vWorkoutPlans)
                {
                    var vTemp = vRelative.Where(x => x.WorkoutID == v.WorkoutID);
                    if (vTemp != null)
                    {
                        foreach (var vv in vTemp)
                        {
                            if (vv.RelativeStrengthValue > v.WorkoutPlanWeight)
                            {
                                compareCl = new FirstCompareClass();
                                compareCl.Weight = v.WorkoutPlanWeight;
                                compareCl.WorkoutId = v.WorkoutID;
                                compareCl.WorkoutPoint = vv.RelativeStrengthPoint;
                                compareProp.Add(compareCl);
                                break;
                            }
                        }
                    }
                }
                return compareProp;
            }
            else // Just have an 1RM, no workouts.
            {
                var vWorkoutPlans = db.C1RMWorkout.Where(x => x.RMPlanId == vLastPlans[0].PlanID);
                var vUser = db.EWPUsers.FirstOrDefault(x => x.UserID == userId);
                var vRelative = db.RelativeStrengthTs.Where(x => x.Sex == vUser.Gender).OrderByDescending(x => x.RelativeStrengthPoint);
                List<FirstCompareClass> compareProp = new List<FirstCompareClass>();
                FirstCompareClass compareCl = null;
                foreach (var v in vWorkoutPlans)
                {
                    var vTemp = vRelative.Where(x => x.WorkoutID == v.WorkoutID);
                    if (vTemp != null)
                    {
                        foreach (var vv in vTemp)
                        {
                            if (vv.RelativeStrengthValue > v.WorkoutWeight)
                            {
                                compareCl = new FirstCompareClass();
                                compareCl.Weight = (float)v.WorkoutWeight;
                                compareCl.WorkoutId = v.WorkoutID;
                                compareCl.WorkoutPoint = vv.RelativeStrengthPoint;
                                compareProp.Add(compareCl);
                                break;
                            }
                        }
                    }
                }
                return compareProp;
            }
                // Comparision Finishes
            //}
        }

        private List<CompareClass> CompareWorkouts(List<Plan> vLastPlans)
        {
            var vWorkoutPlans1 = db.WorkoutPlans.Where(x => x.PlanID == vLastPlans[0].PlanID);
            var vWorkoutPlans2 = db.WorkoutPlans.Where(x => x.PlanID == vLastPlans[1].PlanID);
            List<CompareClass> comparePlans = new List<CompareClass>();
            CompareClass comparePlan = null;
            foreach (var v in vWorkoutPlans1)
            {
                var v2 = vWorkoutPlans2.FirstOrDefault(x => x.WorkoutID == v.WorkoutID);
                if (v2 != null)
                {
                    comparePlan = new CompareClass();
                    comparePlan.Plan1Weight = v.WorkoutPlanWeight;
                    comparePlan.Plan2Weight = v2.WorkoutPlanWeight;
                    comparePlan.WorkoutId = v.WorkoutID;
                    comparePlan.WeightDifferent = comparePlan.Plan1Weight - comparePlan.Plan2Weight;
                    comparePlans.Add(comparePlan);
                }
            }
            return comparePlans;
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
}