using Domination_WebAPI.Enum;

namespace Domination_WebAPI.Models
{
    public class NodeImprovementCost : EntityBase
    {
        public int NodeImprovementId { get; set; }
        public NodeImprovement NodeImprovement { get; set; }
        public int Cost { get; set; }
        public ResourceTypeEnum Type { get; set; }
    }
}
