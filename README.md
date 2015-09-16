Multi-Pac
================================

A project that wanted to create a networked pac-man game. This was a class asingment to create a networked multiplayer game.
Several ideas were realised by different teams. Some would create a turn-based game, were network latency could be ignored.
This project however tried to create a real-time multiplayer game.
As such, certain techniques had to be developed to prevent lag from interfering with the gameplay.

Several simplifications were made, such as that the AI ghosts would behave everywhere the same and didn't need a server side simulation (although they would still sync their positions at certain intervals).

For compensating lag on player input, a [paper](https://developer.valvesoftware.com/wiki/Source_Multiplayer_Networking) released by [Valve](http://www.valvesoftware.com/) was studied and adapted to work with our engine.

The engine used several libraries:
* [XNA](https://de.wikipedia.org/wiki/XNA_%28Microsoft%29)
* [Lidgren](https://code.google.com/archive/p/lidgren-network-gen3/)
