# Storage-Account-File-Serving
# What

Simple API, meant to be deployed on an Azure Function, that streams files from storage accounts using AD authentication.

## Parameters

accountname : Name of the storage account from which to download a file

filepath : File path of a blob, containing the container name
- e.g. container/folder/blobname.extension

# Why

Strangely, Azure doesn't allow downloading from a storage account using AD authentication. It only allows for SAS authentication or public access.
This API therefore manages the stream between the storage account and the user using a simple GET request. This is ideal for business users or to embed a URL into a webpage.

# TODO

I only implmented the happy path. Error management needs to be implemented.
