using Domination_WebAPI.Enum;

namespace Domination_WebAPI.Models
{
    public class GameNode : EntityBase
    {
        public int xCoord { get; set; }
        public int yCoord { get; set; }     
        public DateTime LastProducedDate { get; set; }
        public ResourceTypeEnum CurrentResourceType { get; set; } = ResourceTypeEnum.None;
        public int UserId { get; set; }
        public int GameZoneId { get; set; }
        public GameZone GameZone { get; set; }
        public bool IsDevelopable { get; set; }
        public double Culture { get; set; } = .5;
        public NodeQuality NodeQuality { get; set; } = NodeQuality.Medium;
        public int? NodeImprovementId { get; set; }
        public NodeImprovement? NodeImprovement { get; set; }
        public int CheckNodeCost()
        {
            switch (this.NodeQuality)
            {
                case NodeQuality.Very_Low:
                    return 10;
                case NodeQuality.Low:
                    return 20;
                case NodeQuality.Medium:
                    return 30;
                case NodeQuality.High:
                    return 40;
                case NodeQuality.Quality:
                    return 50;
                default:
                    return 100;
            }
        }
    }
}
