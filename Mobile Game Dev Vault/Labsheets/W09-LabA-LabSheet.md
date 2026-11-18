# Week 9, Lab A (2h): Art pass, asset budgets and device tiers

**Module:** Mobile Game Development (A12581) · **Engine:** Unity 6.6 · **Platform:** Android (sideload) · **Date:** Mon 9 Nov 2026
**Feeds:** CA2 Playable Vertical Slice (due Sun 15 Nov 2026, 23:59; showcase Wed 11 Nov) · **LOs:** LO3

> **Goal:** Finish one fully art-passed lane inside explicit texture and audio budgets, create three quality levels for Android, and pick the right one automatically by device tier. Re-check the Baseline Sheet so CA2 ships with current numbers.

## What you will complete today
1. Three quality levels (Low, Mid, High) with one URP Asset each and the Part 3 slide values.
2. `DeviceTier.cs` selecting a level at start-up, with a settings-menu override for testing.
3. One lane art-passed with import settings that respect the budgets (ASTC, max sizes, mipmaps, atlases, audio load types).
4. Memory Profiler texture and audio totals, draw calls and frame time recorded in the Baseline Sheet.
5. A tagged release candidate `v0.2-rc1` and the CA2 checklist ticked.

## Pre-flight
- Test plan from Week 8 committed; the worst failure fixed or filed.
- Memory Profiler package installed (**Package Manager > Unity Registry > Memory Profiler**).
- Final art and audio assets for one lane imported (placeholder is fine for the rest).
- Your Week 3 Baseline Sheet peak-memory target to hand.
- Phone charged; Development Build available for the Memory Profiler capture.

## Part A: Create the three quality levels (~15 min)
1. **Edit > Project Settings > Quality**. Delete the default levels you do not use, then add three named exactly **Low**, **Mid**, **High**, in that order (index 0, 1, 2). Tick the **Android** column for all three and set **Mid** as the Android default.
2. Duplicate your URP Asset twice (**Assets > Create > Rendering > URP Asset (with Universal Renderer)** or Ctrl+D on the existing one). Name them `URP-Low`, `URP-Mid`, `URP-High` and assign one per level under **Render Pipeline Asset**.
3. Set the values per level:

| Setting | Low | Mid | High |
|---------|-----|-----|------|
| URP Render Scale | 0.75 | 0.9 | 1.0 |
| URP Anti Aliasing (MSAA) | Disabled | 2x | 4x |
| URP Main Light Shadows | Off | On, 20 m, 1024 | On, 40 m, 2048 |
| URP Post-processing on camera | Off | Bloom only | On |
| Quality: Global Mipmap Limit | Half resolution (1) | Full (0) | Full (0) |
| Quality: Texture Streaming | On | On | On |
| Quality: Anisotropic Textures | Disabled | Per Texture | Forced On |
| Target frame rate (set by DeviceTier) | 30 | 60 | 60 (High FPS allowed) |

4. In each URP Asset, open **Shader Stripping** and untick features you do not use (Additional Light Shadows, Screen Space Shadows, Reflection Probe Blending). Fewer variants, faster builds.
5. Set `Shader.globalMaximumLOD` per tier later from `DeviceTier.cs` if your shaders use `LOD` blocks (300 for Low, 600 for Mid and High).

*Expected result: switching levels in the Quality panel while in Play mode changes MSAA and shadows visibly in the Game view.*

## Part B: DeviceTier.cs and a tier override (~15 min)
1. Create `Assets/Scripts/Performance/DeviceTier.cs` with the deck code (Part 4 slide) and attach it to the persistent boot object.
2. **Edit > Project Settings > Script Execution Order**: add `DeviceTier` at -100 and `PerformanceMode` at -50 so the tier applies first and the Balanced / High FPS toggle then adjusts render scale and frame rate on top.
3. In the settings menu add a **Dropdown - TextMeshPro** with options *Auto, Low, Mid, High*. On change:

```csharp
public void SetTierOverride(int option)   // 0 = Auto, 1..3 = Low..High
{
    if (option == 0) PlayerPrefs.DeleteKey("tier_override");
    else PlayerPrefs.SetInt("tier_override", option - 1);
    PlayerPrefs.Save();
}
```

   The new tier applies on next launch; say so in the label.
4. Add the chosen tier name to your `session_start` telemetry event.
5. Build, run, check logcat for `tier=Mid` (or whatever your phone picks). Force *Low* from the dropdown, relaunch, confirm `tier=Low` and the half-resolution textures.

*Expected result: your phone logs a tier; the override changes it; the Week 7 toggle still works on top.*

## Part C: Art pass on one lane (~40 min)
Pick the lane you will show on Wednesday. Polish only that lane; everything else stays greybox.

1. **Textures** (select the asset, **Inspector > Android tab > Override for Android**):
   - Format **ASTC 6x6** for albedo and emissive; **ASTC 4x4** for normal maps and UI sprites.
   - **Max Size** 1024 for props and characters; 2048 only for atlases and the one hero texture in the lane.
   - **Generate Mipmaps** on for anything rendered in 3D; off for UI sprites.
   - **Streaming Mipmaps** ticked on large 3D textures so the Quality setting can page them out.
2. **Sprites and UI**: create **Assets > Create > 2D > Sprite Atlas** per screen (HUD, menu, in-game UI). Add the folders, tick **Include in Build**, set the atlas to ASTC 4x4, max 2048.
3. **Audio** (select the clip, **Inspector**):
   - Music: **Load Type Streaming**, **Compression Format Vorbis**, **Quality 70**.
   - SFX under 1 s: **Decompress On Load**, **PCM** or **ADPCM**.
   - Longer SFX and voice: **Compressed In Memory**, **Vorbis**.
   - **Force To Mono** on unless the clip is positional stereo you can hear the difference in.
   - **Sample Rate Setting** Override to 22050 Hz for SFX that have no content above 11 kHz.
