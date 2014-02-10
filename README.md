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
* Box.V2 SDK (referenced in library)

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

// Retrieve the MetadataManager through the resource plugin manager
BoxMetadata getMD = await _client.ResourcePlugins.Get<BoxMetadataManager>().GetMetadata("YOUR_FILE_ID");

// Create a BoxMetadataRequest object to add, edit, test, or remove metadata
var req = new BoxMetadataRequest[] 
{ 
    new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path = "/key", Value="value"},
    new BoxMetadataRequest() { Op = BoxMetadataOperations.Add, Path = "/key2", Value="value2"}
};
BoxMetadata editMD = await _client.ResourcePlugins.Get<BoxMetadataManager>().EditMetadata(_fileId, req);

```


### Tests

Tests can be run through the test explorer window in Visual Studio. Unit tests (tests under BoxMetadataManagerTest) can be run without additional setup. Integration tests (tests under BoxMetadataManagerTestIntegration) must have a valid ClientId/ClientSecret set, along with a valid Access Token to a metadata enabled account.
