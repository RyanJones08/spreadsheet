/*
 * Written by Alex Gritton, Jordan Wanlass, Triston Thorpe and Ryan Jones.
 * CS 3505 Final Server Code for Multi-User Spreadsheet.
 */


#include <iostream>
#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/enable_shared_from_this.hpp>
#include <sstream>
#include <map>
#include <set>
#include <queue>
#include <vector>
#include <stack>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/xml_parser.hpp>
#include <boost/foreach.hpp>
#include <boost/thread.hpp>

using boost::asio::ip::tcp;
/*
 * This is the client wrapper class that keeps track of client information.
 */
class tcp_client
  : public boost::enable_shared_from_this<tcp_client>
{
	public:
		/*a boost pointer that manages lifetime and memory of an object*/
		typedef boost::shared_ptr<tcp_client> pointer;
		/*a single pointer to a connected client*/
		static pointer create(boost::asio::io_service& io_service)
		{
			return pointer(new tcp_client(io_service));
		}
		
		/*
		 * Deconstructor for a client. This clears the list of spreadsheets the client has worked on.
		 */
		~tcp_client()
		{
			spreadsheets.clear();
		}
		
		/*
		 * Returns the clients socket.
		 */
		tcp::socket& socket()
		{
			return socket_;
		}
		
		/*
		 * Returns the clients unique number.
		 */
		int client_number()
		{
			return client_number_;
		}
		
		/*
		 * Returns the command number. 
		 */
		int command_number()
		{
			return command_number_;
		}
		
		/*
		 * Sets the clients unique id number. 
		 */
		void set_client_num(int num)
		{
			client_number_=num;
		}
		
		/*
		 * Sets the clients command number. 
		 */
		void set_command_number(int num)
		{
			command_number_ = num;
		}
		
		/*
		 * Gets the clients async_read buffer.
		 */
		boost::asio::streambuf& get_buffer()
		{
			return buffer_;
		}
		
		/*
		 * Adds a spreadsheet name to the list of all spreadsheets this object has worked on.
		 */
		void add_spreadsheet(std::string name)
		{
			spreadsheets.insert(name);
		}
		
		/*
		 * Returns the list of all spreadsheets the client has worked on.
		 */
		std::set<std::string>& get_spreadsheet_names()
		{
			return spreadsheets;
		}
	private:
		/*
		 * Client contructor. Initializes a socket for communication.
		 */
		tcp_client(boost::asio::io_service& io_service)
			: socket_(io_service)
		{
		}
		/*The clients socket*/
		tcp::socket socket_;
		/*Unique id number*/
		int client_number_;
		/*The number of subcommand keywords the server needs to listen for*/
		int command_number_;
		/*All spread sheets the client has worked on*/
		std::set<std::string> spreadsheets;
		/*The async_read_until buffer the server accesses*/
		boost::asio::streambuf buffer_;
};
class spreadsheet
{
	public:
		/*A boost pointer object that controls the lifetime and memory of this object*/
		typedef boost::shared_ptr<spreadsheet> pointer;
		/*creates a pointer to the spreadsheet object. Follows singleton pattern*/
		static pointer create(std::string name, std::string password)
		{
			return pointer(new spreadsheet(name, password));
		}
		
		/*
		 * Updates this spreadsheets current version. 
		 */
		void update_version()
		{
			ss_version++;
		}
		
		/*
		 * Returns this spreadsheets current version.
		 */
		int get_version()
		{
			return ss_version;
		}
		
		/*
		 * Returns the password associated with this spreadsheet. 
		 */
		std::string get_password()
		{
			return spreadsheet_password;
		}
		
		/*
		 * Returns the number of clients currently working on this spreadsheet.
		 */
		int get_conn_count()
		{
			return connected_clients_.size();
		}
		
		/*
		 * Adds a client pointer to the list of clients currently working on this spreadsheet.
		 */
		void add_client(tcp_client::pointer new_connection)
		{
			connected_clients_.insert(new_connection);
		}
		
		/*
		 * Removes the given client pointer from the list of clients currently working on this spreadsheet.
		 */
		void remove_client(tcp_client::pointer new_connection)
		{
			connected_clients_.erase(new_connection);
		}
		
		/*
		 * Given a cell name and contents this function changes the spreadsheet accordingly.
		 */
		void add_change(std::string cell_name, std::string contents)
		{
			/*Checks if the given cell name has been previously modified.*/
			if(ss.count(cell_name))
			{
				/*Adds the cell names previous contents to a stack of possible undos*/
				undo_stack.push(std::pair<std::string,std::string>(cell_name, ss[cell_name]));
				/*Changes the cells contents to the given contents*/
				ss[cell_name] = contents;
			}
			/*The cell hasn't been modified before.*/
			else
			{
				/*Adds the cell names previous contents, in this case it was empty before, to the stack of possible undos.*/
				undo_stack.push(std::pair<std::string, std::string>(cell_name, std::string("")));
				/*Inserts the cell name and its new contents into a hashmap of :Key = cell name, Value = contents.*/
				ss.insert(std::pair<std::string, std::string>(cell_name, contents));
			}
		}
		
