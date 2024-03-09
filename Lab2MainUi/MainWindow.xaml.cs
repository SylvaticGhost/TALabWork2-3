using System.Windows;
using System.Windows.Controls;
using TALab2;

namespace Lab2MainUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModels _dataContext;

        public MainWindow()
        {
            InitializeComponent();
            _dataContext = new ViewModels();
            this.DataContext = _dataContext;
        }


        private void ChangeAlgorithm(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if (radioButton.Name != Convert.ToString(_dataContext.TypeOfAlgorithm))
                _dataContext.TypeOfAlgorithm = _dataContext.TypeOfAlgorithm.GetOpposite();
        }


        private void ChangeFunctions(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if (radioButton.Name != Convert.ToString(_dataContext.FunctionType))
                _dataContext.FunctionType = _dataContext.FunctionType.GetOpposite();
        }


        private void BtnCalculate_OnClick(object sender, RoutedEventArgs e)
        {
            if (_dataContext.VertexMain is null)
            {
                ShowException("You haven't chosen a main vertex");
                return;
            }

            if (_dataContext.VertexSecond is null && _dataContext.FunctionType is FunctionType.DistanceToPoint)
            {
                ShowException("For calculating distance between points you have to define the second point");
                return;
            }

            StartCalculating();

        }


        private void ShowException(string text)
        {
            MessageBox.Show($"Error: {text}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }


        private void ShowInfo(string text)
        {
            MessageBox.Show(text,
                "info",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        private void StartCalculating()
        {
            Graph graph = new Graph(_dataContext.Vertices, _dataContext.Edges);

            if (_dataContext.FunctionType == FunctionType.List)
            {
                IEnumerable<Destination> points =
                    graph.GetListOfShortest(_dataContext.VertexMain!, _dataContext.TypeOfAlgorithm);

                string list = Functions.CollectionToString(points);

                ShowInfo($"Used algorithm {_dataContext.TypeOfAlgorithm}\n List: \n" + list);
                return;
            }

            if (_dataContext.FunctionType == FunctionType.DistanceToPoint)
            {
                double distance;

                if (_dataContext.TypeOfAlgorithm == TypeOfAlgorithm.DjikstraAlgorithm)
                    distance = graph.DijkstraAlgorithm(_dataContext.VertexMain!, _dataContext.VertexSecond!);
                else
                    distance = graph.FloydWarshallAlgorithm(_dataContext.VertexMain!, _dataContext.VertexSecond!);

                ShowInfo($"Used algorithm {_dataContext.TypeOfAlgorithm}\n From {_dataContext.VertexMain!.Sign} to {_dataContext.VertexSecond!.Sign}\n distance = {Math.Round(distance, 3)} km");
            }
        }
    }
}