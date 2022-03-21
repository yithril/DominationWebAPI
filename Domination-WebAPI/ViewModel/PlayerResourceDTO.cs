using Domination_WebAPI.Models;

namespace Domination_WebAPI.ViewModel
{
    public class PlayerResourceDTO
    {
        public int Food { get; set; }
        public int Culture { get; set; }
        public int Population { get; set; }
        public int Research { get; set; }
        public int IndustrialGoods { get; set; }
        public int Money { get; set; }
        public int MilitaryMight { get; set; }
        public int AcquisitionPoints { get; set; }
        public string NationName { get; set; }
        public string Adjective { get; set; }
        public List<Bonus> Bonuses { get; set; }
    }
}
