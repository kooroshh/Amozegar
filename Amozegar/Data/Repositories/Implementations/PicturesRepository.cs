using Amozegar.Areas.Shared.Models;
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

        // Utilities

        private async Task<TableType> getPictureTypeByType(string type)
        {
            return await this._context.TableTypes
                .SingleAsync(pt => pt.Type == type);
        }

        private async Task<int> getClassIdByClassIdentity(string classIdentity)
        {
            var cls = await this._context.Classes
                .Select(c => new { c.ClassId, c.ClassIdentity })
                .SingleAsync(c => c.ClassIdentity == classIdentity);

            return cls.ClassId;
        }

        // Main Methods
        public async Task AddImagesAsync(string path, int recordId, string type, string classIdentity)
        {
            var classId = await this.getClassIdByClassIdentity(classIdentity);
            var pictureType = await this.getPictureTypeByType(type);

            var newPicture = new Picture()
            {
                PicturePath = path,
                TableType = pictureType,
                TableTypeId = pictureType.TypeId,
                TableTypeRecordId = recordId,
                ClassId = classId
            };

            await this._context.Pictures.AddAsync(newPicture);

        }

        public async Task DeleteByClassIdentityByTypeAndRecordIdAsync(string classIdentity, int recordId, string type)
        {
            var classId = await this.getClassIdByClassIdentity(classIdentity);
            var pictureType = await this.getPictureTypeByType(type);

            var pictures = await this._context.Pictures
                .Where(p => p.TableType == pictureType && p.TableTypeRecordId == recordId && p.ClassId == classId)
                .ToListAsync();

            this._context.Pictures.RemoveRange(pictures);
        }

        public async Task<List<string>> GetPicturesPathsByClassIdentityByTypeAndRecordIdAsync(string classIdentity, int recordId, string type)
        {
            var pictureType = await this.getPictureTypeByType(type);
            var classId = await this.getClassIdByClassIdentity(classIdentity);
            var pictures = await this._context.Pictures
                .Where(p => p.TableType == pictureType && p.TableTypeRecordId == recordId && p.ClassId == classId)
                .Select(p => p.PicturePath)
                .ToListAsync();

            return pictures;
        }

        public async Task<List<PictureForEditViewModel>> GetPicturesForEditByClassIdentityByTypeAndRecordIdAsync(string classIdentity, int recordId, string type)
        {
            var pictureType = await this.getPictureTypeByType(type);
            var classId = await this.getClassIdByClassIdentity(classIdentity);

            var pictures = await this._context.Pictures
                .Where(p => p.TableType == pictureType && p.TableTypeRecordId == recordId && p.ClassId == classId)
                .Select(p => new PictureForEditViewModel()
                {
                    PicturePath = p.PicturePath,
                    PictureId = p.PictureId
                })
                .ToListAsync();

            return pictures;
        }

        public async Task<Picture?> GetPictureByClassIdentityByTypeAndRecordIdAndPictureIdAsync(string classIdentity, int pictureId, int recordId, string type)
        {
            var pictureType = await this.getPictureTypeByType(type);
            var classId = await this.getClassIdByClassIdentity(classIdentity);

            var picture = await this._context.Pictures
                .SingleOrDefaultAsync(p =>
                    p.TableType == pictureType &&
                    p.TableTypeRecordId == recordId &&
                    p.PictureId == pictureId &&
                    p.ClassId == classId
                );


            return picture;

        }

        public async Task<int> GetLastImageCountByClassIdentityByRecordIdByTypeForAddAsync(string classIdentity, int recordId, string type)
        {
            var picturesPaths = await this.GetPicturesPathsByClassIdentityByTypeAndRecordIdAsync(classIdentity, recordId, type);

            var lastCount = 0;

            foreach (var picture in picturesPaths)
            {
                var startIndex = picture.IndexOf(Path.DirectorySeparatorChar) + 1;
                var lastIndex = picture.IndexOf(".");
                var number = int.Parse(picture.Substring(startIndex, lastIndex - startIndex));

                if (number > lastCount)
                {
                    lastCount = number;
                }
            }

            return (lastCount + 1);
        }
    }
}
