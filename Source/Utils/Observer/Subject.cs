namespace Application.Source.Utils.Observer
{
    public abstract class Subject
    {
        private readonly List<IObserver> handler;

        protected Subject()
        {
            handler = [];
        }

        public void Subscribe(IObserver observer)
        {
            handler.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            handler.Remove(observer);
        }

        public virtual void Notify()
        {
            foreach (var observer in handler)
            {
                observer.Update(this);
            }
        }
    }
}