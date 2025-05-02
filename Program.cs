using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AppLogger;

namespace TestForGeneticAlgorythm
{
    internal class Program
    {
        private static bool keepRunning = true;

        static void Main(string[] args)
        {
            int dim = 5;
            double MutationChance = 0.05;
            int[][] Distances1 = new int[dim][];
            int[][] Distances2 = new int[dim][];
            int[][] DistancesTest = new int[dim][];
            for (int i = 0; i < dim; ++i)
            {
                Distances1[i] = new int[dim];
                Distances2[i] = new int[dim];
                DistancesTest[i] = new int[dim];
            }
            {
                Distances1[0][0] = 0;
                Distances1[0][1] = 3;
                Distances1[0][2] = 4;
                Distances1[0][3] = 1;
                Distances1[0][4] = 8;
                Distances1[1][0] = 3;
                Distances1[1][1] = 0;
                Distances1[1][2] = 5;
                Distances1[1][3] = 7;
                Distances1[1][4] = 3;
                Distances1[2][0] = 4;
                Distances1[2][1] = 5;
                Distances1[2][2] = 0;
                Distances1[2][3] = 12;
                Distances1[2][4] = 2;
                Distances1[3][0] = 1;
                Distances1[3][1] = 7;
                Distances1[3][2] = 12;
                Distances1[3][3] = 0;
                Distances1[3][4] = 6;
                Distances1[4][0] = 8;
                Distances1[4][1] = 3;
                Distances1[4][2] = 2;
                Distances1[4][3] = 6;
                Distances1[4][4] = 0;
            }
            // 0   3   4   1   8
            // 3   0   5   7   3
            // 4   5   0  12   2
            // 1   7  12   0   6
            // 8   3   2   6   0
            {
                Distances2[0][0] = 0;
                Distances2[0][1] = 3;
                Distances2[0][2] = 4;
                Distances2[0][3] = 1;
                Distances2[0][4] = 8;
                Distances2[1][0] = 5;
                Distances2[1][1] = 0;
                Distances2[1][2] = 6;
                Distances2[1][3] = 3;
                Distances2[1][4] = 9;
                Distances2[2][0] = 9;
                Distances2[2][1] = 2;
                Distances2[2][2] = 0;
                Distances2[2][3] = 12;
                Distances2[2][4] = 6;
                Distances2[3][0] = 8;
                Distances2[3][1] = 7;
                Distances2[3][2] = 8;
                Distances2[3][3] = 0;
                Distances2[3][4] = 6;
                Distances2[4][0] = 5;
                Distances2[4][1] = 2;
                Distances2[4][2] = 2;
                Distances2[4][3] = 16;
                Distances2[4][4] = 0;
            }
            // 0   3   4   1   8
            // 5   0   6   3   9
            // 9   2   0  12   6
            // 8   7   8   0   6
            // 5   2   2  16   0
            {
                DistancesTest[0][0] = 0;
                DistancesTest[0][1] = 1;
                DistancesTest[0][2] = 100;
                DistancesTest[0][3] = 100;
                DistancesTest[0][4] = 100;
                DistancesTest[1][0] = 100;
                DistancesTest[1][1] = 0;
                DistancesTest[1][2] = 1;
                DistancesTest[1][3] = 100;
                DistancesTest[1][4] = 100;
                DistancesTest[2][0] = 100;
                DistancesTest[2][1] = 100;
                DistancesTest[2][2] = 0;
                DistancesTest[2][3] = 100;
                DistancesTest[2][4] = 1;
                DistancesTest[3][0] = 1;
                DistancesTest[3][1] = 100;
                DistancesTest[3][2] = 100;
                DistancesTest[3][3] = 0;
                DistancesTest[3][4] = 100;
                DistancesTest[4][0] = 100;
                DistancesTest[4][1] = 100;
                DistancesTest[4][2] = 100;
                DistancesTest[4][3] = 1;
                DistancesTest[4][4] = 0;
            }
            // 0    1   100 100 100
            // 100  0   1   100 100
            // 100  100 0   100 1
            // 1    100 100 0   100
            // 100  100 100 1   0


            Console.WriteLine();
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; ++j)
                    Console.Write($"{Distances1[i][j]} ");
                Console.WriteLine();
            }

            Population family = new Population(dim);
            family.pity = true;
            printAll(family.population);
            //for (int i = 0; i < 10; ++i)
            //{
            //    Console.Write($"{family.MemberValue(Distances, family.population[i], dim)} "); 
            //}
            //Console.WriteLine($"Population number {0}");
            //family.Evolution(Distances, MutationChance, dim);
            //Console.WriteLine();
            //printAll(family.population);
            //Console.WriteLine($"{family.minDistance}");
            //Console.ReadLine();


            int counter = 0;
            Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                keepRunning = false;
            };
            while (keepRunning)
            {
                counter++;
                Console.WriteLine($"Population number {counter}");
                family.Evolution(Distances1, MutationChance, dim);
                printAll(family.population);
                for (int i = 0; i < 10; ++i)
                {
                    Console.Write($"{family.MemberValue(Distances1, family.population[i], dim)} ");
                }
                Console.WriteLine();
                //if (counter % 5 == 0) Console.ReadLine();

            }

            Console.WriteLine($"Best score: {family.minDistance}");
            Console.Write($"Best solution: (");
            for (int i = 0; i < dim - 1; ++i)
            {
                Console.Write($"{family.bestSpecimen[i] + 1}, ");
            }
            Console.WriteLine($"{family.bestSpecimen[dim - 1] + 1})");
        }

        public static void printOne(int[] Member)
        {
            Console.Write('(');
            for (int i = 0; i < Member.Length - 1; ++i)
            {
                Console.Write($"{Member[i] + 1}, ");
            }
            Console.WriteLine($"{Member[Member.Length - 1] + 1})");
        }

        public static void printAll(int[][] Population)
        {
            for (int i = 0; i < 10; ++i)
            {
                printOne(Population[i]);
            }
            Console.WriteLine();
        }

    }
}
