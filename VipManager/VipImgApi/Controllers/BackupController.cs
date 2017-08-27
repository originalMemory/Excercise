using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using VipData.Helper;
using VipData.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace VipImgApi.Controllers
{
    public class BackupController : ApiController
    {
        [HttpPost]
        public ResultDto BackupCommand(BackupPost data)
        {
            var result = new ResultDto();
            try
            {
                //获取用户的数据存储位置
                var userObjId = new ObjectId(data.userId);

            }
        }
    }
}