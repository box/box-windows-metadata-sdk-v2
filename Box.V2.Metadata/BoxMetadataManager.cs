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
    /// <summary>
    /// Box Resource manager for the Metadata endpoint 
    /// This endpoint is currently in beta and must be added to the Box Client through the Resource Plugin interface
    /// </summary>
    public class BoxMetadataManager : BoxResourceManager
    {
        private const string MetadataEndpointPath = @"files/{0}/metadata/";
        private const string JsonPatchContentType = "application/json-patch+json";

        public BoxMetadataManager(IBoxConfig config, IBoxService service, IBoxConverter converter, IAuthRepository auth)
            : base(config, service, converter, auth) { }


        /// <summary>
        /// Retrieves the metadata for the given file id
        /// </summary>
        /// <param name="id">ID of the file to retrieve metadata from</param>
        /// <returns></returns>
        public async Task<BoxMetadata> GetMetadata(string id)
        {
            id.ThrowIfNullOrWhiteSpace("id");

            BoxRequest request = new BoxRequest(new Uri(Constants.BoxApiUriString + string.Format(MetadataEndpointPath, id)));

            IBoxResponse<BoxMetadata> response = await ToResponseAsync<BoxMetadata>(request).ConfigureAwait(false);

            return response.ResponseObject;
        }

        /// <summary>
        /// Send a collection of BoxMetadataRequests 
        /// The current available actions include: Add, Edit, Test, and Remove
        /// </summary>
        /// <param name="id">ID of the file to edit the metadata of</param>
        /// <param name="metadataRequests">The collection of metadata operations</param>
        /// <returns></returns>
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
