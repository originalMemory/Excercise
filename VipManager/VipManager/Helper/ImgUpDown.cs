using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VipData.Model;
using System.Drawing;

namespace VipManager.Helper
{
    public class ImgUpDown
    {
        /// <summary>
        /// 根据md5上传文件
        /// </summary>
        /// <param name="filePath">本地图片路径</param>
        /// <returns></returns>
        public Result UploadImg(string filePath,UserImgType type)
        {
            Result result=new Result();
            try
            {
                if (File.Exists(filePath))
                {
                    result.Message = "图片不存在！";
                }
                //读取图片文件
                FileStream fs = new FileStream(filePath, FileMode.Open);
                byte[] bs = new byte[fs.Length];       //比特流
                BinaryReader br = new BinaryReader(fs);
                br.Read(bs, 0, bs.Length);
                UserImgMongo img = new UserImgMongo
                {
                    Name = Path.GetFileName(filePath),
                    Bytes = bs,
                    CreateAt = DateTime.Now.AddHours(8),
                    Extention = Path.GetExtension(filePath),
                    Size = bs.Length,
                    Type = type
                };
                MongoDBHelper.Instance.GetUserImg().InsertOne(img);
                result.IsSuccess = true;
            }
            catch (Exception)
            {
                result.Message = "网络状态不好，请重试！";
            }
            return result;
        }

        

        ///// <summary>
        ///// 根据md5下载文件
        ///// </summary>
        ///// <param name="fileId">文件唯一标识</param>
        ///// <returns></returns>
        //[HttpGet]
        //public HttpResponseMessage DownloadFile(string fileId)
        //{
        //    var builder = Builders<UserImgMongo>.Filter;
        //    var filter = builder.Eq(x => x._id, new ObjectId(fileId));
        //    var file = MongoDBHelper.Instance.GetUserImgMongo().Find(filter).FirstOrDefault();

        //    if (file != null)
        //    {
        //        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //        MemoryStream stream = new MemoryStream(file.Bytes);
        //        response.Content = new StreamContent(stream);
        //        if (file.FileType == "png")
        //            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        //        else
        //        {
        //            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //            {
        //                FileName = file.FileName
        //            };
        //        }

        //        return response;
        //    }
        //    return ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, "");
        //}
    }
}
