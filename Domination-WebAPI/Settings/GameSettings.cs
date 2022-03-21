namespace Domination_WebAPI.Settings
{
    public class GameSettings
    {
        public int AcquisitionPointCost { get; set; }
        public int CultureAcquisitionCost { get; set; }
        public int AcquisitionGain { get; set; }

        //How much food do our pops need per unit?
        public int PopFoodRequiredModifier { get; set; }

        //If you feed everybody how many new pops do you get?
        public int PopFullModifier { get; set; }

        //If you fail to feed folks how much do they go down?
        public double MaxPopDeclineModifier { get; set; }

        //How affected by culture is each node
        public double CultureNodeModifier { get; set; }

        //Base cost to make a claim to a zone
        public int BaseClaimCost { get; set; }

        //How much extra do you pay if someone already has a claim on a zone
        public int OpponentClaimModifier { get; set; }
    }
}
