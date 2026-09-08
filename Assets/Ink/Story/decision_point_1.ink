=== decision_point_1 ===
The settlement needs shelter and fuel. How do we take from the forest?

+ [Selective harvest at the outskirts] 
    -> silviculture
+ [Clear the central forest] 
    -> clearcut

=== silviculture ===
~ forest_stock -= 10
~ soil_fertility -= 5
Axes ring only at the tree line. # Mother Nature # system_impact: balancing_loop_maintained # env_state: 1 # audio: chopwood 
The work is slow, but the canopy holds. # Mother Nature
The forest's regeneration keeps pace with our axes. # Mother Nature
-> seasonal_rains

=== clearcut ===
~ forest_stock -= 60
~ soil_fertility -= 30
The saws don't stop at the tree line. # Mother Nature # system_impact: reinforcing_loop_triggered # env_state: 2 # bg: barren_stumps # audio: chopwood # visual_fx: pale_grass_patches # audio: degraded_forest
By dusk, stumps stand where shade once was. # Mother Nature
Without the shade, the soil bakes in the low heat. # Mother Nature
-> seasonal_rains