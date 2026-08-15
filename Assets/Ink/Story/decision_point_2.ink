// decision_point_2.ink
// Acts on river_clarity. Option A = balancing loop, Option B = reinforcing loop.
// This is a mandatory active choice regardless of Decision Point 1's outcome.

=== decision_point_2 ===
The settlement must act to secure the water supply against the swelling current.

+ [Construct Bioswales: Replant native shrubs and build soft barriers] 
    -> restoration
+ [Build Retaining Wall: Channel the river in concrete] 
    -> retaining_wall

=== restoration ===
~ river_clarity += 5
// Initiates a balancing loop. Increases natural capital recovery.
# Mother Nature 
# water_recovering
# loop_balancing

Dedicating our labor to the land slows the settlement's immediate growth, but the natural barriers begin to take root. 

Human hands go back into the mud, not to fight it, but to hold it in place. The soil stabilizes, and the stream filters naturally through the new bioswales.
-> end_state_check

=== retaining_wall ===
~ river_clarity -= 20
~ settlement_order += 10 
// Increases human infrastructure, but permanently chokes water flow.
# Mother Nature 
# water_choked
# loop_reinforcing

We channel the river, bypassing the mud entirely. The concrete goes up fast, rapidly restoring order to the settlement. 

But the river goes quiet where it meets the wall, permanently disconnected from its natural floodplain. Over the coming weeks, the translucent blue water turns to a dull, chemical-grey hue. The small fish that used to dart through the shallows disappear, and the crops planted nearest to the concrete banks begin to wilt.
-> end_state_check