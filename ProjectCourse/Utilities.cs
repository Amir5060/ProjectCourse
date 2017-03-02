using ProjectCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectCourse
{
    public static class Utilities
    {
        private static aspnetEntities db = new aspnetEntities();

        /// <summary>
        /// Description:
        ///     For calculating 1RM  by Brzycki formula
        /// History:
        ///     Amir Naji   02/12/2016
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="repetition"></param>
        /// <returns></returns>
        public static float OneRMCalculator(float weight, int repetition)
        {
            return Convert.ToSingle(weight / (1.0278 - (0.0278 * repetition)));
        }

        /// <summary>
        /// Description:
        ///     To calculate the relative score to compare it with table
        /// History:
        ///     Amir Naji   02/12/2016
        /// </summary>
        /// <param name="bodyWeight"></param>
        /// <param name="liftWeight"></param>
        /// <returns></returns>
        public static float RelativeCalculator(float bodyWeight, float liftWeight)
        {
            return bodyWeight / liftWeight;
        }

        public static void SuggestWorkout(string userID)
        {
            /// Is it first
            WorkoutPlan wp = new WorkoutPlan();
            var v = wp.GetLastWorkoutsByUserId(userID);
            if (v.Count() == 0)
            {
                // Some 1RM suggestions
            }

        }        
    }
}