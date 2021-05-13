using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DungeonGame.Event;

namespace DungeonGame
{
    public class ProgramUI
    {
        //enum to give us item types
        //inventory list to hold items we pick up
        public enum Item { key, mechanism, slingshot }
        public List<Item> inventory = new List<Item>();

        //a place all rooms can access to store their exits
        Dictionary<string, Room> Rooms = new Dictionary<string, Room>

        {
            {"bedroom", bedroom },
            {"sitting", sitting },
            {"tower", tower },
            {"study", study },
            {"lab", lab },
            {"roof", roof },
        };



        public void Run()
        {
            Room currentRoom = bedroom;

            //first line printed to console when opening the game
            Console.WriteLine("Hello, welcome to our adventure game! \n" +
                "HAUNTED TOWER ESCAPE \n" +
                "Enter commands to try and escape. Type 'help' at any time to see a list of commands.\n" +
                "Press any key to continue");
            Console.ReadKey();

            Console.WriteLine("You awake in a hard, lumpy bed.\n" +
                "You have no memory of how you might have gotten here.\n" +
                "A faintly putrid stench flits under your nostils.\n" +
                "Dark thoughts fill your mind and you have an overwhelming feeling that you must leave this place\n" +
                "Press any key to continue");
            Console.ReadKey();

            bool isAlive = true;
            bool hasOpenedTower = false;
            bool trollAlive = true;

            //the main program loop
            while (isAlive)
            {
                Console.Clear();
                Console.WriteLine(currentRoom.Description);
                Console.WriteLine("What would you like to do?");

                string userInput = Console.ReadLine().ToLower();

                if (userInput.StartsWith("help"))
                {
                    Console.WriteLine("Valid commands are 'go' 'use' 'take' 'examine'.\n" +
                        "You then always want to follow your command with the object or room you are trying to interact with or move to.\n" +
                        "To see your inventory type 'inventory'\n" +
                        "An example would be typing in 'go sitting room'. Or 'examine dresser'.\n" +
                        "Please note room names are always in CAPITOL letters\n" +
                        "You can even 'use' some items in your inventory!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
                else if (userInput.StartsWith("go") || userInput.StartsWith("exit"))
                {
                    //referencing our dictionary to see if the exit exits for the current room
                    //Console.WriteLine("Where would you like to go?");
                    //userInput = Console.ReadLine().ToLower();
                    bool foundExit = false;
                    foreach (string exit in currentRoom.Exits)
                    {
                        if (userInput.Contains(exit))
                        {
                            //extra checks for tower
                            //if ther user tries to go to the tower & they have the key & this is thier first time opening the door (which is the hasOpenedTower variable) then it gives them the first condition
                            if (userInput.Contains("tower") && inventory.Contains(Item.key) && hasOpenedTower == false)
                            {
                                Console.WriteLine("You unlock the door with the key you found behind the painting.\n" +
                                    "You can now enter the tower!.");
                                foundExit = true;
                                hasOpenedTower = true;
                                currentRoom = Rooms[exit];
                            }
                            else if (userInput.Contains("roof") && trollAlive == true)
                            {
                                Console.WriteLine("You try to sneak past the troll but its keen sense of smell easily detects the precence of an intruder.\n" +
                                    "He waits until you are nearly to the stairs and then grabs you. Laughing horribly.\n" +
                                    "\n\nGAME OVER.\nPress any key to exit...");
                                Console.ReadKey();
                                isAlive = false;
                            }
                            //if the user has already opened the tower, they go here
                            else if(userInput.Contains("tower") && hasOpenedTower)
                            {
                                foundExit = true;
                                currentRoom = Rooms[exit];

                                break;
                            }
                            //if the user has not opened the tower, they go here
                            else if (userInput.Contains("tower") && !inventory.Contains(Item.key))
                            {
                                Console.WriteLine("The tower door is locked tightly. How will you get in?");
                                Console.ReadKey();
                            }
                            //checks if troll is alive before going to the roof
                            else
                            {
                                foundExit = true;
                                currentRoom = Rooms[exit];

                                break;
                            }
                        }
                    }
                    if (!foundExit)
                    {
                        Console.WriteLine("I do not recognize that room, or it is unreachable from here");
                    }
                }
                else if (userInput.StartsWith("take") || userInput.StartsWith("get"))
                {
                    bool foundItem = false;
                    foreach (Item item in currentRoom.Items)
                    {
                        if (!foundItem && userInput.Contains(item.ToString()))
                        {
                            Console.WriteLine("You found a: " + item);
                            //Console.WriteLine("Press any key to continue");
                            currentRoom.RemoveItem(item);
                            inventory.Add(item);
                            foundItem = true;
                            break;
                        }
                    }
                    if (!foundItem)
                    {
                        Console.WriteLine("There is nothing here of that name that you can take");
                    }
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
                else if (userInput.StartsWith("use") || userInput.StartsWith("examine"))
                {
                    string eventMessage = "You look at the object. Nothing happens";
                    foreach (Event roomEvent in currentRoom.Events)
                    {

                        if (!userInput.Contains(roomEvent.TriggerPhrase))
                        {
                            continue;
                        }
                        if (roomEvent.EventResult.Type == Result.ResultType.GetItem)
                        {
                            inventory.Add(roomEvent.EventResult.ResultItem);
                            eventMessage = roomEvent.EventResult.ResultMessage;
                        }
                        else if (roomEvent.EventResult.Type == Result.ResultType.MessageOnly)
                        {
                            if (roomEvent.TriggerPhrase.Contains("use slingshot") && inventory.Contains(Item.slingshot))
                            {
                                eventMessage = roomEvent.EventResult.ResultMessage;
                                trollAlive = false;
                            }
                            
                        }
                        else if(roomEvent.EventResult.Type == Result.ResultType.GameOver)
                        {
                            if(roomEvent.TriggerPhrase.Contains("use balloon") && !inventory.Contains(Item.mechanism))
                            {
                                Console.WriteLine("The hot air balloon seems to be missing a part on the firing mechanism.\n" +
                                    "This balloon will be useless unless you can fix it.\n" +
                                    "You wonder if you have seen a mechanisim somewhere that would fit");
                            }
                            else
                            {
                            eventMessage = roomEvent.EventResult.ResultMessage;

                            isAlive = false;
                            }
                        }
                        

                    }
                    Console.WriteLine(eventMessage);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
                else if (userInput.Contains("inventory"))
                {
                    Console.WriteLine("Your inventory contains:");
                    foreach(Item i in inventory)
                    {
                        Console.WriteLine(i);
                    }
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }

                else
                {
                    Console.WriteLine("I do not recognize that command. Please type 'help' for a list of valid commands");
                }
                


            }


        }

        //new bedroom object -description -exits - items - events
        public static Room bedroom = new Room("BEDROOM\n" +
            "You are in a small bedroom.\n" +
            "As you look around the furnishings are scarce and utilitarian, indicating to you the room may only be used for sleeping.\n" +
        "You see a small dresser on one side of the room, the bed you lay in, and a small painting of ship on the east wall. \n" +
            "There is a door to a SITTING room to your left\n",
            new List<string> { "sitting" },
            new List<Item> { },
            //adding new event - using event contructor inside of new list event
            //new list holds an event object. That event object will then hold a new result object
            //              Event trigger phrase   result-item to add   resulting message to display
            new List<Event> 
            { 
                new Event("examine painting", new Result(Item.key, "As you look more closely you see scratch marks around the painting.\n" +
                "You realize there must be something hidden behind it.\n" +
                "As you move the painting aside you see a small hole in the wall.\n" +
                "Reaching inside you find a small key hidden within. You quickly pocket the key and replace the painting...\n")), 
                new Event("examine dresser", new Result("The dresser appears to have some standard issue wizards robes and a pointy hat on top.\n" +
                    "What wizard would leave thier pointy hat behind?\n"))
            });

        public static Room sitting = new Room("SITTING room\n" +
            "You are in a SITTING room.\n" +
            "There are six chairs around a table in the middle of the room. \n" +
            "An ornate suit of armor is on a display stand is in the corner.\n" +
            "It seems to be pulling you in, asking you to gaze upon its beauty. \n" +
            "There is a door to the west leading into a STUDY, a door to the TOWER to the east, and the BEDROOM to the south.\n",
            new List<string> { "bedroom", "tower", "study" },
            new List<Item> { },
            new List<Event> 
            { 
                new Event("examine armor", new Result("As you gaze in wonder upon the glimmering suit of armor, you begin to move towards it.\n" +
                "You have never been this happy in your whole life...\n\n" +
                "Or more terrified! As you step closer and closer you realize that you are no longer in control.\n" +
                "You cannot look away even if you wanted to! An empty gauntlet moves to reach out and touch you.\n" +
                "Dazed you are powerless to resist as the world around you fades to black.\n\nGAME OVER.\nPress any key to exit...", true)), 
                new Event("examine table", new Result("The table appears to be covered in magazines about various subjects.\n" +
                    "Of particular note are a magazine about black cats, one about the uses of pickled body parts, and one about witches makeup routines\n"))
            });

        public static Room tower = new Room("TOWER\n" +
            "A tall circular room with a chandelier hanging above the center of the room.\n" +
            "There is a large troll taking up most of the room, you cannot tell if it is dead or just sleeping.\n" +
            "A spiral staircase circles around the entire outer wall leading up to the ROOF.\n",
            new List<string> { "sitting", "upstairs", "roof" },
            new List<Item> { },
            new List<Event>
            { 
                new Event("use slingshot", new Result("You use the slingshot to hit the weak link in the chandelier hanging above the sleeping troll.\n" +
                "It drops with a huge crash, straight on top of the troll.\n" +
                "He stops snoring.")),
                new Event("examine chandelier", new Result("You look at the chandelier. It seems that there might be a weak link in it. If only you had something in your inventory you could USE...\n"))
            });

        public static Room roof = new Room("ROOF\n" +
            "The roof of the tower appears to be above the clouds.\n" +
            "There is nothing of great significance up here aside from a hot air baloon. Perhaps you can use it to escape... \n" +
            "The stairs behind you lead back to the TOWER\n",
            new List<string> { "tower" },
            new List<Item> { },
            new List<Event> 
            { 
                new Event("use balloon", new Result("You approach the hot air balloon.\n" +
                "You fit the part you found in the glass case into the firing mechanizim fairly easily.\n" +
                "After you rig it up you then you easily fly away to make an escape! Congratulations, you escaped!\n\n" +
                "Press any key to close the game...\n", true)),
                new Event("examine baloon", new Result("This looks like something that might be USEful"))
            });

        public static Room study = new Room("STUDY\n" +
            "The study is a mess of papers and strange glowing items.\n" +
            "There is a large desk in the middle of the room that has a large book open on top.\n" +
            "The walls are lined with shelves covered in loose papers and books.\n" +
            "There is a glass case displaying a variety of strange items \n" +
            "There is a door to the south leading to a LAB, and a door to the east leading to the SITTING room\n",
            new List<string> { "sitting", "lab" },
            new List<Item> { Item.mechanism },
            new List<Event> 
            { 
                new Event("examine case", new Result("You look into the case and see various odds and ends.\n" +
                "It looks like this is mainly just a bunch of junk upon closer inspection.\n" +
                "Sickly green toenails, a large crystal ball, a humanoid skull, and several small identical mechanisms made out of metal\n")), 
                new Event("examine desk", new Result("The desk has papers covered in a scrawling script you cannot read.\n" +
                    "Your attention is drawn to a large book laying open in the middle of all the mess.\n" +
                    "The page reads:\n" +
                    "The secrets of the elder lay hidden within these pages. To read them is to know, and to know is to read.\n" +
                    "Such great power at your fingertips, but once you turn the first page you must read until the reading is done...\n" +
                    "You decide it best to move on")),
                new Event("examine book", new Result("The desk has papers covered in a scrawling script you cannot read.\n" +
                    "Your attention is drawn to a large book laying open in the middle of all the mess.\n" +
                    "The page reads:\n" +
                    "The secrets of the elder lay hidden within these pages. To read them is to know, and to know is to read.\n" +
                    "Such great power at your fingertips, but once you turn the first page you must read until the reading is done...\n" +
                    "You decide it best to move on")),
                new Event("examine shelves", new Result("The papers are in languages you do not understand. Books on the shelves include titles mainly dealing with how to be a powerful and fashionable wizard.")),
                new Event("examine papers", new Result("The papers are in languages you do not understand. Books on the shelves include titles mainly dealing with how to be a powerful and fashionable wizard.")),
                new Event("examine skull", new Result("Bleached white, the skull resembles something humanoid.\n" +
                    "Except for the big snout that is")),
                new Event("examine crystal", new Result("You gaze into the crystal ball and grow dizzy")),
                new Event("examine toenails", new Result("Very, very gross.")),
                new Event("examine mechanism", new Result("Looks like a small lever of some kind.\n" +
                    "No one would notice if you took one, would they?"))

            });

        public static Room lab = new Room("LAB\n" +
            "As you enter the lab you are overtaken by a stench so powerful you nearly pass out...\n" +
            "But the sensation is gone almost as quickly as it came on. \n" +
            "There is a set of vials with mysterious liquids to one side of the room.\n" +
            "Taxidermy creatures you've never seen before are scattered throughout the room on top of small tables \n" +
            "There is a chest partially covered by a taxidermy hawk to one side of the room\n" +
            "There is a door to the north leading back to the STUDY\n",
            new List<string> { "study" },
            new List<Item> { },
            new List<Event> 
            { 
                new Event("use vial", new Result("Why on earth would you do that?" +
                "\n\nGAME OVER.\nPress any key to exit...", true)), 
                new Event("use chest", new Result(Item.slingshot, "You open the chest and inside you find several rusty and useless weapons. As you sort through the contents however, you find a slingshot that seems to be functional! You add the slingshot to your inventory.")),
                new Event("examine vial", new Result("This looks like something that might be USEful")),
                new Event("examine chest", new Result("This looks like something that might be USEful")),
                new Event("examine taxidermy", new Result("Scattered accross the room is a zoo of taxidermy animals.\n" +
                    "Some of which have horrible expressiosns as if they are in some excruciating pain.\n" +
                    "Of note is a very large bird covered in scales hanging from the cieling, as well as a small green creature holding a club"))
            });


      


    }




}

