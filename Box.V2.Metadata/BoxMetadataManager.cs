using Box.V2.Auth;
using Box.V2.Config;
using Box.V2.Converter;
using Box.V2.Exceptions;
using Box.V2.Extensions;
using Box.V2.Models;
using Box.V2.Plugins.Managers;
using Box.V2.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private const string MetadataEndpointPath = @"files/{0}/metadata/{1}";
        private const string JsonContentType = "application/json";
        private const string JsonPatchContentType = "application/json-patch+json";
        private const string DefaultTypeInstance = "properties";


        public BoxMetadataManager(IBoxConfig config, IBoxService service, IBoxConverter converter, IAuthRepository auth)
            : base(config, service, new BoxMetadataJsonConverter(), auth) { }


        /// <summary>
        /// Retrieves the metadata for the given file id
        /// </summary>
        /// <param name="id">ID of the file to retrieve metadata from</param>
        /// <param name="typeInstance">Name of the metadata type instance</param>
        /// <returns>A BoxMetadata object that includes key:value pairs defined by a user or application. 
        /// If there is no type instance present, a 404 HTTP status code of not_found will be returned.</returns>
        public async Task<BoxMetadata> GetMetadata(string id, string typeInstance = DefaultTypeInstance)
        {
            id.ThrowIfNullOrWhiteSpace("id");

            BoxRequest request = new BoxRequest(new Uri(Constants.BoxApiUriString + string.Format(CultureInfo.InvariantCulture, MetadataEndpointPath, id, typeInstance)));

            IBoxResponse<BoxMetadata> response = await ToResponseAsync<BoxMetadata>(request).ConfigureAwait(false);

            return response.ResponseObject;
        }

        /// <summary>
        /// Used to create the metadata type instance for a corresponding Box File Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="metadata"></param>
        /// <param name="typeInstance"></param>
        /// <returns>A BoxMetadata object that includes key:value pairs defined by a user or application. 
        /// If the properties instance already exists, a 409 HTTP status code of conflict will be returned and the update method should be used</returns>
        public async Task<BoxMetadata> CreateMetadata(string id, Dictionary<string,string> metadata, string typeInstance = DefaultTypeInstance)
        {
            id.ThrowIfNullOrWhiteSpace("id");

            BoxRequest request = new BoxRequest(new Uri(Constants.BoxApiUriString + string.Format(CultureInfo.InvariantCulture, MetadataEndpointPath, id, typeInstance)))
                .Method(RequestMethod.Post);

            request.Payload = _converter.Serialize(metadata);
            request.ContentType = JsonContentType;

            IBoxResponse<BoxMetadata> response = await ToResponseAsync<BoxMetadata>(request).ConfigureAwait(false);

            return response.ResponseObject;
        }

        /// <summary>
        /// Used to update the type instance. Updates can be either add, replace, remove , or test. 
        /// The type instance can only be updated if the type instance already exists.
        /// To use reserved characters like “/” and “~” defined by RFC 6092, please refer to section 4 of the API docs. 
        /// It demonstrates escaping “/” as “~1″ and “~” as “~0″.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="metadata"></param>
        /// <param name="typeInstance"></param>
        /// <returns></returns>
        public async Task<BoxMetadata> UpdateMetadata(string id, BoxMetadataRequest[] metadataRequests, string typeInstance = DefaultTypeInstance)
        {
            id.ThrowIfNullOrWhiteSpace("id");
            metadataRequests.ThrowIfNull("metadataRequest");

            BoxRequest request = new BoxRequest(new Uri(Constants.BoxApiUriString + string.Format(CultureInfo.InvariantCulture, MetadataEndpointPath, id, typeInstance)))
                .Method(RequestMethod.Put);

            request.Payload = _converter.Serialize(metadataRequests);
            request.ContentType = JsonPatchContentType;

            IBoxResponse<BoxMetadata> response = await ToResponseAsync<BoxMetadata>(request).ConfigureAwait(false);

            return response.ResponseObject;
        }

        /// <summary>
        /// Used to delete the type instance. To delete custom key:value pairs within a type instance, you should refer to the updating metadata section.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeInstance"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMetadata(string id, string typeInstance = DefaultTypeInstance)
        {
            id.ThrowIfNullOrWhiteSpace("id");

            BoxRequest request = new BoxRequest(new Uri(Constants.BoxApiUriString + string.Format(CultureInfo.InvariantCulture, MetadataEndpointPath, id, typeInstance)))
                .Method(RequestMethod.Delete);

            IBoxResponse<BoxMetadata> response = await ToResponseAsync<BoxMetadata>(request).ConfigureAwait(false);

            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Convenience method that will attempt to create metadata by first calling the Create endpoint, upon receiving a Conflict 409, it will automatically
        /// attempt to try the same request using the update endpoint
        /// </summary>
        /// <param name="id"></param>
        /// <param name="metadata"></param>
        /// <param name="typeInstance"></param>
        /// <returns></returns>
        public async Task<BoxMetadata> CreateOrUpdateMetadata(string id, Dictionary<string, string> metadata, string typeInstance = DefaultTypeInstance)
        {
            try
            {
                return await CreateMetadata(id, metadata, typeInstance);
            }
            catch (BoxException ex)
            {
                // A Conflict status is returned if the type instance has already been created, the SDK will attempt to retry the same request as an update request
                if (ex.StatusCode != System.Net.HttpStatusCode.Conflict)
                    throw;
            }

            var mdReq = metadata.Select(md => new BoxMetadataRequest { Path = md.Key, Op = BoxMetadataOperations.Add, Value = md.Value }).ToArray();
            return await UpdateMetadata(id, mdReq, typeInstance);
        }

    }
}
