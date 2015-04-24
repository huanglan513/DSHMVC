using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;
using DSHOrder.Common;

namespace DSHOrder.Repository
{
    public class UploadFileRepository : ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityUploadFile; }
        }

       
    }
}
