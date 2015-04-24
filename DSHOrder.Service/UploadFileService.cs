using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Service.Interface;
using DSHOrder.Repository;
using DSHOrder.Entity;
using System.Web.Security;
using DSHOrder.Common;

namespace DSHOrder.Service
{
    public class UploadFileService : IUploadFileService
    {
        UploadFileRepository UploadFileRepo = null;

        public UploadFileService()
        {
            UploadFileRepo = new UploadFileRepository();
        }


        #region IUploadFileService 成员

        public UploadFile GetCodeTablesByID(int type)
        {
            throw new NotImplementedException();
        }

        public UploadFile Add(UploadFile entity)
        {
            return UploadFileRepo.Add(entity);
        }

        #endregion

        #region IUploadFileService 成员


        public UploadFile GetById(int Id)
        {
            return UploadFileRepo.GetBy<UploadFile>(m => m.UploadFileID == Id);
        }

        #endregion
    }

}
