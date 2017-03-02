using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace ProjectCourse.Models
{
    public partial class WorkoutPlan
    {
        private aspnetEntities db = new aspnetEntities();
        public int WeeksCount { get; set; }

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

        public void WorkoutsForPlan(string userId)
        {
            C1RMWorkout rm = new C1RMWorkout();
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
                    //JsonConvert.DeserializeObject()
                    using (StreamReader r = new StreamReader(HttpContext.Current.Server.MapPath("/CycleFlow.json")))
                    {
                        string json = r.ReadToEnd();
                        var items = JsonConvert.DeserializeObject<JsonCycle>(json);

                        //DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(JsonCycle));
                        //MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(HttpContext.Current.Server.MapPath("/CycleFlow.json"))));
                        //List<JsonCycle> contacts = (List<JsonCycle>)js.ReadObject(stream);

                        var vCompareWorkout = CompareWorkouts(userId);
                        WorkoutPlan newWorkout = new WorkoutPlan();
                        WeeksCount = items.CycleSteps.Count();
                        foreach (var item in items.CycleSteps)
                        {
                            foreach (var workout in vCompareWorkout)
                            {
                                newWorkout.PlanID = vUnfinishedPlan.PlanID;
                                newWorkout.Repetition = items.CyclesFlow[item.CycleName].Repetition;
                                newWorkout.WorkoutPlanSet = items.CyclesFlow[item.CycleName].Sets;
                                newWorkout.WorkoutPlanWeight = (items.CyclesFlow[item.CycleName].Weight * workout.Weight) / 100;
                                newWorkout.Rest = items.CyclesFlow[item.CycleName].Rest;
                                newWorkout.WorkoutWeek = items.CyclesFlow[item.CycleName].Week;
                                newWorkout.WorkoutID = workout.WorkoutId;
                                db.WorkoutPlans.Add(newWorkout);
                                db.SaveChanges();
                            }
                            //db.WorkoutPlans.Add(newWorkout);
                            //db.SaveChanges();
                        }
                        //foreach (var item in vCompareWorkout)
                        //{
                        //    for (var i = 0; i < 6; i++)
                        //    {
                        //        newWorkout.PlanID = vUnfinishedPlan.PlanID;
                        //        newWorkout.Repetition = 0;
                        //        newWorkout.WorkoutPlanSet = 0;
                        //        newWorkout.WorkoutPlanWeight = 0;
                        //        newWorkout.Rest = 0;
                        //        newWorkout.WorkoutID = item.WorkoutId;
                        //    }
                        //}
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
        public List<Plan> GetLastTwoPlans(string userId)
        {
            return (from p in db.Plans
                    where p.UserID == userId 
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
        public List<FirstCompareClass> CompareWorkouts(string userId)
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
                var planId = vLastPlans[0].PlanID;
                var vWorkoutPlans = db.C1RMWorkout.Where(x => x.RMPlanId == planId).ToList();
                var temp = vWorkoutPlans[0].RMID;
                var vRM = db.C1RM.FirstOrDefault(x => x.RMID == temp);
                var vUser = db.EWPUsers.FirstOrDefault(x => x.UserID == userId);
                var vRelative = db.RelativeStrengthTs.Where(x => x.Sex == vUser.Gender).OrderByDescending(x => x.RelativeStrengthPoint);
                List<FirstCompareClass> compareProp = new List<FirstCompareClass>();
                FirstCompareClass compareCl = null;
                foreach (var v in vWorkoutPlans)
                {
                    var vTemp = vRelative.Where(x => x.WorkoutID == v.WorkoutID).ToList();
                    if (vTemp != null)
                    {
                        var lastItem = vTemp.LastOrDefault();
                        foreach (var vv in vTemp)
                        {
                            // If a person's 1RM is bigger than statistics, the last iteration should go in.
                            if ((vv.RelativeStrengthValue > (v.WorkoutWeight / vRM.UserWeight)) || vv.Equals(lastItem))
                            {
                                compareCl = new FirstCompareClass();
                                compareCl.Weight = (float)v.WorkoutWeight;
                                compareCl.WorkoutId = v.WorkoutID;
                                compareCl.WorkoutPoint = vv.RelativeStrengthPoint;
                                compareProp.Add(compareCl);
                                break;
                            }
                            //if (vv.Equals(lastItem))
                            //{
                            //    compareCl = new FirstCompareClass();
                            //    compareCl.Weight = (float)v.WorkoutWeight;
                            //    compareCl.WorkoutId = v.WorkoutID;
                            //    compareCl.WorkoutPoint = vv.RelativeStrengthPoint;
                            //    compareProp.Add(compareCl);
                            //    break;
                            //}

                        }
                    }
                }
                return compareProp;
            }
            // Comparision Finishes
            //}
        }

        public List<CompareClass> CompareWorkouts(List<Plan> vLastPlans)
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
    }
}