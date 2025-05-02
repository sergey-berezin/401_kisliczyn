using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using System.CodeDom.Compiler;
using PopulationNuget;


namespace TestForWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Data WindowData = new Data();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = WindowData;

            Binding Dimension = new Binding(); // Ввод размерности массива
            Dimension.Source = WindowData;
            Dimension.Path = new PropertyPath("dim");
            dimension.SetBinding(TextBox.TextProperty, Dimension);

            Binding MutationChance = new Binding(); // Ввод значения мутации
            MutationChance.Source = WindowData;
            MutationChance.Path = new PropertyPath("mutChance");
            mutationChance.SetBinding(TextBox.TextProperty, MutationChance);            
            //Binding GridGraph = new Binding(); // Ввод значения мутации
            //GridGraph.Source = WindowData.ChartSpline;
            //GridGraph.Path = new PropertyPath("Series");
            ////GridGraph.UpdateSourceTrigger = PropertyChangedCallback;
            //MaxValueGrid.SetBinding(Grid.BindingGroupProperty, GridGraph);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowData.CreatePoints();
            Answer.Items.Clear();
            for(int i = 0; i < WindowData.dim; ++i)
            {
                Answer.Items.Add(WindowData.output[i]);
            }    
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowData.workMode = true;
            MaxValueGrid.Children.Clear();
            MaxValueGrid.Children.Add(WindowData.ChartSpline);
            WindowData.Execute();
            Answer.Items.Clear();
            for (int i = 0; i < WindowData.dim; ++i)
            {
                Answer.Items.Add(WindowData.output[i]);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            WindowData.workMode = false;
            Answer.Items.Clear();
            for (int i = 0; i < WindowData.dim; ++i)
            {
                Answer.Items.Add(WindowData.output[i]);
            }
        }
    }
}