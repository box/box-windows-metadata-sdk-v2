Box Windows Metadata V2 SDK 
==================


###Important Notice

Box's metadata functionality is in beta and is subject to frequent change.
Do not use it with production content as the data will be frequently purged.
By using the beta, you are acknowledging that you have read and agreed to our
[beta terms of service](https://cloud.box.com/s/w73uuums8jjaumtri853). If you
have questions, send an email to metadata-beta@box.com.


###Prerequisites
* Git  
* Visual Studio 2012 w/ Update 2 CTP  
* Box.V2 SDK (please allow nuget to automatically download missing packages)

###Overview
This is a plugin for [box-windows-sdk-v2](https://github.com/box/box-windows-sdk-v2) 
that enables beta support for Box's metadata API. See
[metadata API documentation](https://developers.box.com/metadata-api/) and
[product documentation](https://developers.box.com/metadata-web-application/).



**Note**: This library and the HTTP API it wraps are in beta and may undergo breaking
changes.

### Usage

Initialize the BoxConfig and BoxClient as you normally would. The addition of the BoxMetadataManager as a resource plugin, enables access to the metadata endpoint:

```c#
// Initialize the client with the MetadataManager plugin
var config = new BoxConfig(<Client_Id>, <Client_Secret>, "https://boxsdk");
var client = new BoxClient(config).AddResourcePlugin<BoxMetadataManager>();

// Initialize the properties instance type and add metadata 
Dictionary<string, string> mdReq = new Dictionary<string, string>() 
            {
                { "client_number","820183"}, 
                { "client_name", "Biomedical Corp"}, 
                { "case_reference", "A83JAA"}, 
                { "case_type", "Employment Litigation"}, 
                { "assigned_attorney", "Francis Burke" },
                { "case_status", "in-progress"}
            };
BoxMetadata md = await _client.ResourcePlugins.Get<BoxMetadataManager>().CreateMetadata("YOUR_FILE_ID", mdReq);

// Retrieve the newly created metadata
BoxMetadata md = await _client.ResourcePlugins.Get<BoxMetadataManager>().GetMetadata(TestFileId);

// Update the existing metadata
var mdReq = new BoxMetadataRequest[]
{
    new BoxMetadataRequest() { Op = BoxMetadataOperations.Test, Path="assigned_attorney", Value="Francis Burke"},
    new BoxMetadataRequest() { Op = BoxMetadataOperations.Replace, Path="assigned_attorney", Value="Eugene Huang"},
    new BoxMetadataRequest() { Op = BoxMetadataOperations.Test, Path="case_status", Value="in-progress"},
    new BoxMetadataRequest() { Op = BoxMetadataOperations.Remove, Path="case_status", Value="Francis Burke"},
    new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path="retention_length", Value="7_years"}
};

BoxMetadata md = await _client.ResourcePlugins.Get<BoxMetadataManager>().UpdateMetadata(TestFileId, mdReq);


// A convenience method is available for creating metadata that will first try the create endpoint, and if the type instance is already available, will automatically retry with the updates endpoint
BoxMetadata md = await _client.ResourcePlugins.Get<BoxMetadataManager>().CreateOrUpdateMetadata(TestFileId, mdReq);

```


### Tests

Tests can be run through the test explorer window in Visual Studio. Unit tests (tests under BoxMetadataManagerTest) can be run without additional setup. Integration tests (tests under BoxMetadataManagerTestIntegration) must have a valid ClientId/ClientSecret set, along with a valid Access Token to a metadata enabled account.
