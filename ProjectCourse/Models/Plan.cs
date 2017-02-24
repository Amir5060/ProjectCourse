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
    
    public partial class Plan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Plan()
        {
            this.C1RMWorkout = new HashSet<C1RMWorkout>();
            this.WorkoutPlans = new HashSet<WorkoutPlan>();
        }
    
        public int PlanID { get; set; }
        public string UserID { get; set; }
        public string Microcycle { get; set; }
        public Nullable<int> WorkoutTime { get; set; }
        public Nullable<System.DateTime> PlanDate { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
    
        public virtual EWPUser EWPUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C1RMWorkout> C1RMWorkout { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