		/*
		 * Returns the next undo from the undo stack.
		 */
		std::pair<std::string, std::string> get_undo()
		{
			/*Creates a temporary copy of the cell name and contents.*/
			std::pair<std::string, std::string> temp = undo_stack.top();
			undo_stack.pop();
			/*Changes the cell name back to the contents it had before an change was made to it.*/
			ss[temp.first] = temp.second;
			return temp;
		}
		
		/*
		 * Returns if an undo is available. True if an undo is available, fasle if no undo can be made. 
		 */
		bool can_undo()
		{
			if(!undo_stack.empty())
			{
				return true;
			}
			return false;
		}
		
		/*
		 * Returns a HashSet of pointers to clients connected to this spreadsheet.
		 */
		std::set<tcp_client::pointer>& connected_clients()
		{
			return connected_clients_;
		}
		
		/*
		 * Saves the current spreadsheet to a xml file.
		 */
		void save_spreadsheet()
		{
			/*a property tree that will be written to the xml file.*/
			boost::property_tree::ptree tree;
			/*adds the spreadsheet tag to the xml file*/
			tree.add("spreadsheet.<xmlattr>.version", "ps6");
			/*loops through all the cells that have been modified in the hashmap ss*/
			for(std::map< std::string, std::string>::iterator itr = ss.begin(); itr != ss.end(); itr++)
			{
				/*adds the cell tag to the xml file*/
				boost::property_tree::ptree & cell = tree.add("spreadsheet.cell","");
				/*adds the name tag and name of the cell to the xml file in the cell tag*/
				cell.add("name",itr->first);
				/*adds the contents tage and the contents to the xml file in the cell tag*/
				cell.add("contents",itr->second);
			}
			/*adds the .xml extension to the spreadsheet name*/
			std::string f = spreadsheet_name; f+=".xml";
			/*writes the property tree to a xml file*/
			boost::property_tree::write_xml(f.c_str(), tree, std::locale(), boost::property_tree::xml_writer_settings<char>(' ', 4));
			/*clears the undo stack anytime a save is made*/
			while(!undo_stack.empty())
			{
				undo_stack.pop();
			}
		}
		
		/*
		 * Opens a xml file containing spreadsheet data. 
		 */
		void open_spreadsheet()
		{	
			/*property tree to read the xml file into*/			
			boost::property_tree::ptree pt;
			/*filename to read*/
			std::string f = spreadsheet_name; f+=".xml";
			/*trys to opent the file name*/
			std::ifstream filename(f.c_str());
			/*checks if the file was successfully opened.*/	
			if(!filename.fail())
			{		
				/*reads the xml into the proptery tree.*/
				boost::property_tree::read_xml(filename, pt);
				/*loops through the property tree.*/
				BOOST_FOREACH(boost::property_tree::ptree::value_type &v, pt.get_child("spreadsheet"))
				{
					/*check if the tag equals cell*/
					if(v.first == "cell")
					{
						/*gets the name of the cell from the name tag*/
						std::string name = v.second.get<std::string>("name");
						/*gets the contents of the cell from the contents tag*/
						std::string content = v.second.get<std::string>("contents");
						/*checks if the cell is in the spreadsheet hashmap*/
						if(ss.count(name))
						{
							ss[name] = content;
						}
						else
						{
							ss.insert(std::pair<std::string, std::string>(name, content));
						}
					}		
				}
				/*closes the xml file*/
				filename.close();
			}
		}
		
		/*
		 * Generates and returns a xml string to send the client. 
		 */
		std::string send_spreadsheet()
		{
			std::string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><spreadsheet version=\"ps6\">";
			/*Loops through all the cell names and contents appending them to the xml string*/
			for(std::map< std::string, std::string>::iterator itr = ss.begin(); itr != ss.end(); itr++)
			{
				xml+="<cell><name>";
				xml+=itr->first;
				xml+="</name><contents>";
				xml+=itr->second;
				xml+="</contents></cell>";
			}
			xml+="</spreadsheet>";
			return xml;
		}
	private:
		/*Spreadsheet object contructor*/
		spreadsheet(std::string name, std::string password)
		{
			/*sets the spreadsheet name*/
			spreadsheet_name = name;
			/*sets the spreadsheet password*/
			spreadsheet_password = password;
			/*sets the spreadsheet version*/
			ss_version = 0;
		}
		/*Begin Private Variable Declaration*/
		/*A stack of available undos*/
		std::stack<std::pair<std::string, std::string> > undo_stack;
		/*A map of cell names and their contents*/
		std::map<std::string,std::string> ss;
		/*Stores the spread sheet name and password*/
		std::string spreadsheet_name, spreadsheet_password;
		/*current spreadsheet version*/
		int ss_version;
		/*a hashset of all of the clients currently connected to this spreadsheet*/
		std::set<tcp_client::pointer> connected_clients_;
		/*End Private Variable Declaration*/
};
class tcp_server
{
	public:
		/*tcp_server constructor*/
		tcp_server(boost::asio::io_service& io_service, int port_num, bool debug_m = true)
			: acceptor(io_service, tcp::endpoint(tcp::v4(), port_num))
		{
			/*initializes unique client counter*/
			client_number=0;
			debug_mode = debug_m;
			/*starts accepting clients*/
			start_accept();
			/*opens a file containing spreadsheet names and passwords.*/
			open_spreadsheets();
		}
	
