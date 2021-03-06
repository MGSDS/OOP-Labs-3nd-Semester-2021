using System.IO;
using Backups.Entities;
using Backups.Repositories;

namespace BackupsExtra.Repository
{
    public interface IExtraBackupDestinationRepository : IBackupDestinationRepository
    {
        void Read(Storage storage, Stream stream);
        void Write(Storage storage, Stream stream);
        void Delete(Storage storage);
    }
}