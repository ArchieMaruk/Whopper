# Known Issues – Whopper

## Open

### 1. Memory over budget on the new phone

- Scenario: 5 minutes of play
- Steps: play for 5 min, then `adb shell dumpsys meminfo com.archil.whopper`
- Expected: TOTAL PSS under 300 MB (MDA budget)
- Actual: TOTAL PSS 374 MB
- Log line: `TOTAL PSS: 382937`
- Build: 0.6.0 (28fbc052), Galaxy S26+, Android 17

### 2. Some hazard models are smaller than their collider

- Scenario: normal play
- Steps: play until a jar or carton spawns, and brush past its edge
- Expected: the player is only hit when they visibly touch the model
- Actual: every model in a hazard shares one box collider, so smaller models can hit the player slightly before they appear to touch
- Build: 0.7.0, Galaxy S26+, Android 17

### 3. No scenery beside the lane

- Scenario: normal play
- Expected: kitchen walls or counters along both sides of the lane
- Actual: the lane floats over the default sky
- Build: 0.7.0, Galaxy S26+, Android 17

## Fixed

### Swipe only triggers when the finger lifts

- Found in: Week 2 accessibility playtest; build 0.6.0 (28fbc052)
- Fixed: 9 Nov 2026 (feat/vertical-slice). Swipes now trigger as soon as the finger has moved far enough, and each finger only triggers one swipe.

### Impossible three-wide hazard walls

- Found in: Week 9 playtesting; build 0.7.0
- Cause: long chunks overlapped the next chunk at high speed, and a jump (about 26 m at top speed) landed inside the next chunk
- Fixed: 11 Nov 2026. The spawner now waits for each chunk's length, chunks are about 30 m apart at top speed, and the zigzag moves one lane per row.