namespace Gameplay.Battle
{
    public enum PlayerRockPaperScissorsHand
    {
        Rock,
        Paper,
        Scissors
    }
    
    public static class PlayerRockPaperScissorsHandExtensions
    {
        public static bool IsWin(this PlayerRockPaperScissorsHand hand, PlayerRockPaperScissorsHand otherHand)
        {
            return (hand == PlayerRockPaperScissorsHand.Rock && otherHand == PlayerRockPaperScissorsHand.Scissors) ||
                   (hand == PlayerRockPaperScissorsHand.Paper && otherHand == PlayerRockPaperScissorsHand.Rock) ||
                   (hand == PlayerRockPaperScissorsHand.Scissors && otherHand == PlayerRockPaperScissorsHand.Paper);
        }
    }
}