using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreWebGoat.Models;
using NetCoreWebGoat.Repositories;

namespace NetCoreWebGoat.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class CommentController : BaseController
    {
        private CommentRepository _commentRepository;

        public CommentController(ILogger<CommentController> logger, CommentRepository commentRepository) : base(logger)
        {
            _commentRepository = commentRepository;
        }

        [HttpPost]
        public ActionResult Post([FromBody] string xml)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.XmlResolver = new XmlUrlResolver();

            CreateCommentModel comment = null;

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CreateCommentModel));
                XmlReader reader = XmlReader.Create(stream, settings);
                comment = serializer.Deserialize(reader) as CreateCommentModel;
            }

            comment.UserId = UserId;
            _commentRepository.Add(comment.ToEntity());
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _commentRepository.Delete(id);
            return Ok();
        }
    }
}