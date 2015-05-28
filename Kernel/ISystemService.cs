using System.Collections;

public interface ISystemService
{
    IEventAggregator EventAggregator { get; set; }

    /// <summary>
    /// The setup method is called when the controller is first created and has been injected.  Use this
    /// to subscribe to any events on the EventAggregator
    /// </summary>
    void Setup();

    IEnumerator SetupAsync();

}