using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Box.V2.Models;
using System.Threading.Tasks;
using Box.V2.Managers;

namespace Box.V2.Test.Integration
{
    [TestClass]
    public class BoxMetadataManagerTestIntegration : BoxResourceManagerTestIntegration
    {
        const string _fileId = "YOUR_TEST_FILEID";

        public BoxMetadataManagerTestIntegration()
        {
            _client.AddResourcePlugin<BoxMetadataManager>();
        }

        [TestMethod]
        public async Task AddMetadata_LiveSession_ValidResponse()
        {
            /*** Arrange ***/
            var req = new BoxMetadataRequest[] 
            { 
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path = "/case_type", Value="Employment Litigation"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path = "/assigned_attorney", Value="Francis Burke"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path = "/case_status", Value="in-progress"},
            };

            /*** Act ***/
            BoxMetadata md = await _client.ResourcePlugins.Get<BoxMetadataManager>().EditMetadata(_fileId, req);

            /*** Assert ***/
            Assert.IsTrue(md.Count > 0);
        }


        [TestMethod]
        public async Task GetMetadata_LiveSession_ValidResponse()
        {
            /*** Act ***/
            BoxMetadata md = await _client.ResourcePlugins.Get<BoxMetadataManager>().GetMetadata(_fileId);

            /*** Assert ***/
            Assert.AreEqual("Employment Litigation", md["case_type"]);
            Assert.AreEqual("Francis Burke", md["assigned_attorney"]);
            Assert.AreEqual("in-progress", md["case_status"]);
        }

        [TestMethod]
        public async Task EditMetadata_FullWorkflow_ValidResponse()
        {
            /*** Add ***/
            var req = new BoxMetadataRequest[] 
            { 
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path = "/key", Value="value"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path = "/key2", Value="value2"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path = "/key3", Value="value3"},
            };

            BoxMetadata md = await _client.ResourcePlugins.Get<BoxMetadataManager>().EditMetadata(_fileId, req);

            Assert.AreEqual("value", md["key"]);
            Assert.AreEqual("value2", md["key2"]);
            Assert.AreEqual("value3", md["key3"]);

            /*** Remove ***/

            req = new BoxMetadataRequest[]
            {
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Remove, Path = "/key2" },
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Remove, Path = "/key3" }
            };

            md = await _client.ResourcePlugins.Get<BoxMetadataManager>().EditMetadata(_fileId, req);

            Assert.AreEqual("value", md["key"]);

            /*** Test ***/

            req = new BoxMetadataRequest[]
            {
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Test, Path = "/key", Value = "value" }
            };

            md = await _client.ResourcePlugins.Get<BoxMetadataManager>().EditMetadata(_fileId, req);

            Assert.AreEqual("value", md["key"]);


            /*** Clean-up ***/

            req = new BoxMetadataRequest[]
            {
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Remove, Path = "/key" }
            };

            md = await _client.ResourcePlugins.Get<BoxMetadataManager>().EditMetadata(_fileId, req);
        }

    }
}