	private:
	/*Begin Class Level Variable Declaration*/
		/*If Debug = true, messages are printed to console, else nothing is printed.*/
		bool debug_mode;
		/*Hashmap of all active clients as well as their command queue.*/
		std::map<tcp_client::pointer, std::queue<std::string> > all_clients;
		/*A lock object to make sure only one client is changing the spreadsheet at a time.*/
		boost::mutex lock_object;
		/*A server acceptor that keeps track of io_service.*/
		tcp::acceptor acceptor;
		/*Each client receives a unique number.*/
		int client_number;
		/*A hashmap; Key = spreadsheet name, Value = pointer to the spreadsheet object*/
		std::map<std::string, spreadsheet::pointer> active_spreadsheets;
		/*A hashmap; Key = spreadsheet name, Value = Password*/
		std::map<std::string, std::string> all_spreadsheets;
	/*End Class level vairable Declaration*/
		
		/*
		 *This function begins listening for client connections. 
		 */
		void start_accept()
		{
			/*Creates a new client object.*/
			tcp_client::pointer new_connection = tcp_client::create(acceptor.get_io_service());
			/*Begins listening for the new client object.*/
			acceptor.async_accept(new_connection->socket(), boost::bind(&tcp_server::handle_accept, this, new_connection, boost::asio::placeholders::error));
		}
		
		/*
		 *Callback function async_accept. Once a client has connected this method is called. 
		 */
		void handle_accept(tcp_client::pointer new_connection, const boost::system::error_code& error)
		{
			/*Checks for connection errors*/
			if (!error)
			{
				/*Begins listening for incoming messages from the newly connected client.*/
				startRead(new_connection);
			}
			/*Adds the client to the list of currenly connected clients.*/
			all_clients.insert(std::pair<tcp_client::pointer,std::queue<std::string> >(new_connection,std::queue<std::string>()));
			/*Gives the client a unique number*/
			new_connection->set_client_num(client_number);
			if(debug_mode)
			{	
				std::cout<<"Client connected. Client assigned id="<<client_number<<std::endl;
			}
			/*Updates the unique client number*/
			client_number++;
			/*Begins listening for another client to connect.*/
			start_accept();
		}
		
		/*
		 * This function saves the list of spreadsheets and passwords to an xml file.
		 */
		void save_spreadsheets()
		{
			/*Creates a new propterty tree which will be saved as xml.*/
			boost::property_tree::ptree tree;
			/*Add the spreadsheet identifier to the tree.*/
			tree.add("spreadsheet.<xmlattr>.version", "");
			/*Loops through all the created spreadsheets and passwords adding them to the proptery tree.*/
			for(std::map< std::string, std::string>::iterator itr = all_spreadsheets.begin(); itr != all_spreadsheets.end(); itr++)
			{
				/*creates and info tag that contains a spreadsheet name and password.*/
				boost::property_tree::ptree & cell = tree.add("spreadsheet.info","");
				/*add the spreadsheet name*/
				cell.add("name",itr->first);
				/*add the spreadsheet password*/
				cell.add("password",itr->second);
			}
			/*Saves the proptery tree to a xml file.*/
			boost::property_tree::write_xml("spreadsheetlist.xml", tree, std::locale(), boost::property_tree::xml_writer_settings<char>(' ', 4));
		}
		
		/*
		 * This function opens the list of all spreadsheet names and their password and stores them in a hashmap.
		 */
		void open_spreadsheets()
		{
			/*creates a tree to read into from a xml*/
			boost::property_tree::ptree pt;
			/*attemps to open the xml file*/
			std::ifstream filename("spreadsheetlist.xml");
			/*if the xml didn't fail to read, process it.*/
			if(!filename.fail())
			{			
				/*reads the xml file into the proptery tree*/
				boost::property_tree::read_xml(filename, pt);
				/*loops through the proptery tree adding the names and passwords to a hashmap*/
				BOOST_FOREACH(boost::property_tree::ptree::value_type &v, pt.get_child("spreadsheet"))
				{
					/*parses on the info tag in the xml*/
					if(v.first == "info")
					{
						/*gets the name of the spreadsheet*/
						std::string name = v.second.get<std::string>("name");
						/*gets the name of the password*/
						std::string password = v.second.get<std::string>("password");
						if(debug_mode)
						{
							std::cout<<name<<" "<<password<<std::endl;
						}
						/*adds the spreadsheet name and password to the hashmap*/
						all_spreadsheets[name] = password;
					}		
				}
				/*closes the xml file*/
				filename.close();
			}
		}
		
