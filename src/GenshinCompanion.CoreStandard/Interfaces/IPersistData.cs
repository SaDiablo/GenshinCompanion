using System.Threading.Tasks;

namespace GenshinCompanion.CoreStandard.Interfaces
{
    public interface IPersistData
    {
        Task Open();
        void Save();
    }
}