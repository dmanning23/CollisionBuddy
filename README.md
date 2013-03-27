CollisionBuddy
==============

A quick little c# XNA/Monogame collision detection library for circle/line and tunnel-free circle/circle.  Good for shmup!

This library checks for fast circle-circle collisions.  It also prevents tunnelling... like say you shoot a bullet at an incoming missile, but the velocity is greater than the radius of the bullet, and it goes right through the missle because the collision would've occured between updates.  That's tunnelling, and it sucks.

It also checks for circle-line collision, but that one doesn't prevent tunneling.

There is a cirlce-rect check in there that also checks for tunnelling.  Assume you have an axis aligned box, you want the circle to stay inside that box.  This method checks if it is touching or outside the walls, and if it is gives you a vector you can add to the circle position to put it back inside the box.

For an example of how to use this dude, check out the sample at https://github.com/dmanning23/CollisionBuddySample

Cheers!