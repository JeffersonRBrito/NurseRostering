namespace NurseRostering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Program
    {
        const int INITIAL_SOLUTION_AMOUNT = 100;

        static void Main(string[] args)
        {
            NurseLoader nl = new NurseLoader("c:/users/jeff/documents/visual studio 2015/Projects/NurseRostering/NurseRostering/Nurses.xml");
            var nurseList = nl.GetNurseList();
            List<string[,]> solutionPool = new List<string[,]>();

            for (int i = 0; i < INITIAL_SOLUTION_AMOUNT; i++)
            {
                string[,] solution = null;
                do
                {
                    solution = GeneticAlgorithm.GenerateRandomSchedule(nurseList);
                } while (!GeneticAlgorithm.IsSolutionValid(solution));

                solutionPool.Add(solution);
                Console.WriteLine("number of valid solutions so far: " + solutionPool.Count);
            }
            
            foreach(var solution in solutionPool)
            {
                printSolution(solution);
                Console.WriteLine();
                Console.WriteLine();
            }
            
            Console.ReadLine();

        }

        static void printSolution(string[,] solution)
        {
            
            Console.Write(
                  "=============================Nurse schedule===========================\n"
                + "           Mon  |   Tue  |   Wed  |   Thr  |   Fri  |   Sat  |   Sun  \n"
                + "Nurse  |E |L |N |E |L |N |E |L |N |E |L |N |E |L |N |E |L |N |E |L |N |\n"
                );
            

            for(int i = 0; i < solution.GetLength(0); i++)
            {
                for(int j = 0; j < solution.GetLength(1); j++)
                {
                    // TODO: Write a tsv file
                    Console.Write(solution[i, j]);

                    if (j == 0) Console.Write("\t");
                    else Console.Write("  ");
                }

                Console.Write("\n");
            }
        }

    }

    public class Nurse
    {
        public Nurse(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        // TODO: Add a data modelling for nurse preferences
        public string id;
        public string name;
    }
}
