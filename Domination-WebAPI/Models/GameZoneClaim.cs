namespace Domination_WebAPI.Models
{
    public class GameZoneClaim : EntityBase
    {
        public int GameZoneId { get; set; }
        public GameZone GameZone { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
