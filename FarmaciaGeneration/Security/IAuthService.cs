using FarmaciaGeneration.Model;

namespace FarmaciaGeneration.Security
{
    public interface IAuthService
    {
        Task<UserLogin?> Autenticar(UserLogin userLogin);
    }
}
