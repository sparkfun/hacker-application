/*--------------------------------------------------------

1. Sam Wizer/ 2/6/2016:

2. Java version used, if not the official version for the class:


3. Precise command-line compilation examples / instructions:

In a terminal, navigate to the directory containing MyWebserver.Java and type
> javac *.java


4. Precise examples / instructions to run this program:

Place MyWebServer.class and WSThread.class in the root directory from which files should be accessed.
In a terminal navigate to the directory and type:
>java MyWebServer
Open Mozilla FireFox, either locally or on a remote machine.
If accessing locally, enter
127.0.0.1:2540
into the address bar.
If accessing remotely, enter
[IP]:2540
where [IP] represents the address of the remote machine.
Navigate directories and files by clinking links or using the back or forward navigation buttons in the browser.

5. List of files needed for running the program.

 a. MyWebServer.java

5. Notes:

----------------------------------------------------------*/

import java.io.*;
import java.net.*;
	
	//Creates a separate thread once a connection is established
	class WSThread extends Thread
	{
		Socket sock;
		File currentDirectory;
		File rootDirectory;
		
		//Constructor for WSThread with a Socket argument.
		WSThread(Socket  _sock)
		{
			sock = _sock;
			currentDirectory =  new File(System.getProperty("user.dir"));
			rootDirectory = new File(System.getProperty("user.dir"));
		}
		
		//The primary function for handling connections and communications from clients
		public void run()
		{
			PrintStream out = null;
			BufferedReader fromClient;
			try
			{
				//Connect to a client and open input and output streams.
				out = new PrintStream(sock.getOutputStream());
				fromClient = new BufferedReader(new InputStreamReader(sock.getInputStream()));
				
				//Read incoming packet from client
				String textFromClient = fromClient.readLine();
				System.out.println(textFromClient);
				
				//Client connects to default address, server returns file listing for root directory
				if (textFromClient.contains("GET / "))
				{
					currentDirectory =  new File(System.getProperty("user.dir"));
					out.println(displayDirectory());
				}
				
				//Parse client's request
				if (textFromClient.contains("/cgi/addnums.fake-cgi"))
				{
					out.println(addNums(textFromClient));
				}
				else
				{
				File directoryListing[] = currentDirectory.listFiles();
				textFromClient = textFromClient.replaceAll("%20", " ");			//Change %20 to a space
				System.out.println(textFromClient);
				for(int i = 0; i < directoryListing.length; i++)
				{
					System.out.println(directoryListing[i]);
					//Does the requested directory exist?
					if (textFromClient != null && textFromClient.contains(directoryListing[i].toString().substring(System.getProperty("user.dir").length() + 1)))
					{
						//Is it contained in a directory or subdirectory of the root directory? (security)
						if (directoryListing[i].getAbsolutePath().contains(rootDirectory.toString()))
						{
							System.out.println("Processing request.");
							out.println(processRequest(directoryListing[i]));
							break;
						}
						else
						{
							//Unauthorized access
							out.println(addHeader("Unautorized Access", "text/HTML"));
						}
					}
				}
				}
				
				sock.close();
			}
			catch (IOException except)
			{
				System.out.println("Server Error");
			}
		}
		
		//Parses input, adds numbers, and returns output for fake CGI.
		String addNums(String textToParse)
		{
			String output = new String();
			int marker1 = textToParse.indexOf("?person=");
			marker1 += "?person=".length();
			int marker2 = textToParse.indexOf("&num1=");
			String name = textToParse.substring(marker1, marker2);
			
			marker2 += "&num1=".length();
			int marker3 = textToParse.indexOf("&num2=");
			int num1 = Integer.parseInt(textToParse.substring(marker2, marker3));
			
			marker3 += "&num2=".length();
			int marker4 = textToParse.indexOf(" HTTP");
			int num2 = Integer.parseInt(textToParse.substring(marker3, marker4));
			
			output = "Hello " + name + ", the sum of  " + num1 + " and " + num2 + " is " + (num1 + num2) + ".";
			return output;
		}
		
		String processRequest(File request)
		{
			//Determine if its a directory or file, print directory if that's what it is, otherwise send out the file
			if (request.isDirectory())
			{
				System.out.println("Directory requested.");
				this.currentDirectory = request;
				return displayDirectory();
			}
			else if (request.isFile())
			{
				System.out.println("File Requested.");
				return sendFile(request);
			}
			else
			{
				return addHeader("error", "text/HTML");
			}
		}
		
		String sendFile(File requestedFile)
		{
			//Open the file requested by client
			String packetToClient = new String();
			try
			{
			FileInputStream fromSource = new FileInputStream(requestedFile);
			
			//Append the contents of the file to the buffer
			InputStreamReader sourceReader = new InputStreamReader(fromSource);
			BufferedReader buffer = new BufferedReader(sourceReader);
			String incoming = new String();
			while ((incoming = buffer.readLine()) != null)
			{
				packetToClient += incoming;
			}
			fromSource.close();
			}
			catch(IOException except)
			{
				System.out.println("Error reading file.");
			}
			//Determine MIME type
			String fileString = requestedFile.toString();
			String MIME;
			String extension = fileString.substring(fileString.indexOf("."));
			if (extension.equals(".xy1"))
			{
				MIME = "application/xy1";
			}
			else
			{
				MIME = "text/HTML";
			}
			
			//Add header to buffer
			packetToClient = addHeader(packetToClient, MIME);
			//And send it out
			System.out.println(packetToClient);
			return packetToClient;
		}
		
		//Determines the size of the buffer, puts it in the header, and puts the header in the front of the buffer
		String addHeader(String in, String MIMEType)
		{
			String output = "HTTP/1.1 200 OK\r\n"
					+ "Content-Type: " + MIMEType+"\r\n"
					+ "Content-Length: ";
			int size = (in.length() + output.length() + 16);				//16 represents the 8 characters and int that will be added in the next line
			output += size +  "\r\n\r\n" + in;
			System.out.println(output);
			return output;
		}
		
		//Gets the files from the current directory and adds them to the buffer, then attaches a header
		String displayDirectory()
		{
			String output = listFiles();
			output = addHeader(output, "text/HTML");
			return output;
		}
		
		//Lists files in current directory, formats them as links in HTML, and appends them to the buffer
		public String listFiles()
		{
			String output = new String();
			
			output += "<BR><H3>Files/Directories: </h3></BR>\n";
			File directory = currentDirectory;
			//directory.
			File[] files = directory.listFiles();
			for (int i = 0; i < files.length; i++)
			{
				if (files[i].isDirectory())
				{
					output+= "Directory:&emsp;<a href = \"" + files[i].toString().substring(System.getProperty("user.dir").length() + 1) + "\">" + files[i].toString().substring(System.getProperty("user.dir").length()) + "</a><br>\n";
					//System.out.println("Directory: " + files[i]);
				}
				else
				{
					//System.out.println("File: " + files[i]);
					output+= "File:&emsp;\t<a href = \"" +  files[i].toString().substring(System.getProperty("user.dir").length() + 1) + "\">" + files[i].toString().substring(System.getProperty("user.dir").length()) + "</a><br>\n";
				}
			}
			return output;
		}
	}
	
	

class MyWebServer
{	
		public static void main (String args[])throws IOException
		{
			int backLog = 6;		//Number of simultaneous requests to store.
			int port = 2540;		//Port for non-admin client connection
			Socket sock;				
			
			ServerSocket serverSocket = new ServerSocket(port, backLog);		//Socket to listen for clients
			
			System.out.println("MyWebServer ready to accept clients.\n");
		
		while (true)
		{
			sock = serverSocket.accept();				//Listen for new clients trying to connect.
			new WSThread(sock).start();			//Creates a separate thread for the client's connection
		}
	}
}