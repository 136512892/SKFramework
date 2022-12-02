namespace SK.Framework.Events
{
    public abstract class EventArgsPack 
    {
        public abstract int PackID { get; protected set; }

        public EventArgsPack(int packId)
        {
            PackID = packId;
        }
    }
}