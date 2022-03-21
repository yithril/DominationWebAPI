namespace Domination_WebAPI.Models
{
    public class PlayerResearchTech : EntityBase
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ResearchTechId { get; set; }
        public ResearchTech ResearchTech { get; set; }
    }
}
