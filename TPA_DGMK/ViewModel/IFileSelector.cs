namespace ViewModel
{
    public interface IFileSelector
    {
        string SelectSource();

        void FailureAlert(); 
    }
}

