namespace ViewModel
{
    public interface IFileSelector
    {
        string SelectSource();

        string SelectTarget();

        void FailureAlert(); 
    }
}

