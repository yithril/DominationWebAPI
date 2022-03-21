using Domination_WebAPI.Enum;

namespace Domination_WebAPI.Models
{
    public class GameZoneTemplate
    {
        public TemplateType TemplateType { get; set; }
        public double WaterTileProbability { get; set; }
        public double LandTileProbability { get; set; }
        public double WastelandProbability { get; set; }
    }
}
