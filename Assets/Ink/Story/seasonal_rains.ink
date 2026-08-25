=== seasonal_rains ===

# rain: heavy

{
    - forest_stock < 50:
        ~ river_clarity -= 25
        ~ soil_fertility -= 15
        # Mother Nature # system_impact: degradation_cascade # env_state: 2 # audio: heavy_rain_erosion
        The rains come, and there is nothing left to hold them.
        Mud finds the river before the river finds the sea.
    - else:
        # Mother Nature # system_impact: balancing_loop_maintained
        The rains come. Roots drink first, and the river stays clear a while longer.
}
-> DONE