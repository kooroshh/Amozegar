using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IPicturesRepository : IGenericRepository<Picture>
    {
        public Task AddImagesAsync(string path, int recordId, string type);
        public Task DeleteByTypeAndRecordIdAsync(int recordId, string type);
        public Task<List<string>> GetPicturesPathsByTypeAndRecordIdAsync(int recordId, string type);
        public Task<List<PictureForEditViewModel>> GetPicturesForEditByTypeAndRecordIdAsync(int recordId, string type);
        public Task<Picture?> GetPictureByTypeAndRecordIdAndPictureIdAsync(int pictureId, int recordId, string type);
    }
}
