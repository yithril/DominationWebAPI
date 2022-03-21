namespace Domination_WebAPI.Models
{
    public class User : EntityBase
    {
        public string AccountId { get; set; }
        public int Food { get; set; }
        public int Culture { get; set; }
        public int Population { get; set; }
        public int Research { get; set; }
        public int IndustrialGoods { get; set; }
        public int Money { get; set; }
        public int MilitaryMight { get; set; }
        public int AirForce { get; set; }
        public int Army { get; set; }
        public int Navy { get; set; }
        public int AcquisitionPoints { get; set; }
        public string NationName { get; set; }
        public string Adjective { get; set; }
        public List<PlayerBonus> Bonus { get; set; } = new List<PlayerBonus>();
        public DateTime LastAcquisitionAcquired { get; set; }
        public double Happiness { get; set; } = 1;
    }
}
