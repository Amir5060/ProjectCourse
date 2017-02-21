using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectCourse.Models
{
    public class WorkoutViewModel
    {
        public List<Workout> workout { get; set; }
        public List<WorkoutMuscle> workoutMuscle { get; set; }

        public List<Muscle> muscle { get; set; }
    }
}