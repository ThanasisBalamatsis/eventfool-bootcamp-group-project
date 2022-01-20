using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using EventFool.Domain.Models;

namespace EventFool.Repository.Interfaces
{
    public interface IPhotoRepository : IGenericRepository<Photo>
    {
        void Upload(HttpPostedFileBase file, Photo photo);

        //byte[] GetImageFromDataBase(Guid Id);

        //void UploadImageInDataBase(HttpPostedFileBase file, Photo photo);
        IEnumerable<Photo> ReadAll(string id);
        IEnumerable<Photo> ReadAllEager(string id);
    }
}
