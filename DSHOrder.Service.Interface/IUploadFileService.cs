using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Service.Interface
{
    public interface IUploadFileService
    {
        UploadFile GetCodeTablesByID(int type);
        UploadFile Add(UploadFile entity);

        UploadFile GetById(int p);
    }
}
