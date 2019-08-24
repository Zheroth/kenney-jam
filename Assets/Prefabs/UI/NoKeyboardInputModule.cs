using UnityEngine.EventSystems;

class NoKeyboardInputModule : StandaloneInputModule
{
    public override void Process()
    {
        bool usedEvent = SendUpdateEventToSelectedObject();

        ProcessMouseEvent();
    }
}