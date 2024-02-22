using System.Collections.ObjectModel;
using TALab2;

namespace Lab2MainUi;

public class ViewModels
{
    private DataProvider _dataProvider;
    public ObservableCollection<Vertex> Vertices { get; private set; }
    public TypeOfAlgorithm TypeOfAlgorithm { get; set; }
    public FunctionType FunctionType { get; set; }
    public Vertex? VertexMain { get; set; } = null;
    public Vertex? VertexSecond { get; set; } = null;

    public IEnumerable<Edge> Edges { get; private set; }

    public ViewModels()
    {
        _dataProvider = new DataProvider();
        Vertices = new ObservableCollection<Vertex>(_dataProvider.Vertixes);
        TypeOfAlgorithm = TypeOfAlgorithm.DjikstraAlgorithm;
        FunctionType = FunctionType.List;
        Edges = _dataProvider.Edges;
    }
}
