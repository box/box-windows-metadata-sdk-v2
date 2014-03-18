using Box.V2.Auth;
using Box.V2.Config;
using Box.V2.Managers;
using Box.V2.Models;
using Box.V2.Plugins.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Box.V2.Test
{
    [TestClass]
    public class BoxMetadataManagerTest : BoxResourceManagerTest
    {
        protected BoxMetadataManager _metadataManager;

        public BoxMetadataManagerTest()
        {
            _metadataManager = new BoxMetadataManager(_config.Object, _service, _converter, _authRepository);
        }

        [TestMethod]
        public async Task GetMetadata_ValidResponse_ValidMetadata()
        {
            /*** Arrange ***/
            var responseString = "{\"$id\":\"c79896a0-a33f-11e3-a5e2-0800200c9a66\",\"$type\": \"properties\",\"$parent\": \"file_552345101\",\"client_number\": \"820183\",\"client_name\": \"Biomedical Corp\",\"case_reference\": \"A83JAA\",\"case_type\": \"Employment Litigation\",\"assigned_attorney\": \"Francis Burke\",\"case_status\": \"in-progress\"}";
            _handler.Setup(h => h.ExecuteAsync<BoxMetadata>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadata>>(new BoxResponse<BoxMetadata>()
                {
                    Status = ResponseStatus.Success,
                    ContentString = responseString
                }));

            /*** Act ***/
            BoxMetadata md = await _metadataManager.GetMetadata("fakeId");

            /*** Assert ***/
            Assert.AreEqual("c79896a0-a33f-11e3-a5e2-0800200c9a66", md.Id);
            Assert.AreEqual("properties", md.Type);
            Assert.AreEqual("file_552345101", md.Parent);
            Assert.AreEqual("820183", md["client_number"]);
            Assert.AreEqual("Biomedical Corp", md["client_name"]);
            Assert.AreEqual("A83JAA", md["case_reference"]);
            Assert.AreEqual("Employment Litigation", md["case_type"]);
            Assert.AreEqual("Francis Burke", md["assigned_attorney"]);
            Assert.AreEqual("in-progress", md["case_status"]);
        }

        [TestMethod]
        public async Task CreateMetadata_ValidResponse_ValidMetadata()
        {
            /*** Arrange ***/
            var responseString = "{\"$id\":\"c79896a0-a33f-11e3-a5e2-0800200c9a66\",\"$type\": \"properties\",\"$parent\": \"file_552345101\",\"client_number\": \"820183\",\"client_name\": \"Biomedical Corp\",\"case_reference\": \"A83JAA\",\"case_type\": \"Employment Litigation\",\"assigned_attorney\": \"Francis Burke\",\"case_status\": \"in-progress\"}";
            
            _handler.Setup(h => h.ExecuteAsync<BoxMetadata>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadata>>(new BoxResponse<BoxMetadata>()
                {
                    Status = ResponseStatus.Success,
                    ContentString = responseString
                }));

            Dictionary<string, string> mdReq = new Dictionary<string, string>() 
            {
                { "client_number","820183"}, 
                { "client_name", "Biomedical Corp"}, 
                { "case_reference", "A83JAA"}, 
                { "case_type", "Employment Litigation"}, 
                { "assigned_attorney", "Francis Burke" },
                { "case_status", "in-progress"}
            };

            /*** Act ***/
            BoxMetadata md = await _metadataManager.CreateMetadata("fakeId", mdReq);

            /*** Assert ***/
            Assert.AreEqual("c79896a0-a33f-11e3-a5e2-0800200c9a66", md.Id);
            Assert.AreEqual("properties", md.Type);
            Assert.AreEqual("file_552345101", md.Parent);
            Assert.AreEqual("820183", md["client_number"]);
            Assert.AreEqual("Biomedical Corp", md["client_name"]);
            Assert.AreEqual("A83JAA", md["case_reference"]);
            Assert.AreEqual("Employment Litigation", md["case_type"]);
            Assert.AreEqual("Francis Burke", md["assigned_attorney"]);
            Assert.AreEqual("in-progress", md["case_status"]);
        }

        [TestMethod]
        public async Task UpdateMetadata_ValidResponse_ValidMetadata()
        {
            /*** Arrange ***/
            var responseString = "{\"$id\":\"c79896a0-a33f-11e3-a5e2-0800200c9a66\",\"$type\": \"properties\",\"$parent\": \"file_552345101\",\"client_number\": \"820183\",\"client_name\": \"Biomedical Corp\",\"case_reference\": \"A83JAA\",\"case_type\": \"Employment Litigation\",\"assigned_attorney\": \"Eugene Huang\",\"retention_length\": \"7_years\"}";
            
            _handler.Setup(h => h.ExecuteAsync<BoxMetadata>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadata>>(new BoxResponse<BoxMetadata>()
                {
                    Status = ResponseStatus.Success,
                    ContentString = responseString
                }));

            var mdReq = new BoxMetadataRequest[]
            {
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Test, Path="/assigned_attorney", Value="Francis Burke"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Replace, Path="/assigned_attorney", Value="Eugene Huang"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Test, Path="/case_status", Value="in-progress"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Remove, Path="/case_status", Value="Francis Burke"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path="/retention_length", Value="7_years"}
            };

            /*** Act ***/
            BoxMetadata md = await _metadataManager.UpdateMetadata("fakeId", mdReq);

            /*** Assert ***/

            Assert.AreEqual("c79896a0-a33f-11e3-a5e2-0800200c9a66", md.Id);
            Assert.AreEqual("properties", md.Type);
            Assert.AreEqual("file_552345101", md.Parent);
            Assert.AreEqual("820183", md["client_number"]);
            Assert.AreEqual("Biomedical Corp", md["client_name"]);
            Assert.AreEqual("A83JAA", md["case_reference"]);
            Assert.AreEqual("Employment Litigation", md["case_type"]);
            Assert.AreEqual("Eugene Huang", md["assigned_attorney"]);
            Assert.AreEqual("7_years", md["retention_length"]);
        }

        [TestMethod]
        public async Task DeleteMetadata_ValidResponse_ValidResponse()
        {
            /*** Arrange ***/
            var responseString = "{}";
            _handler.Setup(h => h.ExecuteAsync<BoxMetadata>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadata>>(new BoxResponse<BoxMetadata>()
                {
                    Status = ResponseStatus.Success,
                    StatusCode = System.Net.HttpStatusCode.NoContent,
                    ContentString = responseString
                }));

            /*** Act ***/
            bool isSuccess = await _metadataManager.DeleteMetadata("fakeId");

            Assert.IsTrue(isSuccess);
        }

        [TestMethod]
        public async Task GetMetadata_EmptyResponse_EmptyMetadata()
        {
            /*** Arrange ***/
            var responseString = "{}";
            _handler.Setup(h => h.ExecuteAsync<BoxMetadata>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadata>>(new BoxResponse<BoxMetadata>()
                {
                    Status = ResponseStatus.Success,
                    ContentString = responseString
                }));

            /*** Act ***/
            BoxMetadata md = await _metadataManager.GetMetadata("fakeId");

            /*** Assert ***/
            //Assert.AreEqual(0, md.Count);
        }
    }
}
