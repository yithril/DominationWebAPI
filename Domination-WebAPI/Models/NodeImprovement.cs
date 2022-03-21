using Domination_WebAPI.Enum;

namespace Domination_WebAPI.Models
{
    public class NodeImprovement : EntityBase
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImgURL { get; set; }
        public List<NodeImprovementCost> Costs { get; set; } 
        public ResourceTypeEnum ResourceType { get; set; }
        public int StaticBonus { get; set; }
        public double Modifier { get; set; } = 1;
    }
}
