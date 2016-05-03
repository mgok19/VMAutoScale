// Michael O'Keefe
// Cloud Computing, Project 1 - Checkpoint 2
// Due: 10/29/15
// 
// Consumer is a console application that reads one message every three seconds
// (if a message is available) from the queue 'michaelq' located within Microsoft Azure. 
// When a message is recieved, the consumer will append of the message to a blob created 
// specifically for the machine that the consumer is running on. 
// 
// Assumtions: 
// - The queue 'michaelq' has already been created and exists (yet if it did not exist
//   it would not be the consumers job to create it)
// - If a blob has not been created for the machine the consumer is running on, one
//   will be created
// - If a blob already exists for the machine the consumer is running on, then messages
//   will be appended to the already existing blob

using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace Consumer_CP2
{
    class Program
    {
        static void Main(string[] args)
        {
            // store the start time for logging
            String startTime = DateTime.Now.ToString("h:mm:ss tt");

            // store the vmName for logging purposes 
            String vmName = System.Environment.MachineName;

            // create client queue assuming michaelq exists 
            string connectionString =
                CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            QueueClient client =
                QueueClient.CreateFromConnectionString(connectionString, "michaelq");

            // retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // retrieve a reference to a container
            CloudBlobContainer container = blobClient.GetContainerReference("mgok-consumerlogs");

            // create the container if it doesn't already exist
            container.CreateIfNotExists();

            // make container public so blob can be accessed
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                BlobContainerPublicAccessType.Blob
                });

            // create a reference to the append blob
            CloudAppendBlob appendBlob = container.GetAppendBlobReference(vmName + "blob.txt");

            // create the referenced append blob if it does not already exist
            if (!appendBlob.Exists())
            {
                appendBlob.CreateOrReplace();
            }

            // log the start time of the conumser app
            appendBlob.AppendText("*****************************************************" + Environment.NewLine);
            appendBlob.AppendText("     " + vmName +  " started at " + startTime + "     " + Environment.NewLine);
            appendBlob.AppendText("*****************************************************" + Environment.NewLine);

            while (true)  //infinite loop
            {
                // gets message from the queue 
                // (also takes care of the case where there are no messages)
                BrokeredMessage message = client.Receive();

                // checks if there was a message to receive
                if (message != null)
                {
                    
                    // store the time the message was received for logging
                    String timeReceived = DateTime.Now.ToString("h:mm:ss tt");

                    // log the message body and the time it was received
                    appendBlob.AppendText(vmName + " Received: " + message.GetBody<string>() + " at " 
                        + timeReceived + "" + Environment.NewLine);

                    // discards the message that was read
                    message.Complete();
                }

                

                // waits 3 seconds (3000 miliseconds)
                System.Threading.Thread.Sleep(3000);

            }
        }
    }
}
