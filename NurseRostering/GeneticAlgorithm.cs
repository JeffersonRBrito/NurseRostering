namespace NurseRostering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class GeneticAlgorithm
    {
        /*
         
         - Hard Constraints
         OneShiftADay : A nurse can work only one shift per day.
         MaxHours     : Nurses work a maximum number of hours.
         MaxDaysOn    : Nurses work a maximum number of consecutive days without a break.
         MinDaysOn    : Nurses work a minimum number of consecutive days.
         Succession   : Several shift combinations are not allowed ,e.g. night shift followed by early shift and vice-versa. 
         Hard Request : Nurses must take the annual leave they are entitled to.   
         
         - Soft Constraints
         SoftRequest     : Nurses prefer some favorable shifts (or days-off) for some days once in a while.
         SingleNight     : Nurses prefer to work night shifts in blocks of two or more.
         WeekendBalance  : Nurses should not work more than 3 out of 4 consecutive weekends.
         WeekendSplit    : As for the weekends, the nurses prefer to work both days of the weekend or take both the days of the weekend off.
         Coverage        : The coverage demand for each shift should be satisfied as closely as possible.
         CoverageBalance : The deficit/surplus of coverage demand should be balanced for all shifts in the planning period.
         
        */
        private static Random rng = new Random(DateTime.Now.Millisecond);

        public static string[] earlyShift = { "1", "0", "0" };
        public static string[] lateShift = { "0", "1", "0" };
        public static string[] nightShift = { "0", "0", "1" };
        public static string[] dayOff = { "0", "0", "0" };

        private static Dictionary<Shift, string[]> shiftArrangement = new Dictionary<Shift, string[]>()
        {
            { Shift.EARLY  , earlyShift },
            { Shift.LATE   , lateShift},
            { Shift.NIGHT  , nightShift },
            { Shift.DAY_OFF, dayOff}
        };

        public static string[,] GenerateRandomSchedule(List<Nurse> nurses)
        {
            // A certain amount of nurses distributed throughout a week
            // each day has 3 shifts, so 21 columns + 1 for nurse id
            string[,] solution = new string[nurses.Count, 22];
            
            int i = 0;
            nurses.Shuffle<Nurse>();
            foreach (var nurse in nurses)
            {
                for (int j = 0; j < 22;)
                {
                    if (j == 0)
                    {
                        solution[i, j] = nurse.id;
                        j++;
                    }
                    else
                    {
                        var shift = GetRandomShiftPattern();
                        solution[i, j] = shift[0].ToString();
                        solution[i, j + 1] = shift[1].ToString();
                        solution[i, j + 2] = shift[2].ToString();
                        j += 3;
                    }
                }

                i++;
            }

            return solution;
        }

        private static string[] GetRandomShiftPattern()
        {
            string[] shift;
            switch(rng.Next(0, 4))
            {
                case 0:
                    return shiftArrangement.TryGetValue(Shift.EARLY, out shift) ? shift : null;
                case 1:
                    return shiftArrangement.TryGetValue(Shift.LATE, out shift) ? shift : null;
                case 2:
                    return shiftArrangement.TryGetValue(Shift.NIGHT, out shift) ? shift : null;
                default:
                    return shiftArrangement.TryGetValue(Shift.DAY_OFF, out shift) ? shift : null;
            }
        }
        
        public static Boolean IsSolutionValid(string [,] solution)
        {
            if (solution == null || solution.GetLength(1) != 22 || solution.GetLength(0) == 0)
                return false;

            for (int i = 0; i < solution.GetLength(0); i++)
            {
                int daysInABreak = 0;
                // First column holds the nurse ID so it starts from 1
                for (int j = 1; j < solution.GetLength(1); j += 3)
                {
                    var sum = Int16.Parse(solution[i, j]) + Int16.Parse(solution[i, j + 1]) + Int16.Parse(solution[i, j + 2]);

                    // works more than a shift a day
                    if (sum > 1)
                        return false;

                    // has a break in that day (all shifts has the NOT WORK flag)
                    if (sum == 0)
                        daysInABreak++;
                }

                // has more than a single break a week
                if (daysInABreak != 1)
                    return false;
            }

            for (int j = 1; j < solution.GetLength(1); j++)
            {
                int numberOfNursesWorkingAShift = 0;
                for (int i = 0; i < solution.GetLength(0); i++)
                {
                    numberOfNursesWorkingAShift += Int16.Parse(solution[i, j]);
                }

                if (numberOfNursesWorkingAShift == 0)
                    return false;
            }

            return true;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static Nurse GetNurse(string id, List<Nurse> nurses)
        {
            foreach (var nurse in nurses)
            {
                if (nurse.id.Equals(id)) return nurse;
            }

            return null;
        }
    }

    public enum Shift
    {
        NIGHT,
        LATE,
        EARLY,
        DAY_OFF
    }
}
