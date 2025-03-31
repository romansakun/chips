namespace Definitions
{
    public enum PlayerType
    {
        MyPlayer,
        LeftPlayer,
        RightPlayer
    }

    public static class PlayerTypeExt
    {
        public static readonly PlayerType[] AllPlayers = 
        {
            PlayerType.MyPlayer,
            PlayerType.LeftPlayer,
            PlayerType.RightPlayer
        };
    }
}