﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    public partial class aspnetEntities : DbContext
    {
        public aspnetEntities()
            : base("name=aspnetEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C1RM> C1RM { get; set; }
        public virtual DbSet<Bone> Bones { get; set; }
        public virtual DbSet<EWPUser> EWPUsers { get; set; }
        public virtual DbSet<Injury> Injuries { get; set; }
        public virtual DbSet<Joint> Joints { get; set; }
        public virtual DbSet<Muscle> Muscles { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<RelativeStrengthT> RelativeStrengthTs { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Workout> Workouts { get; set; }
        public virtual DbSet<WorkoutMuscle> WorkoutMuscles { get; set; }
        public virtual DbSet<C1RMWorkout> C1RMWorkout { get; set; }
        public virtual DbSet<WorkoutPlan> WorkoutPlans { get; set; }
    
        public virtual ObjectResult<GetAllSports_Result> GetAllSports()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllSports_Result>("GetAllSports");
        }

        

        public virtual ObjectResult<GetUserByUserID_Result> GetUserByUserID(string userID)
        {
            var userIDParameter = userID != null ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetUserByUserID_Result>("GetUserByUserID", userIDParameter);
        }
    }
}
