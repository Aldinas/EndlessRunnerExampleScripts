using UnityEngine;

public class ControllerSingleton : Singleton<ControllerSingleton>
{
    // (Optional) Prevent non-singleton constructor use.
    protected ControllerSingleton() { }
}