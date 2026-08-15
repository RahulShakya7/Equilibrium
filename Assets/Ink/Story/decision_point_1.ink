=== decision_point_1 ===
The settlement needs shelter and fuel. How do we take from the forest?

+ [Selective harvest at the outskirts] -> silviculture
+ [Clear the central forest] -> clearcut

=== silviculture ===
~ forest_stock -= 10
# Mother Nature
Axes ring only at the tree line. The canopy holds.
-> END

=== clearcut ===
~ forest_stock -= 60
~ soil_fertility -= 30
# Mother Nature
The saws don't stop at the tree line.
By dusk, stumps stand where shade once was.
-> END