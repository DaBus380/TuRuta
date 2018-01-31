namespace TuRuta.Common.ViewModels
{
    public class StopVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public (double, double) Location { get; set; }
    }
}