using Azure.Storage.Sas;

namespace Api.Clients;

public interface IBlobClient
{
    Task UploadData(string fileName, Stream data, bool overwrite = false);
    Task UploadData(string fileName, byte[] data, bool overwrite = false);
    Task<byte[]> DownloadData(Uri uri);
    Uri GenerateSaSUri(string fileName, BlobContainerSasPermissions permission);
    Task DeleteBlob(string fileName);
    Task DeleteBlob(Uri uri);
}
