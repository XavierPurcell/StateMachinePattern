# State Machine Pattern Platformer

## What is this place about ?

Proof of concent for a state machine pattern for a general character controller.

Most tutorials/guides or even production code at times can end up IF ELSE monsters for character controllers.

## State Machine Pattern Why?

Example of an IF ELSE monster from a game that sold millions of copies -> https://github.com/NoelFB/Celeste/blob/master/Source/Player/Player.cs (Again still a fine solution)

Example of someonme trying to tackle the above code without the state pattern -> https://www.youtube.com/watch?v=rJECT58CQHs&t=260s

Nearly all character controller code you'll find online doesn't scale because they don't use this pattern or some derivative.

## Why We Need It

![image](/StateMachine.png)

Imagine trying to manage and add to that.

Based my implementation on this video: https://www.youtube.com/watch?v=OjreMoAG9Ec

## Notes
- General platformer game