		/*
		 * This function sends a given message to all connected clients on a given spreadsheet.
		 */
		void send_to_all(std::string name, std::vector<std::string> message, tcp_client::pointer new_connection)
		{	
			/*iter for all connected client on the given spreadsheet*/
			std::set<tcp_client::pointer>::iterator iter;
			/*Loops through all the spreadsheets clients*/
			for (iter = active_spreadsheets[name]->connected_clients().begin(); iter != active_spreadsheets[name]->connected_clients().end(); ++iter)
			{
				/*if the client is not the message originator, then send it the message.*/
				/*Note: the message originator is given a different message.*/
				if(active_spreadsheets[name]->connected_clients().find(new_connection) != iter)
				{
					/*begins sending the message to the client*/
					startWrite(message, (*iter));
				}
			}
		}
		
		/*
		 * Sends a given message to a given client. 
		 */
		void startWrite(std::vector<std::string> messages, tcp_client::pointer new_connection)
		{
			/*Initializes the message*/
			std::string message = "";
			/*loops through all pieces of the message to be send adding new line characters where appropriate.*/
			for(int i = 0; i<messages.size();i++)
			{
				/*appends a piece of the full message with a new line character.*/
				message += messages[i]+"\n";
			}
			/*begins sending the fully contructed message.*/
			boost::asio::async_write(new_connection->socket(), boost::asio::buffer(message),boost::bind(&tcp_server::handle_write,this,boost::asio::placeholders::error));
		}
		
		/*
		 * Callback function for the async_write in startWrite(); 
		 */
		void handle_write(const boost::system::error_code&)
		{
		}
		
		/*
		 * This function begins listening for a message from a given client. 
		 */	
		void startRead(tcp_client::pointer new_connection)
		{
			/*begins listening for a message and calls the callback once a new line character is received.*/
			boost::asio::async_read_until(new_connection->socket(), new_connection->get_buffer(),"\n",boost::bind(&tcp_server::handle_read,this, new_connection,boost::asio::placeholders::error));
		}
		
		/*
		 * Callback function for the async_read_until in startRead(); 
		 */
		void handle_read(tcp_client::pointer new_connection,const boost::system::error_code& )
		{
			/*Gets the bytes of the lines from the clients buffer*/
			std::istream str(&new_connection->get_buffer()); 
			/*A string object to put the line from the client into*/
			std::string s;
			/*Takes the buffer line and puts it into a string object*/
			std::getline(str, s);
			/*Checks if a client has disconnected.*/
			if(s == "")
			{
				if(debug_mode)
				{
					std::cout<<"Diconnecting Client"<<std::endl;
				}
				/*checks if the message received came from an existing client*/
				if(all_clients.count(new_connection))
				{
					/*gets the names of all the spreadsheet that client was working on.*/
					std::set<std::string> names = new_connection->get_spreadsheet_names();
					/*an iterator for the spreadsheet names set*/
					std::set<std::string>::iterator iter;
					/*loops through all the spreadsheets the client was connected to.*/
					for(iter = names.begin();iter!=names.end();++iter)
					{
						/*checks if the spreadsheet the client was working on is still active*/
						if(active_spreadsheets.count((*iter)))
						{
							/*removes the client from the spreadsheet object*/
							active_spreadsheets[(*iter)]->remove_client(new_connection);
							/*checks if this client was the last one working on the spreadsheet.*/
							if(active_spreadsheets[(*iter)]->get_conn_count() == 0)
							{
								/*if there are 0 clients working on a spreadsheet it saves it.*/
								active_spreadsheets[(*iter)]->save_spreadsheet();
								/*removes the spreadsheet from the list of active spreadsheets.*/
								active_spreadsheets.erase((*iter));
							}
						}
					}
					/*closes the client socket*/
					new_connection->socket().close();
					/*removes the client object from the list of active clients.*/
					all_clients.erase(new_connection);
				}
				if(debug_mode)
				{
					std::cout<<"Current Number of Clients = "<<all_clients.size()<<std::endl;
				}
			/*unlocks the lock if the client disconnected mid command process.*/
				//lock_object.unlock();
			}
			/*checks if the command the client sent is a keyword*/
			else
			{
				/*adds the command to the clients queue for later processing.*/
				all_clients[new_connection].push(s);
				if(s == "CREATE" || s=="UNDO")
				{
					/*sets the number of subcommands to listen for*/
					new_connection->set_command_number(2);
					/*starts listening for the subcommands*/
					startReadCommand(new_connection);
				}
				else if(s=="CHANGE")
				{
					/*sets the number of subcommands to listen for*/
					new_connection->set_command_number(5);
					/*starts listening for the subcommands*/
					startReadCommand(new_connection);
				}
				else if(s=="SAVE")
				{
					/*sets the number of subcommands to listen for*/
					new_connection->set_command_number(1);
					/*starts listening for the subcommands*/
					startReadCommand(new_connection);
				}
				else if(s=="LEAVE")
				{
					/*sets the number of subcommands to listen for*/
					new_connection->set_command_number(1);
					/*starts listening for the subcommands*/
					startReadCommand(new_connection);
				}
				else if(s=="JOIN")
				{
					/*sets the number of subcommands to listen for*/
					new_connection->set_command_number(2);
					/*starts listening for the subcommands*/
					startReadCommand(new_connection);
				}
				else
				{
					/*a list of keywords to send*/
					std::vector<std::string> messages;
					/*adds the keyword error because an invalid command was received*/
					std::string e = "ERROR";
					/*adds to the list of keywords to send*/
					messages.push_back(e);
					/*sends the error message to the client that sent the invalid command.*/
					startWrite(messages, new_connection);
					/*starts listening for new messages from the client*/
					startRead(new_connection);
				/*unlocks*/
					//lock_object.unlock();
				}
				if(debug_mode)
				{
					s+=": : Sent By Client ";
					std::stringstream ss;
					ss<<new_connection->client_number();
					s+=ss.str();
					std::cout<<s<<std::endl;
				}
			}	
		}
		
