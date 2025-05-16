using Amozegar.Areas.Shared.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IPicturesRepository : IGenericRepository<Picture>
    {
        public Task AddImagesAsync(string path, int recordId, string type, string classIdentity);
        public Task DeleteByClassIdentityByTypeAndRecordIdAsync(string classIdentity, int recordId, string type);
        public Task<List<string>> GetPicturesPathsByClassIdentityByTypeAndRecordIdAsync(string classIdentity, int recordId, string type);
        public Task<List<PictureForEditViewModel>> GetPicturesForEditByClassIdentityByTypeAndRecordIdAsync(string classIdentity, int recordId, string type);
        public Task<Picture?> GetPictureByClassIdentityByTypeAndRecordIdAndPictureIdAsync(string classIdentity, int pictureId, int recordId, string type);
        public Task<int> GetLastImageCountByClassIdentityByRecordIdByTypeForAddAsync(string classIdentity, int recordId, string type);
    }
}
