namespace CleanTemplate.Application.UseCases
{
    public interface IUseCaseBase
    {
        void Commit();
        void BeginTransaction();
    }
}
