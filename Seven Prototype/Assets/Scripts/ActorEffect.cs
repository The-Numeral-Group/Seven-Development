/*ActorEffect interface allows any object that inherits
from it to passively effect the player. Can be made more
general with some improvements to PlayerActor (like making
it inherit from something), but that's a later thing.

The ref makes the PlayerActor object pass by reference,
so the changes are maintained*/
using UnityEngine;

public interface ActorEffect
{
    //the bool represents whether or not the effect can be/was applied.
    //if it returns false, the effect doesn't happen
    //these should be Actor, but that type isn't a thing yet
    bool ApplyEffect(ref MonoBehaviour effectee);

    void RemoveEffect(ref MonoBehaviour effectee);
}