		/*
		 * A function to start listening for subcommand keywords. 
		 */
		void startReadCommand(tcp_client::pointer new_connection)
		{
			/*begins listening for subcommand keywords until a new line character is received.*/
			boost::asio::async_read_until(new_connection->socket(), new_connection->get_buffer(),"\n",boost::bind(&tcp_server::handle_command,this, new_connection,boost::asio::placeholders::error));
		}
		
		/*
		 * Handler function for the subcommand async_read_until in startReadCommand();
		 */
		void handle_command(tcp_client::pointer new_connection,const boost::system::error_code&)
		{
			/*gets the bytes of the line from the buffer.*/
			std::istream str(&new_connection->get_buffer());
			/*a string object to store the line*/ 
			std::string s; 
			/*places the line from the buffer into the string object.*/
			std::getline(str, s);
			/*while there are still subcommand keywords to listen for this is true.*/
			if(new_connection->command_number() > 1)
			{
				/*decrements the number of subcommand keywords to listen for.*/
				new_connection->set_command_number(new_connection->command_number()-1);
				/*adds the subcommand keyword to the client command queue for later processing.*/
				all_clients[new_connection].push(s);
				if(debug_mode)
				{
					s+=": :";
					std::stringstream ss;
					ss<<new_connection->client_number();
					s+=ss.str();
					std::cout<<s<<std::endl;
				}
				/*begins listening for a new subcommand keyword.*/
				startReadCommand(new_connection);
			}
			/*if the last subcommand keyword has been received the command should be processed and a new command is listened for.*/
			else
			{	
				/*decrements the number of subcommand keywords to listen for*/
				new_connection->set_command_number(new_connection->command_number()-1);
				/*adds the last subcommand keyword to the clients command queue.*/
				all_clients[new_connection].push(s);
				if(debug_mode)
				{
					s+=": :";
					std::stringstream ss;
					ss<<new_connection->client_number();
					s+=ss.str();
					std::cout<<s<<std::endl;
				}	
				/*processes the fully received command*/	
				process_command(new_connection);
				/*clears the async_read_until buffer*/
				new_connection->get_buffer().consume(new_connection->get_buffer().size());
				/*begins listening for a new command from the client.*/
				startRead(new_connection);
			}
		}
		
