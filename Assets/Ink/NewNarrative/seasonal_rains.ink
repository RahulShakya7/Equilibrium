// Not a decision - a state-dependent narration beat.
// The wording itself is the feedback for Decision Point 1's outcome.

=== seasonal_rains ===
The seasonal rains arrive, heavy and unforgiving. The valley system reacts to our presence.
{
    - forest_stock < 50:
        Without the canopy, the soil cannot hold the moisture. Without the roots, the earth cannot hold itself. # Mother Nature
        Massive mudslides tear through the barren stumps. Runoff carries thick, choking soil directly into the river, blinding our only water source. # Mother Nature
    - else:
        The rains beat down, but the deep roots drink first. The intact forest absorbs the deluge, keeping the valley stable. # Mother Nature
        The river swells, but stays clear a while longer. # Mother Nature
}
-> decision_point_2