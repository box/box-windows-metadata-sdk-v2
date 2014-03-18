using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Box.V2.Models;
using System.Threading.Tasks;
using Box.V2.Managers;
using System.Collections.Generic;

namespace Box.V2.Test.Integration
{
    [TestClass]
    public class BoxMetadataManagerTestIntegration : BoxResourceManagerTestIntegration
    {
        const string TestFileId = "TEST_FILE_ID";

        public BoxMetadataManagerTestIntegration()
        {
            _client.AddResourcePlugin<BoxMetadataManager>();
        }

        [TestMethod]
        public async Task CreateMetadata_LiveSession_ValidResponse()
        {
            Dictionary<string, string> mdReq = new Dictionary<string, string>() 
            {
                { "client_number","820183"}, 
                { "client_name", "Biomedical Corp"}, 
                { "case_reference", "A83JAA"}, 
                { "case_type", "Employment Litigation"}, 
                { "assigned_attorney", "Francis Burke" },
                { "case_status", "in-progress"}
            };

            var md = await _client.ResourcePlugins.Get<BoxMetadataManager>().CreateMetadata(TestFileId, mdReq);

            Assert.IsNotNull(md.Id);
            Assert.AreEqual("properties", md.Type);
            Assert.AreEqual(string.Format("file_{0}", TestFileId), md.Parent);
            Assert.AreEqual("820183", md["client_number"]);
            Assert.AreEqual("Biomedical Corp", md["client_name"]);
            Assert.AreEqual("A83JAA", md["case_reference"]);
            Assert.AreEqual("Employment Litigation", md["case_type"]);
            Assert.AreEqual("Francis Burke", md["assigned_attorney"]);
            Assert.AreEqual("in-progress", md["case_status"]);
        }

        [TestMethod]
        public async Task GetMetadata_LiveSession_ValidResponse()
        {
            /*** Act ***/
            BoxMetadata md = await _client.ResourcePlugins.Get<BoxMetadataManager>().GetMetadata(TestFileId);

            /*** Assert ***/
            Assert.IsNotNull(md.Id);
            Assert.AreEqual("properties", md.Type);
            Assert.AreEqual(string.Format("file_{0}", TestFileId), md.Parent);
            Assert.AreEqual("820183", md["client_number"]);
            Assert.AreEqual("Biomedical Corp", md["client_name"]);
            Assert.AreEqual("A83JAA", md["case_reference"]);
            Assert.AreEqual("Employment Litigation", md["case_type"]);
            Assert.AreEqual("Francis Burke", md["assigned_attorney"]);
            Assert.AreEqual("in-progress", md["case_status"]);
        }

        [TestMethod]
        public async Task UpdateMetadata_LiveSession_ValidResponse()
        {
            /*** Arrange ***/
            var mdReq = new BoxMetadataRequest[]
            {
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Test, Path="assigned_attorney", Value="Francis Burke"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Replace, Path="assigned_attorney", Value="Eugene Huang"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Test, Path="case_status", Value="in-progress"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Remove, Path="case_status", Value="Francis Burke"},
                new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path="retention_length", Value="7_years"}
            };

            /*** Act ***/
            BoxMetadata md = await _client.ResourcePlugins.Get<BoxMetadataManager>().UpdateMetadata(TestFileId, mdReq);

            /*** Assert ***/

            Assert.IsNotNull(md.Id);
            Assert.AreEqual("properties", md.Type);
            Assert.AreEqual(string.Format("file_{0}", TestFileId), md.Parent);
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
            /*** Act ***/
            bool isSuccess = await _client.ResourcePlugins.Get<BoxMetadataManager>().DeleteMetadata(TestFileId);

            Assert.IsTrue(isSuccess);
        }

        [TestMethod]
        public async Task CreateOrUpdateMetadata_SameRequest_ValidResponse()
        {
            Dictionary<string, string> mdReq = new Dictionary<string, string>() 
            {
                { "client_number","820183"}, 
                { "client_name", "Biomedical Corp"}, 
                { "case_reference", "A83JAA"}, 
                { "case_type", "Employment Litigation"}, 
                { "assigned_attorney", "Francis Burke" },
                { "case_status", "in-progress"}
            };

            var md = await _client.ResourcePlugins.Get<BoxMetadataManager>().CreateOrUpdateMetadata(TestFileId, mdReq);

            Assert.IsNotNull(md.Id);
            Assert.AreEqual("properties", md.Type);
            Assert.AreEqual(string.Format("file_{0}", TestFileId), md.Parent);
            Assert.AreEqual("820183", md["client_number"]);
            Assert.AreEqual("Biomedical Corp", md["client_name"]);
            Assert.AreEqual("A83JAA", md["case_reference"]);
            Assert.AreEqual("Employment Litigation", md["case_type"]);
            Assert.AreEqual("Francis Burke", md["assigned_attorney"]);
            Assert.AreEqual("in-progress", md["case_status"]);

            md = await _client.ResourcePlugins.Get<BoxMetadataManager>().CreateOrUpdateMetadata(TestFileId, mdReq);

            Assert.IsNotNull(md.Id);
            Assert.AreEqual("properties", md.Type);
            Assert.AreEqual(string.Format("file_{0}", TestFileId), md.Parent);
            Assert.AreEqual("820183", md["client_number"]);
            Assert.AreEqual("Biomedical Corp", md["client_name"]);
            Assert.AreEqual("A83JAA", md["case_reference"]);
            Assert.AreEqual("Employment Litigation", md["case_type"]);
            Assert.AreEqual("Francis Burke", md["assigned_attorney"]);
            Assert.AreEqual("in-progress", md["case_status"]);
        }

        [TestMethod]
        public async Task CreateOrUpdateMetadata_NewKey_ValidResponse()
        {
            Dictionary<string, string> mdReq = new Dictionary<string, string>() 
            {
                {"retention_length", "5_years"}
            };

            var md = await _client.ResourcePlugins.Get<BoxMetadataManager>().CreateOrUpdateMetadata(TestFileId, mdReq);
        }

    }
}