4. **Materials and lighting**: one URP Lit or Simple Lit material per surface type; bake lighting if the lane is static; no realtime point lights beyond two.
5. **UI**: replace placeholder text with final strings; check every touch target is at least 48 dp and the layout respects `Screen.safeArea`.

*Expected result: the lane looks finished on the phone; the Editor console shows no missing texture or audio warnings.*

## Part D: Measure against the budgets (~15 min)
1. Build with Development Build on and run. **Window > Analysis > Memory Profiler > Capture** from the phone after 60 seconds in the lane.
2. In the snapshot's **Summary**, read the totals for **Texture2D** and **AudioClip** (the *All Of Memory* view groups them). Also note **Managed** heap.
3. Write the budget split and the measured values into the Baseline Sheet:

| Category | Budget (MB) | Measured (MB) | Inside? |
|----------|-------------|---------------|---------|
| Textures (incl. atlases) | | | |
| Audio | | | |
| Managed heap | | | |
| Total PSS (from Week 8) | | | |

4. Attach the Profiler for 60 seconds: frame time mean and 99th percentile, draw calls and batches (Rendering module). Compare with Week 3 and Week 7; note what changed and why.
5. If a category is over budget, fix the top offender now (usually one 4K texture or one uncompressed music track), rebuild and re-measure.

*Expected result: every row inside budget, or a named offender with a fix scheduled before Sunday.*

## Part E: Commit and CA2 checklist (~10 min)
1. Commit and tag: `git tag v0.2-rc1` then push tags. The final CA2 tag will be `v0.2-CA2` after the showcase feedback.
2. Tick the CA2 checklist in `/docs/CA2/checklist.md`:
   - [ ] Release-signed APK installs without ADB on your phone.
   - [ ] Pause and resume tested ten times; no soft lock.
   - [ ] Baseline Sheet complete: frame time, GC allocs, memory, draw calls, thermal soak, cold start, APK size, one bottleneck fixed with before and after.
   - [ ] Telemetry events fire in the right places; event map written.
   - [ ] README with build steps, controls, device targets; Known Issues honest.
   - [ ] Reflective journal 1,000 to 1,500 words, structured, evidence-based.
   - [ ] 3 to 5 minute gameplay capture recorded (boot, menu, one run, pause and resume, relaunch).
3. Confirm your name on the showcase order for **Wed 11 Nov** (alphabetical by surname, blocks of six).

## Troubleshooting
- **`QualitySettings.SetQualityLevel` has no visible effect** -> the level is not ticked for Android, or all three levels share one URP Asset. Check the Quality panel's Android column and the Render Pipeline Asset field per level.
- **Textures look identical on Low** -> Global Mipmap Limit only affects textures with mipmaps; UI sprites without mipmaps stay full size by design.
- **Memory Profiler capture fails** -> Development Build off, or the phone is not the Profiler target. Same fix as the Week 7 Profiler connection.
- **Build size jumped after the art pass** -> a texture without Android override fell back to the default format. Sort the Project window by type and check each texture's Android tab.
- **Audio crackles on Low tier** -> too many Decompress On Load clips playing at once; move the long ones to Compressed In Memory and cap simultaneous voices in the Audio settings.
- **Sprite Atlas not used at runtime** -> *Include in Build* unticked, or a sprite is referenced from a folder outside the atlas. Check the atlas's *Objects for Packing*.

## Checkpoints to commit
1. `ProjectSettings/QualitySettings.asset` and `Assets/Settings/URP-Low.asset`, `URP-Mid.asset`, `URP-High.asset`.
2. `Assets/Scripts/Performance/DeviceTier.cs` and the settings dropdown.
3. Art-passed lane assets with Android overrides; Sprite Atlases; audio import settings.
4. `/docs/CA2/baseline/Performance_Baseline.md` with the budget table and refreshed Week 9 numbers.
5. `/docs/CA2/checklist.md`; tag `v0.2-rc1`.

## Exit ticket
Post the tier your phone picked plus the Memory Profiler texture and audio totals in MB in the Moodle forum, and confirm you are on the showcase list for Wed 11 Nov.

## Reference links
- Quality settings: https://docs.unity3d.com/6000.6/Documentation/Manual/class-QualitySettings.html
- `QualitySettings.SetQualityLevel`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/QualitySettings.SetQualityLevel.html
- `SystemInfo`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/SystemInfo.html
- Texture import overrides per platform: https://docs.unity3d.com/6000.6/Documentation/Manual/class-TextureImporterOverride.html
- Mipmap streaming: https://docs.unity3d.com/6000.6/Documentation/Manual/TextureStreaming.html
- Sprite Atlas: https://docs.unity3d.com/6000.6/Documentation/Manual/class-SpriteAtlas.html
- Audio clip import settings: https://docs.unity3d.com/6000.6/Documentation/Manual/class-AudioClip.html
- `Shader.globalMaximumLOD`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Shader-globalMaximumLOD.html
- `ShaderVariantCollection`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/ShaderVariantCollection.html
- URP package manual (asset, shader stripping): https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest
- Memory Profiler package: https://docs.unity3d.com/Packages/com.unity.memoryprofiler@latest
- Unity 6.6 manual index: https://docs.unity3d.com/6000.6/Documentation/Manual/index.html