		/*
		 * This function processes a fully received command from a client  
		 */
		void process_command(tcp_client::pointer new_connection)
		{
			/*obtains the lock object so only one command is processed at once.*/
			lock_object.lock();
			/*takes the command keyword from the clients command queue.*/
			std::string s = all_clients[new_connection].front();
			/*removes the keyword from the command queue*/
			all_clients[new_connection].pop();
			
			/*Begin checking command keywords*/
			if(s == "CREATE")
			{
				/*gets the name of the spreadsheet to create*/
				std::string name = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				/*removes command keyword leaving just the name.*/
				name = name.substr(5, name.size()-5);
				/*gets the spreadsheet password*/
				std::string password = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				/*removes the password keyword leaving just the password*/
				password = password.substr(9, password.size()-9);
				/*checks if the spreadsheet name is already taken*/
				if(all_spreadsheets.count(name))
				{
					/*message list to send to client*/
					std::vector<std::string> messages;
					/*creates the messages to send to client*/
					std::string m = "CREATE FAIL", n = "Name:", message = "Spreadsheet name is already taken.";
					n+=name;
					/*adds the messages to the message list*/
					messages.push_back(m);messages.push_back(n);messages.push_back(message);
					/*sends the message to the client*/
					startWrite(messages, new_connection);
					
				}
				/*default if spreadsheet name is not taken.*/
				else
				{
					/*adds the spreadsheet name and password to a set of names and passwords*/
					all_spreadsheets.insert(std::pair<std::string,std::string>(name,password));
					/*saves the spreadsheet name and password*/
					save_spreadsheets();
					if(debug_mode)
					{
						std::cout<<"Created Spreadsheet"<<std::endl;
					}
					/*generates the create ok messages to send to the client*/
					std::string m = "CREATE OK", n = "Name:", p = "Password:";
					n+=name; p+=password;
					/*a list of messages to send to the client*/
					std::vector<std::string> messages;
					/*adds the create ok command keywords to the list of messages to send to the client*/
					messages.push_back(m); messages.push_back(n); messages.push_back(p);
					/*send the messages to the client*/
					startWrite(messages, new_connection);
				}
			}
			else if(s=="JOIN")
			{
				/*gets the name of the spreadsheet the client wants to join*/
				std::string name = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				/*checks if a name was actually given*/
				if(name.size()>5)
				{
					/*extracts the spreadsheet name for the name command*/
					name = name.substr(5, name.size()-5);
					/*check if the spreadsheet has been created.*/
					if(all_spreadsheets.count(name))
					{
						/*gets the password to the spreadsheet*/
						std::string password = all_clients[new_connection].front();
						all_clients[new_connection].pop();
						/*extracts the password from the password command*/
						password = password.substr(9, password.size()-9);
						/*checks if the password is the password to the spreadsheet name given*/
						if(all_spreadsheets[name] == password)
						{
							/*checks if the spreadsheet is currently opened by another client*/
							if(!active_spreadsheets.count(name))
							{
								if(debug_mode)
								{
									std::cout<<"Opening Spreadsheet"<<std::endl;
								}
								/*adds the spreadsheet to the list of currently active spreadsheets.*/
								active_spreadsheets.insert(std::pair<std::string,spreadsheet::pointer>(name, spreadsheet::create(name, password)));					
								/*reads in the spreadsheets xml file*/
								active_spreadsheets[name]->open_spreadsheet();
							}
							else
							{
								if(debug_mode)
								{
									std::cout<<"Adding to active Spreadsheet"<<std::endl;
								}
							}
							/*adds the client to the active spreadsheet.*/
							active_spreadsheets[name]->add_client(new_connection);
							/*adds the spreadsheet name to the clients list of spreadsheets it currently is working on.*/
							new_connection->add_spreadsheet(name);
							/*creates the messages to send to the client*/
							std::string m= "JOIN OK", n = "Name:", v = "Version:", l = "Length:1";
							n+=name;
							/*gets the spreadsheets current version to send the client*/
							std::stringstream ss; ss<<active_spreadsheets[name]->get_version();
							v+=ss.str();
							/*gets the xml to send to the client*/
							std::string xml = active_spreadsheets[name]->send_spreadsheet();
							/*a list of messages to send to the client*/
							std::vector<std::string> messages;
							/*adds the messages to send to the message list*/
							messages.push_back(m); messages.push_back(n); messages.push_back(v); messages.push_back(l); messages.push_back(xml);
							/*begins sending the message to the client*/
							startWrite(messages, new_connection);
						}
						/*if the password didn't match the filename*/
						else
						{
							if(debug_mode)
							{
								std::cout<<"Send Join Fail: Invalid Password for filename"<<std::endl;
							}
							/*list of messages to send to client*/
							std::vector<std::string> messages;
							/*creates the join fail messages*/
							std::string m = "JOIN FAIL", n = "Name:", message = "Invalid password for given filename.";
							n+=name;
							/*adds the join fail messages to the messages list*/
							messages.push_back(m); messages.push_back(n); messages.push_back(message);
							/*sends the join fail to the client*/
							startWrite(messages, new_connection);
						}
					}
					/*if the spreadsheet name provided hasn't yet been created*/
					else
					{
						if(debug_mode)
						{
							std::cout<<"Send Join Fail: Invalid Spreadsheet name"<<std::endl;
						}
						/*a list of messages to send to the client*/
						std::vector<std::string> messages;
						/*creates the join fail messages*/
						std::string m = "JOIN FAIL", n = "Name:", message = "Invalid filename: Spreadsheet doesn't exist.";
						n+=name;
						/*adds the messages to send to the list of messages*/
						messages.push_back(m); messages.push_back(n); messages.push_back(message);
						/*pops the password keyword from the command queue.*/
						all_clients[new_connection].pop();
						/*sends the join fail to the client*/
						startWrite(messages, new_connection);
					}
				}
			}
			else if(s=="UNDO")
			{
				/*gets the name of the spreadsheet where the undo is happening*/
				std::string name = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				/*extracts the name for the name keyword*/
				name = name.substr(5, name.size()-5);
				/*gets the version the client sent*/
				std::string version = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				/*extracts the version for the version keyword*/
				version = version.substr(8, version.size()-8);
				/*checks if the spreadsheet name the client sent is an active spreadsheet*/
				if(!active_spreadsheets.count(name))
				{
					if(debug_mode)
					{
						std::cout<<"Send Undo Fail with message no active spreadsheet"<<std::endl;
					}
					/*a message list to send to the client*/
					std::vector<std::string> messages;
					/*messages to send the client*/
					std::string m = "UNDO FAIL", n = "Name:", message = "The Spreadsheet is not open";
					n+=name;
					/*adds the messages to the messages list*/
					messages.push_back(m);messages.push_back(n);messages.push_back(message);
					/*begins sending the undo fail command to the client*/
					startWrite(messages, new_connection);
				}
				/*checks if the clients verison of the spreadsheet matches the actual version of the spreadsheet*/
				else if(active_spreadsheets[name]->get_version() != atoi(version.c_str()))
				{
					if(debug_mode)
					{
						std::cout<<"Sending undo Wait"<<std::endl;
					}
					/*a list of messages to send the client*/
					std::vector<std::string> messages;
					/*converts the spreadsheet version to a string for sending*/
					std::stringstream ss; ss<<active_spreadsheets[name]->get_version();
					/*messages to send the client*/
					std::string m= "UNDO WAIT",n = "Name:", v = "Version:";
					n+=name; v+=ss.str();
					/*adds the messages to send the message list*/
					messages.push_back(m); messages.push_back(n); messages.push_back(v);
					/*sends the undo wait command to the client*/
					startWrite(messages, new_connection);
				}
				/*checks if the spreadsheet object has any undos available*/
				else if(active_spreadsheets[name]->can_undo())
				{
					/*gets the old cell value to send the client*/
					std::pair<std::string, std::string> temp_undo = active_spreadsheets[name]->get_undo();
					/*creates an undo ok message list and an update message list*/
					std::vector<std::string> messages, messages_ok;
					/*updates the spreadsheet version*/
					active_spreadsheets[name]->update_version();
					/*gets the spreadsheet version and converts it to a string*/
					std::stringstream ss; ss<<active_spreadsheets[name]->get_version();
					/*messages for the undo ok client and all other clients*/
					std::string m = "UNDO OK", m2 = "UPDATE";			
					std::string n = "Name:", v = "Version:", c = "Cell:", l = "Length:1", contents = temp_undo.second;
					n+=name; v+=ss.str(); c+=temp_undo.first;
					/*adds the undo ok messages to the undo ok messages list*/
					messages.push_back(m); messages.push_back(n); messages.push_back(v); messages.push_back(c); messages.push_back(l); messages.push_back(contents);
					/*adds the update messages to the update messages list*/
					messages_ok.push_back(m2); messages_ok.push_back(n); messages_ok.push_back(v); messages_ok.push_back(c); messages_ok.push_back(l); messages_ok.push_back(contents);
					/*Sends the undo ok command to the client who sent the undo command*/
					startWrite(messages, new_connection);
					/*sends the update command to all other clients*/
					send_to_all(name, messages_ok, new_connection);
					if(debug_mode)
					{
						std::cout<<m<<n<<v<<c<<l<<contents<<std::endl;
						std::cout<<"Sending undo ok"<<std::endl;
					}
				}
				/*sends an undo end since the spreadsheet has no more undos*/
				else
				{
					/*a messages list to send to the client*/
					std::vector<std::string> messages;
					/*gets the spreadsheets current version*/
					std::stringstream ss; ss<<active_spreadsheets[name]->get_version();
					/*messages to send to the client*/
					std::string m = "UNDO END",n = "Name:",v = "Version:";
					n+=name; v+=ss.str();
					/*adds the messages to the messages list to send client*/
					messages.push_back(m); messages.push_back(n); messages.push_back(v);
					/*sends the undo end messsages to the client*/
					startWrite(messages, new_connection);
					if(debug_mode)
					{
						std::cout<<"Sending undo end"<<std::endl;
					}
				}
			}
			else if(s=="CHANGE")
			{
				/*gets the name of the spreadsheet to change*/
				std::string name = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				name = name.substr(5, name.size()-5);
				/*gets the clients version to check against the actual spreadsheets version*/
				std::string version_num = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				version_num = version_num.substr(8, version_num.size()-8);
				/*gets the name of the cell thats being modified*/
				std::string cell = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				cell = cell.substr(5, cell.size()-5);
				/*gets the length of the contens. NOTE: Not used in code :END*/
				std::string length = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				length = length.substr(7, length.size()-7);
				/*gets the contents of the cell being modified*/
				std::string contents = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				/*checks if the spreadsheet is currently active*/
				if(!active_spreadsheets.count(name))
				{
					if(debug_mode)
					{
						std::cout<<"Send Change Fail with message no active spreadsheet"<<std::endl;
					}
					/*change fail messages list*/
					std::vector<std::string> messages;
					/*change fail messages*/
					std::string m = "CHANGE FAIL", n = "Name:", message = "The Spreadsheet is not open";
					n+=name;
					/*adds change fail messages to messages list to send to client*/
					messages.push_back(m); messages.push_back(n); messages.push_back(message);
					/*start sending the change fail messages to the client*/
					startWrite(messages, new_connection);
				}
				/*checks if the spreadsheet version the client send matches the spreadsheet objects version*/
				else if(active_spreadsheets[name]->get_version() == atoi(version_num.c_str()))
				{
					/*changes the cells contents in the spreadsheet object to those sent by the client*/
					active_spreadsheets[name]->add_change(cell, contents);
					/*updates the spreadsheet objects version number*/
					active_spreadsheets[name]->update_version();
					/*update and change ok messages lists*/
					std::vector<std::string> messages, message_ok;
					/*gets the spreadsheet objects current version to send client*/
					std::stringstream ss; ss<<active_spreadsheets[name]->get_version();
					/*messages to send clients*/
					std::string m= "UPDATE", m2 = "CHANGE OK", n = "Name:", v = "Version:", c = "Cell:", l = "Length:1";
					n+=name; v+=ss.str(); c+=cell;
					/*adds messages to the messages lists*/
					messages.push_back(m); messages.push_back(n); messages.push_back(v); messages.push_back(c); messages.push_back(l); messages.push_back(contents);
					message_ok.push_back(m2); message_ok.push_back(n); message_ok.push_back(v);
					/*send the change ok messages to the client who sent the change command*/
					startWrite(message_ok, new_connection);
					/*sends the update ok to all other clients working on this spreadsheet*/
					send_to_all(name,messages, new_connection);
					if(debug_mode)
					{
						std::cout<<active_spreadsheets[name]->get_version()<<std::endl;
						std::cout<<"Change Ok"<<std::endl;
					}		
				}
				/*if the version the client sent does not match the spreadsheet objects current version, send change wait.*/
				else
				{
					if(debug_mode)
					{
						std::cout<<active_spreadsheets[name]->get_version()<<std::endl;
						std::cout<<"Send Change Wait"<<std::endl;
					}
					/*messages list to send client*/
					std::vector<std::string> messages;
					/*gets the spreadsheet objects current version*/
					std::stringstream ss; ss<<active_spreadsheets[name]->get_version();
					/*change wait messages to send clients*/
					std::string m= "CHANGE WAIT", n = "Name:", v = "Version:";
					n+=name; v+=ss.str();
					/*adds the change wait messages to the messages list*/
					messages.push_back(m); messages.push_back(n); messages.push_back(v);
					/*sends the change wait messages to the client*/
					startWrite(messages, new_connection);
				}
			}
			else if(s=="LEAVE")
			{
				/*gets the name of the spreadsheet the client wants to leave*/
				std::string name = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				/*makes sure a name was actually sent*/
				if(name.size() > 5)
				{
					name = name.substr(5, name.size()-5);
					/*checks that the spreadsheet name the client wants to leave is actually active*/
					if(active_spreadsheets.count(name))
					{
						/*removes the client from the spreadsheet objects list of clients*/
						active_spreadsheets[name]->remove_client(new_connection);
						/*check if this was the last client modifying this spreadsheet*/
						if(active_spreadsheets[name]->get_conn_count() == 0)
						{
							/*saves the spreadsheet to a xml file*/
							active_spreadsheets[name]->save_spreadsheet();
							/*removes the spreadsheet from the list of active spreadsheets*/
							active_spreadsheets.erase(name);
							if(debug_mode)
							{
								std::cout<<"Closing Spreadsheet "<<name<<std::endl;
							}
						}
					}
				}
			}
			else if(s=="SAVE")
			{
				/*gets the name of the spreadsheet the client wants to save*/
				std::string name = all_clients[new_connection].front();
				all_clients[new_connection].pop();
				name = name.substr(5, name.size()-5);
				/*messages list*/
				std::vector<std::string> messages;
				/*save messages to send client*/
				std::string m, n = "Name:", message = "Error saving file.";
				n+=name;
				try
				{
					/*saves the spreadsheet to a xml file*/
					active_spreadsheets[name]->save_spreadsheet();
				/*TODO: VERIFY THIS IS NEEDED*/
					all_spreadsheets.insert(std::pair<std::string, std::string>(name, active_spreadsheets[name]->get_password()));
					m = "SAVE OK";
				}
				catch(int e)
				{
					m = "SAVE FAIL";
					messages.push_back(message);	
				}
				/*adds the save messages to the messages list*/
				messages.push_back(m); messages.push_back(n);
				/*send the save messages to the client*/
				startWrite(messages, new_connection);
				if(debug_mode)
				{
					std::cout<<"Saving"<<std::endl;
				}
			}
			/*End command keyword checking*/
			/*the command has been processed so the command object is released.*/
			lock_object.unlock();
		}
};
/*
 * This is the main function which creates an instance of the server and runs it.
 */
int main(int args, char* argv[])
{
	/*The port the server is running on.*/
	int port_num = 1984;
	bool debug = true;
	boost::asio::io_service io_serv;
	/*Checks if parameters have been passed*/
	if(args > 1)
	{
		/*tries second parameter for port number*/
		try
		{
			port_num = atoi(argv[1]);
		}
		catch(int e)
		{
			std::cout<<"Invalid Port Number"<<std::endl;
			port_num = 1984;
		}
		/*tries third parameter for debug mode value*/
		if(args > 2)
		{
			/*if its false turns debug mode off. No console prints from server*/
			if(argv[2] == "false")
			{
				debug = false;
			}
			else
			{
				std::cout<<"Invalid Debug Mode: accepts keyword \"false\" only."<<std::endl;
			}
		}
	}
	/*Keeps the server running.*/
	std::cout<<"Spreadsheet server running on Port: "<<port_num<<" . Debug Mode: "<<debug<<std::endl;
	tcp_server my_server(io_serv, port_num, debug);
	io_serv.run();
	return 0;
}
