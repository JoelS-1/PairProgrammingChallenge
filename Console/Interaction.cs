using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DungeonGame.ProgramUI;

namespace DungeonGame
{
    public class Event
    {
        //public enum EventType { Battle, Use }
        //public EventType Type;
        //above types not used or needed. May add in later

        public string TriggerPhrase;
        public Result EventResult;

        //event class - gives us a constructor to add event objects to our rooms
        public Event(string triggerPhrase, 
            //EventType eventType, 
            Result eventResult)
        {
            //trigger phrase - the text we must type in to trigger the event
            TriggerPhrase = triggerPhrase;

            //what the event is -is not needed currently
            //Type = eventType;

            //the result of the event
            EventResult = eventResult;
        }

    }

    //result class - gives us results for our events to trigger
    public class Result
    {
        public enum ResultType { GetItem, MessageOnly, GameOver }
        public ResultType Type { get; }

        public Item ResultItem { get; }
        public string ResultMessage { get; }
        public bool GameOver { get; }

        //constructors based on what the event will give you

        //this event gives you a new item
        public Result(Item resultItem, string resultMessage)
        {
            Type = ResultType.GetItem;
            ResultItem = resultItem;
            ResultMessage = resultMessage;
        }

        //this event gives you a special message
        public Result(string resultMessage)
        {
            Type = ResultType.MessageOnly;
            ResultMessage = resultMessage;
        }

        //this event kills the user and ends the game - just a method with an extra bool in the constructor for clarity
        public Result(string resultMessage, bool gameOver)
        {
            Type = ResultType.GameOver;
            ResultMessage = resultMessage;
            GameOver = gameOver;
        }

        


    }
}
