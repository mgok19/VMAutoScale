// Michael O'Keefe
// Cloud Computing, Project 1 - Checkpoint 2
// Due: 10/29/15
// 
// Producer is a console application that writes one message every second
// to the queue 'michaelq' located within Microsoft Azure. A producer 
// needs to be uniquely identified with a name, that is specified by the 
// creator in the first command line arguement. Messages will include the 
// producer's ID and the message number (example: first message is referred 
// to as 'message#1')
// 
// Assumtions: 
// - The queue 'michaelq' has already been created and exists (note that if the
//   queue did not exist, it would normally be the producers job to create the queue)

using System;
using Microsoft.WindowsAzure;
using Microsoft.ServiceBus.Messaging;

namespace Producer_CP2
{
    class Program
    {
        static void Main(string[] args)
        {
            // checks if there are no arguements passed
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter producer identification as arguement one");
            }
            else
            {
                // assuming the queue is created and exists, create a client queue
                string connectionString =
                    CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

                QueueClient client =
                    QueueClient.CreateFromConnectionString(connectionString, "michaelq");

                // send messages to queue and console (one message per second)
                Boolean sendMessage = true;
                int messageCount = 0;
                string producerID = args[0];

                while (sendMessage) // infinite loop 
                {
                    messageCount++;

                    // set message contents
                    BrokeredMessage message =
                        new BrokeredMessage(producerID + " message#" + messageCount);

                    // set message properties 
                    message.Properties["Message number"] = messageCount;
                    message.Properties["Message producer"] = producerID;

                    // sends message to client queue
                    client.Send(message);

                    // prints message to console
                    System.Console.WriteLine(producerID + " message#" + messageCount);

                    // waits 1 second (1000 miliseconds)
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
}

