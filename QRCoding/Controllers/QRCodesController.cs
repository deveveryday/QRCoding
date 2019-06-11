using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QRCoding.Controllers
{
    public class QRCodesController : ApiController
    {
        // GET: api/QRCodes
        public IEnumerable<string> Get()
        {
            return new string[] { "use /qrcodes/?q={website adress to be qrcoded}" };
        }

        // GET: api/QRCodes/5
        public HttpResponseMessage Get([FromUri]string q)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(q, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(8);
            
            Bitmap canvas = qrCodeImage;

            var ms = new MemoryStream();
            canvas.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            HttpResponseMessage r = Request.CreateResponse();
            r.Content = new StreamContent(ms);
            r.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            r.Content.Headers.Add("base64Image", Convert.ToBase64String(ms.GetBuffer()));

            return r;
        }
    }
}
