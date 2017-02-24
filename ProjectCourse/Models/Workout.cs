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
    
    public partial class Workout
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Workout()
        {
            this.WorkoutMuscles = new HashSet<WorkoutMuscle>();
            this.RelativeStrengthTs = new HashSet<RelativeStrengthT>();
            this.C1RMWorkout = new HashSet<C1RMWorkout>();
            this.WorkoutPlans = new HashSet<WorkoutPlan>();
        }
    
        public int WorkoutID { get; set; }
        public string Name { get; set; }
        public int Difficulty { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkoutMuscle> WorkoutMuscles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RelativeStrengthT> RelativeStrengthTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C1RMWorkout> C1RMWorkout { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
