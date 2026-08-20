=== decision_point_1 ===
Winter is approaching faster than expected. The community shivers in the damp nights. The settlement desperately needs immediate shelter and fuel to survive the cold. 

How do we take from the forest?

+ [Sustainable Silviculture: Harvest selectively at the outskirts] 
    -> silviculture
+ [Clear-cutting: Level the central forest for immediate, high yield] 
    -> clearcut

=== silviculture ===
~ forest_stock -= 10
Axes ring only at the tree line. # Mother Nature # system_impact: balancing_loop_maintained # env_state: 1
The work is agonizingly slow, and our builders complain, but the canopy holds. # Mother Nature
The forest's regeneration easily keeps pace with our axes. We have less, but we have enough. # Mother Nature
-> seasonal_rains

=== clearcut ===
~ forest_stock -= 60
~ soil_fertility -= 30
The saws do not stop at the tree line. # Mother Nature # system_impact: reinforcing_loop_triggered # env_state: 2 # bg: barren_stumps # audio: low_hollow_drone # visual_fx: pale_grass_patches
We prioritize our immediate survival. # Mother Nature
By dusk, our wood stores are overflowing. The settlement feels secure, warm, and fast-growing. # Mother Nature
But behind the houses, the vibrant green canopy is gone. Just barren, muddy stumps remain. Without the shade, the soil bakes. # Mother Nature
The light acoustic wind fades from the valley, replaced by a low, hollow drone. # Mother Nature
-> seasonal_rains