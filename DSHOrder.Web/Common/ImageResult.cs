using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace DSHOrder.Web.Common
{
    public class ImageResult:ActionResult
    {
        public ImageResult() { }
        public Image Image { get; set; }
        public ImageFormat ImageFormat { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            // verify properties 
            if (Image == null)
            {
                throw new ArgumentNullException("Image");
            }
            if (ImageFormat == null)
            {
                throw new ArgumentNullException("ImageFormat");
            }
            // output 
            context.HttpContext.Response.Clear();
            if (ImageFormat.Equals(ImageFormat.Bmp)) context.HttpContext.Response.ContentType = "image/bmp";
            if (ImageFormat.Equals(ImageFormat.Gif)) context.HttpContext.Response.ContentType = "image/gif";
            if (ImageFormat.Equals(ImageFormat.Icon)) context.HttpContext.Response.ContentType = "image/vnd.microsoft.icon";
            if (ImageFormat.Equals(ImageFormat.Jpeg)) context.HttpContext.Response.ContentType = "image/jpeg";
            if (ImageFormat.Equals(ImageFormat.Png)) context.HttpContext.Response.ContentType = "image/png";
            if (ImageFormat.Equals(ImageFormat.Tiff)) context.HttpContext.Response.ContentType = "image/tiff";
            if (ImageFormat.Equals(ImageFormat.Wmf)) context.HttpContext.Response.ContentType = "image/wmf";
            Image.Save(context.HttpContext.Response.OutputStream, ImageFormat);
        }

        //public ActionResult Image(int id)
        //{
        //    var db = new NorthwindDataContext(connection);
        //    var found = db.Employees.FirstOrDefault(e => e.EmployeeID == id);


        //    if (found != null)
        //    {
        //        var buffer = found.Photo.ToArray();
        //        ImageConverter converter = new ImageConverter();
        //        var image = (Image)converter.ConvertFrom(buffer);
        //        return new Extensions.ImageResult()
        //        {
        //            Image = image,
        //            ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg
        //        };

        //    }
        //    else
        //    {
        //        ViewData["message"] = "员工不存在";
        //        return View("Error");
        //    }
        //}

          //<img src="/Employee/Image/<%:Model.EmployeeID %>" alt="" />
    }
}