using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using SmartVideo.Model.Document;

namespace SmartCloud.Mvc.Controllers
{
    [Produces("application/json")]
    [Route("api/Document")]
    public class DocumentController : Controller
    {

        private DocumentClient documentClient;

        public DocumentController()
        {
            documentClient = new DocumentClient(new Uri(Constants.DocumentDbEndPointUri), Constants.DocumentDbAccessKey);
            documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = Constants.DocumentDbDatabaseName }).Wait();
            documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(Constants.DocumentDbDatabaseName), new DocumentCollection { Id = Constants.DocumentDbEventVideoCollectionName }).Wait();
        }

        // POST: api/Document
        [HttpPost]
        public async Task Post([FromBody]MediaDocument document)
        {
            if ((document != null)&&(!String.IsNullOrEmpty(document.DeviceId)))
            {
                await documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(Constants.DocumentDbDatabaseName, Constants.DocumentDbEventVideoCollectionName), document);
            }
        }
    }
}
