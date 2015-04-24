﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSHOrder.Entity;

namespace DSHOrder.Repository
{
    public class GroupByCityRepository:ObjectRepository
    {
        protected override string ObjectSetName
        {
            get { return DataBaseEnum.EntityGroupByCity; }
        }
    }
}
