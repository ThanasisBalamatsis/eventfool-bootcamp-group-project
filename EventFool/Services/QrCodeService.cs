using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace EventFool.Services
{
    //public static class QrCodeService
    //{
    //    //public static IHtmlString GenerateQRCode(this HtmlHelper html, string url, string alt = "GeneratedQRCode", int height = 200, int width = 200, int margin = 0)
    //        public static IHtmlString GenerateQRCode(this HtmlHelper html, string url, string alt = "GeneratedQRCode", int height = 20, int width = 20, int margin = 0)
    //        {
    //            var qrWriter = new BarcodeWriter();
    //            qrWriter.Format = BarcodeFormat.QR_CODE;
    //            qrWriter.Options = new EncodingOptions() { Height = height, Width = width, Margin = margin };

    //            using (var q = qrWriter.Write(url))
    //            {
    //                using (var ms = new MemoryStream())
    //                {
    //                    q.Save(ms, ImageFormat.Png);
    //                    var img = new TagBuilder("img");
    //                    img.Attributes.Add("src", String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())));
    //                    img.Attributes.Add("alt", alt);
    //                    return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
    //                }
    //            }
    //        }

    //}
  

    public static class UrlExtensions
    {
        public static string Content(this UrlHelper urlHelper, string contentPath, bool toAbsolute = false)
        {
            var path = urlHelper.Content(contentPath);
            var url = new Uri(HttpContext.Current.Request.Url, path);

            return toAbsolute ? url.AbsoluteUri : path;
        }
    }
    public class QrCodeService
    {

        public void GenerateCode(string qrText, string userName, string fileName)
        {


            var width = 250; // width of the Qr Code
            var height = 250; // height of the Qr Code
            var margin = 0;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            var pixelData = qrCodeWriter.Write(qrText);
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var ms = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    try
                    {
                        // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
                    var ServerSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/") + userName + "/Tickets/");
                    if (!Directory.Exists(ServerSavePath))
                    {
                        Directory.CreateDirectory(ServerSavePath);
                    }
                    
                    // save to folder
                    string fileGuid = Guid.NewGuid().ToString().Substring(0, 4);
                    bitmap.Save(Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/") ,userName+"/Tickets/" +fileName) , System.Drawing.Imaging.ImageFormat.Png);

                    // save to stream as PNG
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    //byteArray = ms.ToArray();

                }
            }
        }
    }
}