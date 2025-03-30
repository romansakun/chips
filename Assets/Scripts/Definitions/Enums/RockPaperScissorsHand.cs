using System;

namespace Definitions
{
    public enum RockPaperScissorsHand
    {
        Rock,
        Paper,
        Scissors
    }

    public static class RockPaperScissorsHandExt
    {
        private static readonly RockPaperScissorsHand[] All = 
        {
            RockPaperScissorsHand.Rock,
            RockPaperScissorsHand.Paper,
            RockPaperScissorsHand.Scissors
        };

        public static RockPaperScissorsHand Opposite(this RockPaperScissorsHand hand)
        {
            switch (hand)
            {
                case RockPaperScissorsHand.Rock: return RockPaperScissorsHand.Paper;
                case RockPaperScissorsHand.Paper: return RockPaperScissorsHand.Scissors; 
                case RockPaperScissorsHand.Scissors: return RockPaperScissorsHand.Rock;
                default: throw new ArgumentOutOfRangeException(nameof(hand));
            }
        }

        public static RockPaperScissorsHand Random()
        {
            return All[UnityEngine.Random.Range(0, All.Length)];
        }

    }
}