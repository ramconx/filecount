// For Directory.GetFiles and Directory.GetDirectories 
// For File.Exists, Directory.Exists 
using System;
using System.IO;
using System.Collections;
using System.Net.Mail;

public class FileCount
{
    static int filecounter = 0;
    static string path = "C:\\neu";

    public static void Main()
    {
        if (File.Exists(path))
        {
            ProcessFile(path); // This path is a file
        }
        else if (Directory.Exists(path))
        {
            ProcessDirectory(path); // This path is a directory
        }
        else
        {
            Console.WriteLine("{0} is not a valid file or directory.", path);
        }

        //After all subdirectories and files are checked call checker() method
        Checker();
    }

    // Process all files in the directory passed in, recurse on any directories  
    // that are found, and process the files they contain. 
    public static void ProcessDirectory(string targetDirectory)
    {
        // Process the list of files found in the directory. 
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
            ProcessFile(fileName);

        // Recurse into subdirectories of this directory. 
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
            ProcessDirectory(subdirectory);
    }

    // Insert logic for processing found files here. Now only filecounter++ to get amount of files
    public static void ProcessFile(string path)
    {
        Console.WriteLine("Processed file '{0}'.", path);
        filecounter++;
    }

    // If the filecounter is bigger than 0, send a Mail via the SendMail(filecounter) method
    public static void Checker()
    {
        if (filecounter != 0)
        {
            Console.WriteLine("Der Counter ist: " + filecounter);
            SendMail(filecounter);
            Console.WriteLine("Email versendet!");
        }
        else
        {
            Console.WriteLine("Es ist im Moment kein File vorhanden");
            Console.ReadKey();
        }
    }

    // Send Emails via Gmail, self explanatory
    public static void SendMail(int filecounter)
    {
        string from = "@gmail.com"; // Add the Creddentials- email adress
        string to = "@gmail.com"; // Add the receiving email
        string to2 = "@gmail.com";
        string machine = Environment.MachineName.ToString();

        // Add all parts of the mail together
        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        mail.To.Add(to);
        mail.To.Add(to2);
        mail.From = new MailAddress(from, "File" , System.Text.Encoding.UTF8);
        mail.Subject = "Es sind ungesendete Emails auf "+ machine +" vorhanden";
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.Body = "Es sind " + filecounter + " Files im Ordner '" + path + "' auf " + machine + " vorhanden.";
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true ;
        mail.Priority = MailPriority.High;
 
        // Setup a new SMTP-Client
        SmtpClient client = new SmtpClient();
        client.Credentials = new System.Net.NetworkCredential(from, "password"); // Add the Creddentials- email password
        client.Port = 587; // Gmail works on this port
        client.Host = "smtp.gmail.com";
        client.EnableSsl = true; //Gmail works on Server Secured Layer

        //try to send this mail
        try
        {
            client.Send(mail);
        }
        catch (Exception ex)
        {
            String error_output = ex.ToString();
            Console.WriteLine(error_output);

            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\fileckecher_error.txt");
            file.WriteLine(error_output);
            file.Close();
        }
    }
}
