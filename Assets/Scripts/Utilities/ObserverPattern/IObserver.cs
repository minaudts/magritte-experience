public interface IObserver {

    // subject uses this method to communicate with the observer
    // note that type of data passed in OnNotify varies depending on use case
    public void OnNotify();
}