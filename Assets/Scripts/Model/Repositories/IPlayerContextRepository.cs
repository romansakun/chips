namespace Model
{
    public interface IPlayerContextRepository
    {
        int GetChipsCount(string chipId);

        void UpdateChipsCount(string chipId, int newCount);
    }
}