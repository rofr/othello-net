othello-net
===========

Othello game engine

I revisited some very old java code (1996) to see how I would have done it differently today (Dec 2012). Luckily, there was room for improvement :) This is the result after about 10 hours of porting and refactoring.
 
The engine uses alphabeta search and composable board evaluation including mobility, frontier discs, material and corner pattern analysis.

There is a primitive console application so you can play against the engine. Good luck beating it!

Ideas for improvement:
* transposition table - cache board evaluations
* More evaluators - stable discs, wedges, regions and parity
* Move ordering - alphabeta will prune more of the tree if good moves are evaluated first
* Adaptive weighting - evaluation weights depending on the stage of the game
* Timeframed evaluation - currently uses a fixed depth
* Change board representation from Disc[8,8] to two ulong
* Add unit tests :)
* Computer vs. Computer play
