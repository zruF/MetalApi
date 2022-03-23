namespace MetalServices.Contracts
{
    public interface IGenreService
    {
        Task<IEnumerable<string>> GetCommonGenres();
    }
}
