using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;

using System.Windows.Input;
//using LiveCharts.WinForms;
using PopulationNuget;
using System.Xml.Linq;
using System.Runtime.CompilerServices;


namespace TestForWPF
{
    class Data : IDataErrorInfo
    {
        public Random rand = new Random();
        public bool workMode = false;
        public int dim { get; set; }
        public double mutChance { get; set; }
        public CartesianChart ChartSpline { get; set; }
        public int[] x = new int[1];
        public int[] y = new int[1];
        public double[][] distances = new double[1][];
        public Population population = new Population(2);

        public List<string> output;
        
        public Data()
        {
            dim = 5;
            mutChance = 0.15;
            this.ChartSpline = new CartesianChart();
            output = new List<string>(0);
            population.pity = false;
        }

        public void CreatePoints()
        {
            x = new int[dim];
            y = new int[dim];
            for (int i = 0; i < this.dim; ++i)
            {
                x[i] = rand.Next(1, 1001);
                y[i] = rand.Next(1, 1001);
            }

            distances = new double[dim][];
            for(int i = 0; i < this.dim; ++i)
            {
                this.distances[i] = new double[dim]; 
            }

            for(int i = 0; i < this.dim; ++i)
            {
                for(int j = i + 1; j < this.dim; ++j)
                {
                    this.distances[i][j] = Math.Sqrt(Math.Pow((x[i] - x[j]), 2) + Math.Pow((y[i] - y[j]), 2));
                    this.distances[j][i] = distances[i][j];
                }
                this.distances[i][i] = 0;
            }

            output.Clear();
            for (int i = 0; i < dim; ++i)
            {
                output.Add($"({x[i]}; {y[i]})");
            }
        }

        public void Execute()
        {
            Thread thread = new Thread(WorkFunction);
            thread.IsBackground = true;
            thread.Start();
        }

        public void WorkFunction()
        {
            var SplineValues = new ChartValues<Point>();
            int counter = 0;
            population = new Population(dim);
            while (workMode)
            {
                population.Evolution(distances, mutChance, dim);
                //if (population.minDistance == 0)
                //{
                //    workMode = false;
                //    break;
                //}

                output.Clear();
                for (int i = 0; i < dim; ++i)
                {
                    output.Add($"({x[population.bestSpecimen[i]]}; {y[population.bestSpecimen[i]]}) -> {(distances[population.bestSpecimen[i]][population.bestSpecimen[(i + 1) % dim]]).ToString("F4")}");
                }
                Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (counter % 100 == 0)
                        {
                            ChartSpline.Series.Clear();
                            SplineValues.Add(new Point() { X = counter, Y = population.MemberValue(distances, population.bestSpecimen, dim) });

                            if ((counter > 500) && (counter % 100 == 0)) SplineValues.RemoveAt(0);

                            this.ChartSpline.Series = new SeriesCollection
                            {
                                new LineSeries
                                {
                                    Configuration = new CartesianMapper<Point>()
                                    .X(point => point.X) // Define a function that returns a value that should map to the x-axis
                                    .Y(point => point.Y), // Define a function that returns a value that should map to the y-axis
                                    Title = "Min Values",
                                    Values = SplineValues,
                                    //PointGeometry = null,
                                }
                            };
                        }
                    });
                Task.WaitAny();
                //if (counter == 10000) break;
                counter++;
            }
            // counter;
        }

        string IDataErrorInfo.Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string errorText = "";
                switch (columnName)
                {
                    case "numberOfPoints":
                        {
                            if (dim <= 1)
                                errorText += "Введено неправильное количество городов\n";
                            break;
                        }
                }
                return errorText;
            }
        }

        
    }
}
