using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Web.Http;
using System.Net.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using VipData.Model;
using VipData.Helper;
using System.Text.RegularExpressions;

namespace VipImgApi.Controllers
{
    public class ImgController : ApiController
    {
        /// <summary>
        /// 根据md5上传文件
        /// </summary>
        /// <param name="fileId">文件唯一标识</param>
        /// <param name="content_type">文件类型</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        [HttpPost]
        public bool UploadImg(string fileId)
        {
            try
            {
                //读取文件
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                byte[] bs = new byte[file.ContentLength];       //比特流
                file.InputStream.Read(bs, 0, bs.Length);        //获取文件流
                string extension = System.IO.Path.GetExtension(file.FileName);
                UserImgMongo ff = new UserImgMongo();
                ff.Bytes = bs;
                ff.Extention = extension;
                ff.Name = file.FileName;
                ff._id = new ObjectId(fileId);
                ff.Size = bs.Length;
                ff.CreateAt = DateTime.Now.AddHours(8);
                
                
                MongoDBHelper.Instance.GetUserImg().InsertOne(ff);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据md5下载文件
        /// </summary>
        /// <param name="fileId">文件唯一标识</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DownloadImg(string fileId)
        {
            var builder = Builders<UserImgMongo>.Filter;
            var filter = builder.Eq(x => x._id, new ObjectId(fileId));
            var file = MongoDBHelper.Instance.GetUserImg().Find(filter).FirstOrDefault();

            if (file != null)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                MemoryStream stream = new MemoryStream(file.Bytes);
                response.Content = new StreamContent(stream);
                Regex reg = new Regex("png|jpg|bmp|gif|jpeg");
                if (reg.IsMatch(file.Extention))
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                else
                {
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = file.Name
                    };
                }

                return response;
            }
            return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, "");
        }
    }
}