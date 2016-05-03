// Michael O'Keefe
// Cloud Computing Project 1 Checkpoint 2 - Extra Credit 1
// Due: 10/29
//
// LogReader is an application that connects to the storage account mgokstorage
// and connects to the container mgok-consumerlogs and gets reference to all 
// three blob log files (assuming everything already exists). Whatever is contained 
// in the blob files is sent to the path 'C:\blobs\' and named accordingly (saved in 
// the form of a txt file.
//
// Assumptions: 
// - storage, container, and blob files are already existing (and named correctly)
// - path 'C:\blobs\' exists on machine the application is run on

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LogReader
{
    class Program
    {
        static void Main(string[] args)
        {
            // retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mgok-consumerlogs");

            // retrieve reference to all three blobs 
            CloudAppendBlob appendBlob1 = container.GetAppendBlobReference("MGOKVM1blob.txt");
           
            CloudAppendBlob appendBlob2 = container.GetAppendBlobReference("MGOKVM2blob.txt");

            CloudAppendBlob appendBlob3 = container.GetAppendBlobReference("MGOKVM3blob.txt");

            // save all blob contents to 3 different files.
            using (var fileStream = System.IO.File.OpenWrite(@"C:\blobs\vm1blob.txt"))
            {
                appendBlob1.DownloadToStream(fileStream);
            }

            using (var fileStream = System.IO.File.OpenWrite(@"C:\blobs\vm2blob.txt"))
            {
                appendBlob2.DownloadToStream(fileStream);
            }

            using (var fileStream = System.IO.File.OpenWrite(@"C:\blobs\vm3blob.txt"))
            {
                appendBlob3.DownloadToStream(fileStream);
            }
            
        }
    }
}
