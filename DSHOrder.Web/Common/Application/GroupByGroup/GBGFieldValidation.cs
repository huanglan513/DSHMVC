using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using FluentValidation;
using DSHOrder.Web.Models;

namespace DSHOrder.Web.Common.Application.GroupByGroup
{
    public class GBGFlowInfoValidation : AbstractValidator<GroupByFlowInfo>
    {
        public GBGFlowInfoValidation()
        {
            //RuleSet("Names", () =>
            //{
            //    RuleFor(x => x.Surname).NotNull();
            //    RuleFor(x => x.Forename).NotNull();
            //});

            //RuleFor(x => x.Id).NotEqual(0);

            RuleFor(m => m.GroupByGroup.GroupByGroupName).NotNull().WithMessage("团购名称不能为空asdfsad").Length(0, 100).WithMessage("团购名称长度不能超过100个字符");
            RuleFor(m => m.Sales).NotNull().WithMessage("团购名称不能为空asdfsad");
            

            //RuleSet(GBGFlowEnumFLowNode.YWYTXTGSQB.ToString(), () =>
            //{
            //    RuleFor(m => m.GroupByGroup.GroupByGroupName).NotNull().WithMessage("团购名称不能为空").Length(0, 100).WithMessage("团购名称长度不能超过100个字符");
            //});

        }

    }

   

}