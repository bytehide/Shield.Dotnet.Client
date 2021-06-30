<a name='assembly'></a>
# Shield.Client

## Contents

- [DependenciesHelper](#T-Shield-Client-Helpers-DependenciesHelper 'Shield.Client.Helpers.DependenciesHelper')
  - [PackDependenciesToZip()](#M-Shield-Client-Helpers-DependenciesHelper-PackDependenciesToZip-System-Collections-Generic-List{System-String}- 'Shield.Client.Helpers.DependenciesHelper.PackDependenciesToZip(System.Collections.Generic.List{System.String})')
  - [PackDependenciesToZip()](#M-Shield-Client-Helpers-DependenciesHelper-PackDependenciesToZip-System-Collections-Generic-List{System-ValueTuple{System-Byte[],System-String}}- 'Shield.Client.Helpers.DependenciesHelper.PackDependenciesToZip(System.Collections.Generic.List{System.ValueTuple{System.Byte[],System.String}})')
- [FileExtensions](#T-Shield-Client-Extensions-FileExtensions 'Shield.Client.Extensions.FileExtensions')
  - [SaveOn(downloadedApplication,path,replaceIfExist)](#M-Shield-Client-Extensions-FileExtensions-SaveOn-System-IO-Stream,System-String,System-Boolean- 'Shield.Client.Extensions.FileExtensions.SaveOn(System.IO.Stream,System.String,System.Boolean)')
  - [SaveOn(downloadedApplication,path,replaceIfExist)](#M-Shield-Client-Extensions-FileExtensions-SaveOn-System-Byte[],System-String,System-Boolean- 'Shield.Client.Extensions.FileExtensions.SaveOn(System.Byte[],System.String,System.Boolean)')
  - [SaveOnAsync(downloadedApplication,path,replaceIfExist)](#M-Shield-Client-Extensions-FileExtensions-SaveOnAsync-System-IO-Stream,System-String,System-Boolean- 'Shield.Client.Extensions.FileExtensions.SaveOnAsync(System.IO.Stream,System.String,System.Boolean)')
  - [SaveOnAsync(downloadedApplication,path,replaceIfExist)](#M-Shield-Client-Extensions-FileExtensions-SaveOnAsync-System-Byte[],System-String,System-Boolean- 'Shield.Client.Extensions.FileExtensions.SaveOnAsync(System.Byte[],System.String,System.Boolean)')
- [MimeTypeMap](#T-Shield-Client-Helpers-MimeTypeMap 'Shield.Client.Helpers.MimeTypeMap')
  - [GetExtension(mimeType,throwErrorIfNotFound)](#M-Shield-Client-Helpers-MimeTypeMap-GetExtension-System-String,System-Boolean- 'Shield.Client.Helpers.MimeTypeMap.GetExtension(System.String,System.Boolean)')
  - [GetMimeType(str)](#M-Shield-Client-Helpers-MimeTypeMap-GetMimeType-System-String- 'Shield.Client.Helpers.MimeTypeMap.GetMimeType(System.String)')
  - [TryGetMimeType(str,mimeType)](#M-Shield-Client-Helpers-MimeTypeMap-TryGetMimeType-System-String,System-String@- 'Shield.Client.Helpers.MimeTypeMap.TryGetMimeType(System.String,System.String@)')
- [ServerSentEvents](#T-Shield-Client-Extensions-ServerSentEvents 'Shield.Client.Extensions.ServerSentEvents')
  - [#ctor(authToken,version,baseUrl)](#M-Shield-Client-Extensions-ServerSentEvents-#ctor-System-String,System-String,System-String- 'Shield.Client.Extensions.ServerSentEvents.#ctor(System.String,System.String,System.String)')
  - [OnClosed](#P-Shield-Client-Extensions-ServerSentEvents-OnClosed 'Shield.Client.Extensions.ServerSentEvents.OnClosed')
  - [OnConnected](#P-Shield-Client-Extensions-ServerSentEvents-OnConnected 'Shield.Client.Extensions.ServerSentEvents.OnConnected')
  - [Destroy(method)](#M-Shield-Client-Extensions-ServerSentEvents-Destroy-System-String- 'Shield.Client.Extensions.ServerSentEvents.Destroy(System.String)')
  - [On(method,action)](#M-Shield-Client-Extensions-ServerSentEvents-On-System-String,System-Action{System-String,System-String,System-DateTime}- 'Shield.Client.Extensions.ServerSentEvents.On(System.String,System.Action{System.String,System.String,System.DateTime})')
  - [ProtectSingleFile()](#M-Shield-Client-Extensions-ServerSentEvents-ProtectSingleFile-System-String,System-String,Shield-Client-Models-API-Application-ApplicationConfigurationDto- 'Shield.Client.Extensions.ServerSentEvents.ProtectSingleFile(System.String,System.String,Shield.Client.Models.API.Application.ApplicationConfigurationDto)')
  - [ProtectSingleFileAsync(projectKey,fileBlob,configuration)](#M-Shield-Client-Extensions-ServerSentEvents-ProtectSingleFileAsync-System-String,System-String,Shield-Client-Models-API-Application-ApplicationConfigurationDto- 'Shield.Client.Extensions.ServerSentEvents.ProtectSingleFileAsync(System.String,System.String,Shield.Client.Models.API.Application.ApplicationConfigurationDto)')
  - [SetDefaultLogger(action)](#M-Shield-Client-Extensions-ServerSentEvents-SetDefaultLogger-System-Action{System-String,System-String,System-DateTime}- 'Shield.Client.Extensions.ServerSentEvents.SetDefaultLogger(System.Action{System.String,System.String,System.DateTime})')
  - [Stop()](#M-Shield-Client-Extensions-ServerSentEvents-Stop 'Shield.Client.Extensions.ServerSentEvents.Stop')
- [ShieldApplication](#T-Shield-Client-ShieldApplication 'Shield.Client.ShieldApplication')
  - [DownloadApplication(downloadKey,format)](#M-Shield-Client-ShieldApplication-DownloadApplication-System-String,Shield-Client-Models-DownloadFormat- 'Shield.Client.ShieldApplication.DownloadApplication(System.String,Shield.Client.Models.DownloadFormat)')
  - [DownloadApplicationAsync(downloadKey,format)](#M-Shield-Client-ShieldApplication-DownloadApplicationAsync-System-String,Shield-Client-Models-DownloadFormat- 'Shield.Client.ShieldApplication.DownloadApplicationAsync(System.String,Shield.Client.Models.DownloadFormat)')
  - [UploadApplicationDirectly(file,dependencies,projectKey)](#M-Shield-Client-ShieldApplication-UploadApplicationDirectly-System-String,Shield-Client-Models-ShieldFile,System-Collections-Generic-List{Shield-Client-Models-ShieldFile}- 'Shield.Client.ShieldApplication.UploadApplicationDirectly(System.String,Shield.Client.Models.ShieldFile,System.Collections.Generic.List{Shield.Client.Models.ShieldFile})')
  - [UploadApplicationDirectly(filePath,dependenciesPaths,projectKey)](#M-Shield-Client-ShieldApplication-UploadApplicationDirectly-System-String,System-String,System-Collections-Generic-List{System-String}- 'Shield.Client.ShieldApplication.UploadApplicationDirectly(System.String,System.String,System.Collections.Generic.List{System.String})')
  - [UploadApplicationDirectlyAsync(file,dependencies,projectKey)](#M-Shield-Client-ShieldApplication-UploadApplicationDirectlyAsync-System-String,Shield-Client-Models-ShieldFile,System-Collections-Generic-List{Shield-Client-Models-ShieldFile}- 'Shield.Client.ShieldApplication.UploadApplicationDirectlyAsync(System.String,Shield.Client.Models.ShieldFile,System.Collections.Generic.List{Shield.Client.Models.ShieldFile})')
  - [UploadApplicationDirectlyAsync(filePath,dependenciesPaths,projectKey)](#M-Shield-Client-ShieldApplication-UploadApplicationDirectlyAsync-System-String,System-String,System-Collections-Generic-List{System-String}- 'Shield.Client.ShieldApplication.UploadApplicationDirectlyAsync(System.String,System.String,System.Collections.Generic.List{System.String})')
- [ShieldClient](#T-Shield-Client-ShieldClient 'Shield.Client.ShieldClient')
  - [CheckConnection(code)](#M-Shield-Client-ShieldClient-CheckConnection-System-Net-HttpStatusCode@- 'Shield.Client.ShieldClient.CheckConnection(System.Net.HttpStatusCode@)')
- [ShieldConnector](#T-Shield-Client-ShieldConnector 'Shield.Client.ShieldConnector')
  - [InstanceAndStartConnector(externalConnection,started)](#M-Shield-Client-ShieldConnector-InstanceAndStartConnector-Shield-Client-Models-HubConnectionExternalModel,Shield-Client-StartedConnection@- 'Shield.Client.ShieldConnector.InstanceAndStartConnector(Shield.Client.Models.HubConnectionExternalModel,Shield.Client.StartedConnection@)')
  - [InstanceAndStartHubConnectorWithLogger(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceAndStartHubConnectorWithLogger-Shield-Client-Models-HubConnectionExternalModel,Shield-Client-StartedConnection@- 'Shield.Client.ShieldConnector.InstanceAndStartHubConnectorWithLogger(Shield.Client.Models.HubConnectionExternalModel,Shield.Client.StartedConnection@)')
  - [InstanceHubConnector(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceHubConnector-Shield-Client-Models-HubConnectionExternalModel- 'Shield.Client.ShieldConnector.InstanceHubConnector(Shield.Client.Models.HubConnectionExternalModel)')
  - [InstanceHubConnector(externalConnection,withLogger)](#M-Shield-Client-ShieldConnector-InstanceHubConnector-Shield-Client-Models-HubConnectionExternalModel,System-Boolean- 'Shield.Client.ShieldConnector.InstanceHubConnector(Shield.Client.Models.HubConnectionExternalModel,System.Boolean)')
  - [InstanceHubConnectorAsync(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceHubConnectorAsync-Shield-Client-Models-HubConnectionExternalModel- 'Shield.Client.ShieldConnector.InstanceHubConnectorAsync(Shield.Client.Models.HubConnectionExternalModel)')
  - [InstanceHubConnectorAsync(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceHubConnectorAsync-Shield-Client-Models-HubConnectionExternalModel,System-Boolean- 'Shield.Client.ShieldConnector.InstanceHubConnectorAsync(Shield.Client.Models.HubConnectionExternalModel,System.Boolean)')
  - [InstanceHubConnectorWithLogger(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceHubConnectorWithLogger-Shield-Client-Models-HubConnectionExternalModel- 'Shield.Client.ShieldConnector.InstanceHubConnectorWithLogger(Shield.Client.Models.HubConnectionExternalModel)')
  - [InstanceHubConnectorWithLoggerAsync(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceHubConnectorWithLoggerAsync-Shield-Client-Models-HubConnectionExternalModel- 'Shield.Client.ShieldConnector.InstanceHubConnectorWithLoggerAsync(Shield.Client.Models.HubConnectionExternalModel)')
  - [InstanceQueueConnector(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceQueueConnector-Shield-Client-Models-QueueConnectionExternalModel- 'Shield.Client.ShieldConnector.InstanceQueueConnector(Shield.Client.Models.QueueConnectionExternalModel)')
  - [InstanceQueueConnector(externalConnection,withLogger)](#M-Shield-Client-ShieldConnector-InstanceQueueConnector-Shield-Client-Models-QueueConnectionExternalModel,System-Boolean- 'Shield.Client.ShieldConnector.InstanceQueueConnector(Shield.Client.Models.QueueConnectionExternalModel,System.Boolean)')
  - [InstanceQueueConnectorWithLogger(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceQueueConnectorWithLogger-Shield-Client-Models-QueueConnectionExternalModel- 'Shield.Client.ShieldConnector.InstanceQueueConnectorWithLogger(Shield.Client.Models.QueueConnectionExternalModel)')
  - [InstanceSseConnector(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceSseConnector 'Shield.Client.ShieldConnector.InstanceSseConnector')
  - [InstanceSseConnector(externalConnection,withLogger)](#M-Shield-Client-ShieldConnector-InstanceSseConnector-System-Boolean- 'Shield.Client.ShieldConnector.InstanceSseConnector(System.Boolean)')
  - [InstanceSseConnectorWithLogger(externalConnection)](#M-Shield-Client-ShieldConnector-InstanceSseConnectorWithLogger 'Shield.Client.ShieldConnector.InstanceSseConnectorWithLogger')
- [ShieldFile](#T-Shield-Client-Models-ShieldFile 'Shield.Client.Models.ShieldFile')
- [ShieldProject](#T-Shield-Client-ShieldProject 'Shield.Client.ShieldProject')
  - [FindByIdOrCreateExternalProject(projectName,projectKey)](#M-Shield-Client-ShieldProject-FindByIdOrCreateExternalProject-System-String,System-String- 'Shield.Client.ShieldProject.FindByIdOrCreateExternalProject(System.String,System.String)')
  - [FindByIdOrCreateExternalProjectAsync(projectName,projectKey)](#M-Shield-Client-ShieldProject-FindByIdOrCreateExternalProjectAsync-System-String,System-String- 'Shield.Client.ShieldProject.FindByIdOrCreateExternalProjectAsync(System.String,System.String)')
  - [FindOrCreateExternalProject(projectName)](#M-Shield-Client-ShieldProject-FindOrCreateExternalProject-System-String- 'Shield.Client.ShieldProject.FindOrCreateExternalProject(System.String)')
  - [FindOrCreateExternalProjectAsync(projectName)](#M-Shield-Client-ShieldProject-FindOrCreateExternalProjectAsync-System-String- 'Shield.Client.ShieldProject.FindOrCreateExternalProjectAsync(System.String)')
- [ShieldProtections](#T-Shield-Client-ShieldProtections 'Shield.Client.ShieldProtections')
  - [GetProtections(projectKey)](#M-Shield-Client-ShieldProtections-GetProtections-System-String- 'Shield.Client.ShieldProtections.GetProtections(System.String)')
  - [GetProtectionsAsync(projectKey)](#M-Shield-Client-ShieldProtections-GetProtectionsAsync-System-String- 'Shield.Client.ShieldProtections.GetProtectionsAsync(System.String)')
- [ShieldTasks](#T-Shield-Client-ShieldTasks 'Shield.Client.ShieldTasks')
- [StartedConnection](#T-Shield-Client-StartedConnection 'Shield.Client.StartedConnection')

<a name='T-Shield-Client-Helpers-DependenciesHelper'></a>
## DependenciesHelper `type`

##### Namespace

Shield.Client.Helpers

<a name='M-Shield-Client-Helpers-DependenciesHelper-PackDependenciesToZip-System-Collections-Generic-List{System-String}-'></a>
### PackDependenciesToZip() `method`

##### Summary

Convert to zip a list of path files

##### Returns



##### Parameters

This method has no parameters.

<a name='M-Shield-Client-Helpers-DependenciesHelper-PackDependenciesToZip-System-Collections-Generic-List{System-ValueTuple{System-Byte[],System-String}}-'></a>
### PackDependenciesToZip() `method`

##### Summary

Convert to zip a list of files

##### Returns



##### Parameters

This method has no parameters.

<a name='T-Shield-Client-Extensions-FileExtensions'></a>
## FileExtensions `type`

##### Namespace

Shield.Client.Extensions

<a name='M-Shield-Client-Extensions-FileExtensions-SaveOn-System-IO-Stream,System-String,System-Boolean-'></a>
### SaveOn(downloadedApplication,path,replaceIfExist) `method`

##### Summary

Save downloaded application as stream to path.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| downloadedApplication | [System.IO.Stream](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IO.Stream 'System.IO.Stream') |  |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| replaceIfExist | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Replace if the path contains an existing file. |

<a name='M-Shield-Client-Extensions-FileExtensions-SaveOn-System-Byte[],System-String,System-Boolean-'></a>
### SaveOn(downloadedApplication,path,replaceIfExist) `method`

##### Summary

Save downloaded application as array to path.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| downloadedApplication | [System.Byte[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte[] 'System.Byte[]') |  |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| replaceIfExist | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Replace if the path contains an existing file. |

<a name='M-Shield-Client-Extensions-FileExtensions-SaveOnAsync-System-IO-Stream,System-String,System-Boolean-'></a>
### SaveOnAsync(downloadedApplication,path,replaceIfExist) `method`

##### Summary

Save downloaded application as stream to path.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| downloadedApplication | [System.IO.Stream](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IO.Stream 'System.IO.Stream') |  |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| replaceIfExist | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Replace if the path contains an existing file. |

<a name='M-Shield-Client-Extensions-FileExtensions-SaveOnAsync-System-Byte[],System-String,System-Boolean-'></a>
### SaveOnAsync(downloadedApplication,path,replaceIfExist) `method`

##### Summary

Save downloaded application as array to path.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| downloadedApplication | [System.Byte[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte[] 'System.Byte[]') |  |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| replaceIfExist | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Replace if the path contains an existing file. |

<a name='T-Shield-Client-Helpers-MimeTypeMap'></a>
## MimeTypeMap `type`

##### Namespace

Shield.Client.Helpers

<a name='M-Shield-Client-Helpers-MimeTypeMap-GetExtension-System-String,System-Boolean-'></a>
### GetExtension(mimeType,throwErrorIfNotFound) `method`

##### Summary

Gets the extension from the provided MINE type.

##### Returns

The extension.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| mimeType | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Type of the MIME. |
| throwErrorIfNotFound | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | if set to `true`, throws error if extension's not found. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') |  |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') |  |

<a name='M-Shield-Client-Helpers-MimeTypeMap-GetMimeType-System-String-'></a>
### GetMimeType(str) `method`

##### Summary

Gets the type of the MIME from the provided string.

##### Returns

The MIME type.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| str | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The filename or extension. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') |  |

<a name='M-Shield-Client-Helpers-MimeTypeMap-TryGetMimeType-System-String,System-String@-'></a>
### TryGetMimeType(str,mimeType) `method`

##### Summary

Tries to get the type of the MIME from the provided string.

##### Returns

The MIME type.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| str | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The filename or extension. |
| mimeType | [System.String@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String@ 'System.String@') | The variable to store the MIME type. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') |  |

<a name='T-Shield-Client-Extensions-ServerSentEvents'></a>
## ServerSentEvents `type`

##### Namespace

Shield.Client.Extensions

##### Summary



<a name='M-Shield-Client-Extensions-ServerSentEvents-#ctor-System-String,System-String,System-String-'></a>
### #ctor(authToken,version,baseUrl) `constructor`

##### Summary



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| authToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| version | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| baseUrl | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |

<a name='P-Shield-Client-Extensions-ServerSentEvents-OnClosed'></a>
### OnClosed `property`

##### Summary



<a name='P-Shield-Client-Extensions-ServerSentEvents-OnConnected'></a>
### OnConnected `property`

##### Summary



<a name='M-Shield-Client-Extensions-ServerSentEvents-Destroy-System-String-'></a>
### Destroy(method) `method`

##### Summary



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| method | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |

<a name='M-Shield-Client-Extensions-ServerSentEvents-On-System-String,System-Action{System-String,System-String,System-DateTime}-'></a>
### On(method,action) `method`

##### Summary



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| method | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| action | [System.Action{System.String,System.String,System.DateTime}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{System.String,System.String,System.DateTime}') |  |

<a name='M-Shield-Client-Extensions-ServerSentEvents-ProtectSingleFile-System-String,System-String,Shield-Client-Models-API-Application-ApplicationConfigurationDto-'></a>
### ProtectSingleFile() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='M-Shield-Client-Extensions-ServerSentEvents-ProtectSingleFileAsync-System-String,System-String,Shield-Client-Models-API-Application-ApplicationConfigurationDto-'></a>
### ProtectSingleFileAsync(projectKey,fileBlob,configuration) `method`

##### Summary



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| projectKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| fileBlob | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| configuration | [Shield.Client.Models.API.Application.ApplicationConfigurationDto](#T-Shield-Client-Models-API-Application-ApplicationConfigurationDto 'Shield.Client.Models.API.Application.ApplicationConfigurationDto') |  |

<a name='M-Shield-Client-Extensions-ServerSentEvents-SetDefaultLogger-System-Action{System-String,System-String,System-DateTime}-'></a>
### SetDefaultLogger(action) `method`

##### Summary



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| action | [System.Action{System.String,System.String,System.DateTime}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{System.String,System.String,System.DateTime}') |  |

<a name='M-Shield-Client-Extensions-ServerSentEvents-Stop'></a>
### Stop() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='T-Shield-Client-ShieldApplication'></a>
## ShieldApplication `type`

##### Namespace

Shield.Client

<a name='M-Shield-Client-ShieldApplication-DownloadApplication-System-String,Shield-Client-Models-DownloadFormat-'></a>
### DownloadApplication(downloadKey,format) `method`

##### Summary

Download protected application from project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| downloadKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| format | [Shield.Client.Models.DownloadFormat](#T-Shield-Client-Models-DownloadFormat 'Shield.Client.Models.DownloadFormat') |  |

<a name='M-Shield-Client-ShieldApplication-DownloadApplicationAsync-System-String,Shield-Client-Models-DownloadFormat-'></a>
### DownloadApplicationAsync(downloadKey,format) `method`

##### Summary

Download protected application from project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| downloadKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| format | [Shield.Client.Models.DownloadFormat](#T-Shield-Client-Models-DownloadFormat 'Shield.Client.Models.DownloadFormat') |  |

<a name='M-Shield-Client-ShieldApplication-UploadApplicationDirectly-System-String,Shield-Client-Models-ShieldFile,System-Collections-Generic-List{Shield-Client-Models-ShieldFile}-'></a>
### UploadApplicationDirectly(file,dependencies,projectKey) `method`

##### Summary

Upload an application to project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| file | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | File to upload |
| dependencies | [Shield.Client.Models.ShieldFile](#T-Shield-Client-Models-ShieldFile 'Shield.Client.Models.ShieldFile') | Required dependencies list |
| projectKey | [System.Collections.Generic.List{Shield.Client.Models.ShieldFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{Shield.Client.Models.ShieldFile}') | Project key (where application will be uploaded) |

<a name='M-Shield-Client-ShieldApplication-UploadApplicationDirectly-System-String,System-String,System-Collections-Generic-List{System-String}-'></a>
### UploadApplicationDirectly(filePath,dependenciesPaths,projectKey) `method`

##### Summary

Upload an application to project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| filePath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Path of file to upload |
| dependenciesPaths | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Required dependencies path list |
| projectKey | [System.Collections.Generic.List{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{System.String}') | Project key (where application will be uploaded) |

<a name='M-Shield-Client-ShieldApplication-UploadApplicationDirectlyAsync-System-String,Shield-Client-Models-ShieldFile,System-Collections-Generic-List{Shield-Client-Models-ShieldFile}-'></a>
### UploadApplicationDirectlyAsync(file,dependencies,projectKey) `method`

##### Summary

Upload an application to project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| file | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | File to upload |
| dependencies | [Shield.Client.Models.ShieldFile](#T-Shield-Client-Models-ShieldFile 'Shield.Client.Models.ShieldFile') | Required dependencies list |
| projectKey | [System.Collections.Generic.List{Shield.Client.Models.ShieldFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{Shield.Client.Models.ShieldFile}') | Project key (where application will be uploaded) |

<a name='M-Shield-Client-ShieldApplication-UploadApplicationDirectlyAsync-System-String,System-String,System-Collections-Generic-List{System-String}-'></a>
### UploadApplicationDirectlyAsync(filePath,dependenciesPaths,projectKey) `method`

##### Summary

Upload an application to project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| filePath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Path of file to upload |
| dependenciesPaths | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Required dependencies path list |
| projectKey | [System.Collections.Generic.List{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{System.String}') | Project key (where application will be uploaded) |

<a name='T-Shield-Client-ShieldClient'></a>
## ShieldClient `type`

##### Namespace

Shield.Client

<a name='M-Shield-Client-ShieldClient-CheckConnection-System-Net-HttpStatusCode@-'></a>
### CheckConnection(code) `method`

##### Summary

Check if current client has connection to the API.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| code | [System.Net.HttpStatusCode@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.HttpStatusCode@ 'System.Net.HttpStatusCode@') | Returned connection status code. |

<a name='T-Shield-Client-ShieldConnector'></a>
## ShieldConnector `type`

##### Namespace

Shield.Client

<a name='M-Shield-Client-ShieldConnector-InstanceAndStartConnector-Shield-Client-Models-HubConnectionExternalModel,Shield-Client-StartedConnection@-'></a>
### InstanceAndStartConnector(externalConnection,started) `method`

##### Summary

Build a Started Connection with the hub of the current process to handle the service from .NET Framework applications.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.HubConnectionExternalModel](#T-Shield-Client-Models-HubConnectionExternalModel 'Shield.Client.Models.HubConnectionExternalModel') |  |
| started | [Shield.Client.StartedConnection@](#T-Shield-Client-StartedConnection@ 'Shield.Client.StartedConnection@') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceAndStartHubConnectorWithLogger-Shield-Client-Models-HubConnectionExternalModel,Shield-Client-StartedConnection@-'></a>
### InstanceAndStartHubConnectorWithLogger(externalConnection) `method`

##### Summary

Build a Started Connection with the hub of the current process to handle the service from .NET Framework applications with current logger.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.HubConnectionExternalModel](#T-Shield-Client-Models-HubConnectionExternalModel 'Shield.Client.Models.HubConnectionExternalModel') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceHubConnector-Shield-Client-Models-HubConnectionExternalModel-'></a>
### InstanceHubConnector(externalConnection) `method`

##### Summary

Build a connection to the shield hub for the current protection process.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.HubConnectionExternalModel](#T-Shield-Client-Models-HubConnectionExternalModel 'Shield.Client.Models.HubConnectionExternalModel') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceHubConnector-Shield-Client-Models-HubConnectionExternalModel,System-Boolean-'></a>
### InstanceHubConnector(externalConnection,withLogger) `method`

##### Summary

Build a connection to the shield hub for the current protection process.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.HubConnectionExternalModel](#T-Shield-Client-Models-HubConnectionExternalModel 'Shield.Client.Models.HubConnectionExternalModel') |  |
| withLogger | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceHubConnectorAsync-Shield-Client-Models-HubConnectionExternalModel-'></a>
### InstanceHubConnectorAsync(externalConnection) `method`

##### Summary

Build a connection to the shield hub for the current protection process async.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.HubConnectionExternalModel](#T-Shield-Client-Models-HubConnectionExternalModel 'Shield.Client.Models.HubConnectionExternalModel') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceHubConnectorAsync-Shield-Client-Models-HubConnectionExternalModel,System-Boolean-'></a>
### InstanceHubConnectorAsync(externalConnection) `method`

##### Summary

Build a Started Connection with the hub of the current process to handle the service from .NET Framework applications with current logger. //TODO: DOC!!!

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.HubConnectionExternalModel](#T-Shield-Client-Models-HubConnectionExternalModel 'Shield.Client.Models.HubConnectionExternalModel') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceHubConnectorWithLogger-Shield-Client-Models-HubConnectionExternalModel-'></a>
### InstanceHubConnectorWithLogger(externalConnection) `method`

##### Summary

Build a connection to the shield hub for the current protection process with current logger.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.HubConnectionExternalModel](#T-Shield-Client-Models-HubConnectionExternalModel 'Shield.Client.Models.HubConnectionExternalModel') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceHubConnectorWithLoggerAsync-Shield-Client-Models-HubConnectionExternalModel-'></a>
### InstanceHubConnectorWithLoggerAsync(externalConnection) `method`

##### Summary

Build a connection to the shield hub for the current protection process with current logger async.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.HubConnectionExternalModel](#T-Shield-Client-Models-HubConnectionExternalModel 'Shield.Client.Models.HubConnectionExternalModel') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceQueueConnector-Shield-Client-Models-QueueConnectionExternalModel-'></a>
### InstanceQueueConnector(externalConnection) `method`

##### Summary

Build a connection to the shield bus for the current protection process.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.QueueConnectionExternalModel](#T-Shield-Client-Models-QueueConnectionExternalModel 'Shield.Client.Models.QueueConnectionExternalModel') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceQueueConnector-Shield-Client-Models-QueueConnectionExternalModel,System-Boolean-'></a>
### InstanceQueueConnector(externalConnection,withLogger) `method`

##### Summary

Build a connection to the shield queues service bus for the current protection process.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.QueueConnectionExternalModel](#T-Shield-Client-Models-QueueConnectionExternalModel 'Shield.Client.Models.QueueConnectionExternalModel') |  |
| withLogger | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceQueueConnectorWithLogger-Shield-Client-Models-QueueConnectionExternalModel-'></a>
### InstanceQueueConnectorWithLogger(externalConnection) `method`

##### Summary

Build a connection to the shield bus for the current protection process with current logger.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [Shield.Client.Models.QueueConnectionExternalModel](#T-Shield-Client-Models-QueueConnectionExternalModel 'Shield.Client.Models.QueueConnectionExternalModel') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceSseConnector'></a>
### InstanceSseConnector(externalConnection) `method`

##### Summary

Build a connection to the shield bus for the current protection process. //TODO: DOC!!

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [M:Shield.Client.ShieldConnector.InstanceSseConnector](#T-M-Shield-Client-ShieldConnector-InstanceSseConnector 'M:Shield.Client.ShieldConnector.InstanceSseConnector') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceSseConnector-System-Boolean-'></a>
### InstanceSseConnector(externalConnection,withLogger) `method`

##### Summary



##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-Shield-Client-ShieldConnector-InstanceSseConnectorWithLogger'></a>
### InstanceSseConnectorWithLogger(externalConnection) `method`

##### Summary

Build a connection to the shield bus for the current protection process with current logger. //TODO: DOC!!

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| externalConnection | [M:Shield.Client.ShieldConnector.InstanceSseConnectorWithLogger](#T-M-Shield-Client-ShieldConnector-InstanceSseConnectorWithLogger 'M:Shield.Client.ShieldConnector.InstanceSseConnectorWithLogger') |  |

<a name='T-Shield-Client-Models-ShieldFile'></a>
## ShieldFile `type`

##### Namespace

Shield.Client.Models

##### Summary

Work-Standard model of custom file for shield.

<a name='T-Shield-Client-ShieldProject'></a>
## ShieldProject `type`

##### Namespace

Shield.Client

<a name='M-Shield-Client-ShieldProject-FindByIdOrCreateExternalProject-System-String,System-String-'></a>
### FindByIdOrCreateExternalProject(projectName,projectKey) `method`

##### Summary

Find by Id or creates a Shield project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| projectName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Project name, used only when creates new project. |
| projectKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Project id to find |

<a name='M-Shield-Client-ShieldProject-FindByIdOrCreateExternalProjectAsync-System-String,System-String-'></a>
### FindByIdOrCreateExternalProjectAsync(projectName,projectKey) `method`

##### Summary

Find by Id or creates a Shield project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| projectName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Project name, used only when creates new project. |
| projectKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Project id to find |

<a name='M-Shield-Client-ShieldProject-FindOrCreateExternalProject-System-String-'></a>
### FindOrCreateExternalProject(projectName) `method`

##### Summary

Find or creates a Shield project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| projectName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Project name, used for find or as new project name |

<a name='M-Shield-Client-ShieldProject-FindOrCreateExternalProjectAsync-System-String-'></a>
### FindOrCreateExternalProjectAsync(projectName) `method`

##### Summary

Find or creates a Shield project

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| projectName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Project name, used for find or as new project name |

<a name='T-Shield-Client-ShieldProtections'></a>
## ShieldProtections `type`

##### Namespace

Shield.Client

<a name='M-Shield-Client-ShieldProtections-GetProtections-System-String-'></a>
### GetProtections(projectKey) `method`

##### Summary

Gets available protections of a project with his key.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| projectKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Project key |

<a name='M-Shield-Client-ShieldProtections-GetProtectionsAsync-System-String-'></a>
### GetProtectionsAsync(projectKey) `method`

##### Summary

Gets available protections of a project with his key async.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| projectKey | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Project key |

<a name='T-Shield-Client-ShieldTasks'></a>
## ShieldTasks `type`

##### Namespace

Shield.Client

##### Summary



<a name='T-Shield-Client-StartedConnection'></a>
## StartedConnection `type`

##### Namespace

Shield.Client

##### Summary

This class is an adaptation of a SignalR HubConnection to handle Shield processes from .net framework applications in a comfortable way.
