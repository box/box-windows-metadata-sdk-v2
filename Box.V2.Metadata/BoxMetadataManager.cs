using Box.V2.Auth;
using Box.V2.Config;
using Box.V2.Converter;
using Box.V2.Extensions;
using Box.V2.Models;
using Box.V2.Plugins.Managers;
using Box.V2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Box.V2.Managers
{
    public class BoxMetadataManager : BoxResourceManager, IBoxMetadataManager
    {
        private const string MetadataEndpointPath = @"files/{0}/metadata/";
        private const string JsonPatchContentType = "application/json-patch+json";

        public BoxMetadataManager(IBoxConfig config, IBoxService service, IBoxConverter converter, IAuthRepository auth)
            : base(config, service, converter, auth) { }


        public async Task<BoxMetadata> GetMetadata(string id)
        {
            id.ThrowIfNullOrWhiteSpace("id");

            BoxRequest request = new BoxRequest(new Uri(Constants.BoxApiUriString + string.Format(MetadataEndpointPath, id)));

            IBoxResponse<BoxMetadata> response = await ToResponseAsync<BoxMetadata>(request).ConfigureAwait(false);

            return response.ResponseObject;
        }

        //public async Task<BoxMetadata> AddMetadata(string id, string name, string value)
        //{
        //    id.ThrowIfNullOrWhiteSpace("id");
        //    name.ThrowIfNullOrWhiteSpace("name");

        //    var metadataRequest = new BoxMetadataRequest
        //    {
        //        Op = BoxMetadataOperations.Add,
        //        Path = "/" + name,
        //        Value = value
        //    };

        //    return await EditMetadata(id, new BoxMetadataRequest[] { metadataRequest }).ConfigureAwait(false);
        //}

        //public async Task<BoxMetadata> RemoveMetadata(string id, string name)
        //{
        //    id.ThrowIfNullOrWhiteSpace("id");
        //    name.ThrowIfNullOrWhiteSpace("name");

        //    var metadataRequest = new BoxMetadataRequest
        //    {
        //        Op = BoxMetadataOperations.Remove,
        //        Path = "/" + name
        //    };

        //    return await EditMetadata(id, new BoxMetadataRequest[] { metadataRequest }).ConfigureAwait(false);
        //}

        //public async Task<BoxMetadata> TestMetadata(string id, string name, string value)
        //{
        //    id.ThrowIfNullOrWhiteSpace("id");
        //    name.ThrowIfNullOrWhiteSpace("name");

        //    var metadataRequest = new BoxMetadataRequest
        //    {
        //        Op = BoxMetadataOperations.Test,
        //        Path = "/" + name,
        //        Value = value
        //    };

        //    return await EditMetadata(id, new BoxMetadataRequest[] { metadataRequest }).ConfigureAwait(false);
        //}

        public async Task<BoxMetadata> EditMetadata(string id, BoxMetadataRequest[] metadataRequests)
        {
            id.ThrowIfNullOrWhiteSpace("id");
            metadataRequests.ThrowIfNull("metadataRequest");

            BoxRequest request = new BoxRequest(new Uri(Constants.BoxApiUriString + string.Format(MetadataEndpointPath, id)))
                .Method(RequestMethod.Put);

            request.Payload = _converter.Serialize(metadataRequests);
            request.ContentType = JsonPatchContentType;

            IBoxResponse<BoxMetadata> response = await ToResponseAsync<BoxMetadata>(request).ConfigureAwait(false);

            return response.ResponseObject;
        }

    }
}
