using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DungeonGame.ProgramUI;

namespace DungeonGame
{
    public class Room
    {

        public string Description { get; }
        public List<string> Exits { get; }
        public List<Item> Items {get;}

        public void RemoveItem(Item item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
            }
        }
        public List<Event> Events { get; }

    //room constructor takes in description and list of exits - plus list of items - plus list of events
    public Room(string discription, List<string> exits, List<Item> items, List<Event> events)
        {
            Description = discription;
            Exits = exits;
            Items = items;
            Events = events;
        }

        //cannot get this method to work. Using this method always crashes our program
    public void ResolveEvent(Event resolvedEvent)
        {
            if (Events.Contains(resolvedEvent))
            {
                Events.Remove(resolvedEvent);
            }
        }
    }
}
