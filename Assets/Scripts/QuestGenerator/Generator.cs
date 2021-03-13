using Info;

namespace QuestGenerator
{
    
    public class Generator
    {
        
        // Default constructor
        public Generator() : this(true, true, true) {}

        // Generator constructor
        // Allows to enable/disable different features
        public Generator(
            bool includePlayerImageFeature,
            bool includeReputationFeature,
            bool includeNonLinearityFeature
                )
        {
            // TODO Create a generator and attach chosen features to it
        }

        public Quest GenerateNewQuest(NpcInfo questProvider, PlayerInfo playerImage, ReputationInfo playerReputation)
        {
            // TODO Generate a quest 
            return null;
        }
        
    }
    
}