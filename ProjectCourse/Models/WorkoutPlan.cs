//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectCourse.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkoutPlan
    {
        public int WorkoutPlanID { get; set; }
        public int WorkoutID { get; set; }
        public int PlanID { get; set; }
        public int Repetition { get; set; }
        public int WorkoutPlanSet { get; set; }
        public string Rest { get; set; }
        public float WorkoutPlanWeight { get; set; }
        public int WorkoutWeek { get; set; }
    
        public virtual Plan Plan { get; set; }
        public virtual Workout Workout { get; set; }
    }
}
