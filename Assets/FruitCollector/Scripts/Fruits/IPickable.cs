public interface IPickable
{
    string Id { get; }
    string DisplayName { get; }
    void Pick(IStorable receiver);
}