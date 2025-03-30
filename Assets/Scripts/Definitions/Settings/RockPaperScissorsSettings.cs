using System;
using System.Collections.Generic;
using UI;

namespace Definitions
{
    [Serializable]
    public class RockPaperScissorsSettings : BaseDef
    {
        public float ShakingHandsTime { get; set; } = 3f;
        public float ShowRoundResultsTime { get; set; } = 1.5f;

        public Dictionary<RockPaperScissorsHand, string> HandSprites { get; set; } = new Dictionary<RockPaperScissorsHand, string>()
        {
            {RockPaperScissorsHand.Rock, "hands[rock]"},
            {RockPaperScissorsHand.Paper, "hands[paper]"},
            {RockPaperScissorsHand.Scissors, "hands[scissors]"},
        };
        public Dictionary<int, string> PlayerOrderLocalizationKeys { get; set; } = new Dictionary<int, string>()
        {
            {0, "ROCK_PAPER_SCISSORS_FIRST_PLAYER"},
            {1, "ROCK_PAPER_SCISSORS_SECOND_PLAYER"},
            {2, "ROCK_PAPER_SCISSORS_THIRD_PLAYER"},
        };
        public string SelectHandTitleLocalizationKey { get; set; } = "ROCK_PAPER_SCISSORS_SELECT_HAND";
        public string NeedNextRoundLocalizationKey { get; set; } = "ROCK_PAPER_SCISSORS_NEED_NEXT_ROUND";
    }
}