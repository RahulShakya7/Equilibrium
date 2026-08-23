=== decision_point_2 ===
The river swells with the rains, threatening to spill into the settlement basin.

+ [Raise a Stone-and-Earth Levee: Pack clay and river stone into the banks now] 
    -> retaining_wall
+ [Construct Bioswales: Replant native shrubs and build soft barriers] 
    -> restoration

=== retaining_wall
~ river_clarity -= 20
~ soil_fertility -= 10
~ settlement_order += 10
We drive stakes into the bank and pack it tight with clay and stone, holding the water back by force. # Mother Nature # system_impact: reinforcing_loop_triggered # river_state: 2 # audio: muted_dull_current # visual_fx: silted_brown_water
The barrier holds, but the river no longer reaches the fields it once fed. # Mother Nature
The water runs thick with the silt it can no longer shed, and the crops nearest the bank begin to wilt. # Mother Nature
-> end_state_check

=== restoration ===
~ river_clarity += 12
{soil_fertility >= 70:
    ~ river_clarity += 5
}
~ soil_fertility += 10
Dedicating our labor to the land slows growth, but natural barriers take root. # Mother Nature # system_impact: balancing_loop_maintained # river_state: 1 # audio: soft_river_flow
Human hands go back into the mud to hold it in place. # Mother Nature
The soil stabilizes, and the stream filters naturally through the new bioswales. # Mother Nature
-> end_state_check