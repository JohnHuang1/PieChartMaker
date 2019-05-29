using System.Drawing;
namespace Olympic_graph_generator
{
    public class ItemClass
    {
        public string Name { get; set; }
        public double Data { get; set; }
        public Color Color { get; set; }

        public ItemClass(string name, double data, Color color)
        {
            Name = name;
            Data = data;
            Color = color;
        }

        public override string ToString()
        {
            return string.Format("{0}\t\t{1}\t{2}", Name, Data, Color);
        }

    }
}
