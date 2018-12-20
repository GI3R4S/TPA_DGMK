namespace ViewModel
{
    public interface IDatabaseSelector
    {
        string SelectSource();

        string SelectTarget();
    }
}
