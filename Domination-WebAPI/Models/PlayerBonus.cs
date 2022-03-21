namespace Domination_WebAPI.Models
{
    public class PlayerBonus : EntityBase
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int BonusId { get; set; }
        public Bonus Bonus { get; set; }
        public DateTime EndDate { get; set; }
    }
}
