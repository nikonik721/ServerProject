using System.Threading.Tasks;

namespace PS.DL.PackGenerator.Interface
{
    public interface IPackGenerator
    {
        Task<int> GeneratePacks(int envelopeNum);
    }
}
