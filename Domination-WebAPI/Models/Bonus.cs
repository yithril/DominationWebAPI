using Domination_WebAPI.Enum;

namespace Domination_WebAPI.Models
{
    public class Bonus : EntityBase
    {
        public double Modifier { get; set; }
        public ResourceTypeEnum Resource { get; set; }
        public int Duration { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ResearchTechId { get; set; }
        public ResearchTech ResearchTech { get; set; }
    }
}
