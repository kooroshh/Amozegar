using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class PicturesRepository : GenericRepository<Picture>, IPicturesRepository
    {
        public PicturesRepository(AmozegarContext context) : base(context)
        {
        }

        private async Task<PictureType> getPictureTypeByType(string type)
        {
            return await this._context.PictureTypes
                .SingleAsync(pt => pt.Type == type);
        }

        public async Task AddImagesAsync(string path, int recordId, string type)
        {
            var pictureType = await this.getPictureTypeByType(type);

            var newPicture = new Picture()
            {
                PicturePath = path,
                PictureType = pictureType,
                PictureTypeId = pictureType.TypeId,
                PictureTypeRecordId = recordId
            };

            await this._context.Pictures.AddAsync(newPicture);

        }

        public async Task DeleteByTypeAndRecordIdAsync(int recordId, string type)
        {
            var pictureType = await this.getPictureTypeByType(type);

            var pictures = await this._context.Pictures
                .Where(p => p.PictureType == pictureType && p.PictureTypeRecordId == recordId)
                .ToListAsync();

            this._context.Pictures.RemoveRange(pictures);
        }

        public async Task<List<string>> GetPicturesPathsByTypeAndRecordIdAsync(int recordId, string type)
        {
            var pictureType = await this.getPictureTypeByType(type);

            var pictures = await this._context.Pictures
                .Where(p => p.PictureType == pictureType && p.PictureTypeRecordId == recordId)
                .Select(p => p.PicturePath)
                .ToListAsync();

            return pictures;
        }

        public async Task<List<PictureForEditViewModel>> GetPicturesForEditByTypeAndRecordIdAsync(int recordId, string type)
        {
            var pictureType = await this.getPictureTypeByType(type);

            var pictures = await this._context.Pictures
                .Where(p => p.PictureType == pictureType && p.PictureTypeRecordId == recordId)
                .Select(p => new PictureForEditViewModel()
                {
                    PicturePath = p.PicturePath,
                    PictureId = p.PictureId
                })
                .ToListAsync();

            return pictures;
        }

        public async Task<Picture?> GetPictureByTypeAndRecordIdAndPictureIdAsync(int pictureId, int recordId, string type)
        {
            var pictureType = await this.getPictureTypeByType(type);

            var picture = await this._context.Pictures
                .SingleOrDefaultAsync(p =>
                    p.PictureType == pictureType &&
                    p.PictureTypeRecordId == recordId &&
                    p.PictureId == pictureId
                );


            return picture;

        }
    }
}
