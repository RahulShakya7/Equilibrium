// decision_point_2.ink
// Acts on river_clarity. Option A = balancing loop, Option B = reinforcing loop.

=== decision_point_2 ===
The settlement must act to secure the water supply against the swelling current.

+ [Construct Bioswales: Replant native shrubs and build soft barriers] 
    -> restoration
+ [Build Retaining Wall: Channel the river in concrete] 
    -> retaining_wall

=== restoration ===
~ river_clarity += 5
Dedicating our labor to the land slows growth, but natural barriers take root. # Mother Nature # system_impact: balancing_loop_maintained # river_state: 1 # audio: soft_river_flow
Human hands go back into the mud to hold it in place. # Mother Nature
The soil stabilizes, and the stream filters naturally through the new bioswales. # Mother Nature
-> end_state_check

=== retaining_wall ===
~ river_clarity -= 20
~ settlement_order += 10 
We channel the river in concrete, bypassing the mud entirely. # Mother Nature # system_impact: reinforcing_loop_triggered # river_state: 2 # audio: muted_dull_current # visual_fx: murky_grey_water
The concrete goes up fast, but the river goes quiet where it meets the wall. # Mother Nature
The translucent blue water turns to a dull, chemical-grey hue as crops begin to wilt. # Mother Nature
-> end_state_check