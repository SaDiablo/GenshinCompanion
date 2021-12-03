using System.Threading.Tasks;

namespace GenshinCompanion.CoreStandard
{
    public interface IPersistData
    {
        Task Open();
        void Save();
    }
}