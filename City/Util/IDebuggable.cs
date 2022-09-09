namespace City.Util
{
    public interface IDebuggable
    {
        bool Debug { get; }

        public virtual void DebugMesage(Func<string> message)
        {
            if (Debug)
            {
                System.Diagnostics.Debug.WriteLine(message());
            }
        }
    }
}
