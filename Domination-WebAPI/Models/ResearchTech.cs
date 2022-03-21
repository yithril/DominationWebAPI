namespace Domination_WebAPI.Models
{
    public class ResearchTech : EntityBase
    {
        public int ResearchCost { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
    }
}
