// decision_point_2.ink
// Acts on river_clarity. Option A = balancing loop, Option B = reinforcing loop.
// This is a mandatory active choice regardless of Decision Point 1's outcome.

=== decision_point_2 ===
+ [Replant and build soft barriers] -> restoration
+ [Wall the river in concrete] -> retaining_wall

=== restoration ===
~ river_clarity += 5
# Mother Nature 
# water_recovering
# loop_balancing
Human hands go back into the mud, not to fight it, but to hold it in place.
-> end_state_check

=== retaining_wall ===
~ river_clarity -= 20
# Mother Nature 
# water_choked
# loop_reinforcing
The concrete goes up fast. The river goes quiet where it meets the wall.
-> end_state_check
