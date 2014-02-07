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
            var responseString = "{\"client_number\": \"820183\",\"client_name\": \"Biomedical Corp\",\"case_reference\": \"A83JAA\",\"case_type\": \"Employment Litigation\",\"assigned_attorney\": \"Francis Burke\",\"case_status\": \"in-progress\"}";
            _handler.Setup(h => h.ExecuteAsync<BoxMetadata>(It.IsAny<IBoxRequest>()))
                .Returns(Task.FromResult<IBoxResponse<BoxMetadata>>(new BoxResponse<BoxMetadata>()
                {
                    Status = ResponseStatus.Success,
                    ContentString = responseString
                }));

            /*** Act ***/
            //BoxMetadata md = await _client.Plugins.GetResource<BoxMetadataManager>().GetMetadata("fakeId");
            BoxMetadata md = await _metadataManager.GetMetadata("fakeId");

            /*** Assert ***/
            Assert.AreEqual("820183", md["client_number"]);
            Assert.AreEqual("Biomedical Corp", md["client_name"]);
            Assert.AreEqual("A83JAA", md["case_reference"]);
            Assert.AreEqual("Employment Litigation", md["case_type"]);
            Assert.AreEqual("Francis Burke", md["assigned_attorney"]);
            Assert.AreEqual("in-progress", md["case_status"]);

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
            Assert.AreEqual(0, md.Count);
        }
    }
}
