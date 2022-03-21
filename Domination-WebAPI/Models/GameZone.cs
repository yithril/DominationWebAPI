namespace Domination_WebAPI.Models
{
    public class GameZone : EntityBase
    {
        public List<GameNode> Nodes { get; set; }
        public int xCoord { get; set; }
        public int yCoord { get; set; }
    }
}
