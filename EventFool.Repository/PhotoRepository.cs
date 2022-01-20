using EventFool.Domain;
using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace EventFool.Repository
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        
        public PhotoRepository(Eventfool dbContext) : base(dbContext)
        {
            
        }

       

        public override void Update(Photo entity)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Photo> ReadAll(string id)
        {
            return _dbContext.Users.Find(id).Photos.ToList();

        }
        public Photo ReadEager(Guid photoId) 
        {
            return _dbContext.Photos.Include("User").Where(x => x.Id == photoId).FirstOrDefault();
        }
        public IEnumerable<Photo> ReadAllEager(string id)
        {
            return _dbContext.Photos.Include("User").Where(x => x.User.Id == id);
        }
        //public byte[] GetImageFromDataBase(Guid Id)
        //{
        //    //return await _context.Orders.Where(c => c.OrderDetails.Contains(orderName)).ToListAsync();
        //    var q = from temp in _dbContext.Photos where temp.Id == Id select temp.Description;
        //    byte[] cover = q.First();

        //    return  cover;

        //}

        public void Upload(HttpPostedFileBase file, Photo photo)
        {
            throw new NotImplementedException();
        }

        //public void UploadImageInDataBase(HttpPostedFileBase file, Photo photo)
        //{
        //    photo.Description = ConvertToBytes(file);
        //    _dbContext.Photos.Add(photo);



        //}

        //public byte[] ConvertToBytes(HttpPostedFileBase image)
        //{
        //    byte[] imageBytes = null;
        //    BinaryReader reader = new BinaryReader(image.InputStream);
        //    imageBytes = reader.ReadBytes((int)image.ContentLength);
        //    return imageBytes;
        //}


    }
}
