# Week 10, Lab A (2h): Store presence that converts: assets and copy

**Module:** Mobile Game Development (A12581) · **Engine:** Unity 6.6 · **Platform:** Android (sideload) · **Date:** Mon 16 Nov 2026
**Feeds:** CA3 Performance & Quality Dossier + Final Build (due Sun 6 Dec 2026, 23:59): presentation video slides and portfolio; Async Pack 2 Store Presence Draft (due Tue 17 Nov 2026, 10:00) · **LOs:** LO2

> **Goal:** Turn your Pack 2 draft into real product-page assets at the exact Play specifications, capture in-game screenshots at store resolution, and write the short and long descriptions. Awareness only: nothing is uploaded this semester.

## What you will complete today
1. A 512x512 32-bit PNG app icon that still reads at 48 px.
2. A 1024x500 feature graphic (JPEG or 24-bit PNG, no alpha).
3. Four to six in-game screenshots captured on your phone with the HUD hidden, at 9:16 or 16:9, each side 1080 to 3840 px.
4. An asset checklist in `/docs/store/asset-checklist.md` with every file ticked against the spec.
5. A short description (80 characters maximum) and a long description (4000 characters maximum) in `/docs/store/descriptions.md`.

## Pre-flight
- Your Pack 2 draft: icon moodboard, 4 to 6 screenshot plan, short and long description drafts, 2 to 3 competitor scan. It is due Tue 17 Nov 2026, 10:00 for the Lab B critique.
- Your CA2 build (or newer) installed on your phone, plus a Development Build of the same project for the capture trigger.
- An image editor you can use: Krita, GIMP, Photopea, Figma or similar. Nothing needs to be paid for.
- `adb devices` lists your phone. USB debugging on.
- A `/docs/store/` folder in your repo.

## Part A: Export the icon at 512x512 (~15 min)
1. From your moodboard pick **one** silhouette and one focal colour. No text, no thin lines, no photo.
2. Work on a square canvas at 1024x1024, then export at **512x512, 32-bit PNG**, under 1024 KB. Play masks the icon into its own shape, so do not round the corners yourself; keep the subject inside the central 80%.
3. Test it small: copy the PNG to your phone (`adb push icon-512.png /sdcard/Download/`), set it as a contact photo or open it in a gallery at thumbnail size, and put the phone next to a real launcher grid.
4. Ask the person next to you what the icon is. If they need more than two seconds, simplify.

*Expected result: `/docs/store/icon-512.png`, and a one-line note in the checklist saying what the silhouette is.*

## Part B: Compose the feature graphic at 1024x500 (~15 min)
1. New canvas **1024x500**. Show the game experience: one hero moment from your game plus the title, in a palette that extends your icon without repeating it (Play warns against branding that duplicates the icon).
2. Put the focal point and every key element (title, logo, slogan, main character) towards the **centre**; keep only background towards the edges, which some Play formats crop. Leave out device frames: Play advises against device imagery. Play overlays a play button on the graphic when a preview video does not autoplay, so keep the subject large enough to read around it.
3. Use vibrant colour: pure white, black and dark grey blend into Play's own background. Leave out fine detail; it disappears on a phone.
4. Export as **JPEG or 24-bit PNG with no alpha channel**. In GIMP: **Image > Flatten Image** before export; in Photopea: **File > Export as > JPG**.
5. Check the file properties (Explorer > **Properties > Details**, or `magick identify feature.png` if ImageMagick is installed): 1024x500, no transparency.

*Expected result: `/docs/store/feature-1024x500.png` (or `.jpg`).*

## Part C: Add StoreShot and capture 4 to 6 screenshots (~30 min)
1. Create `Assets/Scripts/Store/StoreShot.cs` with the code from the deck:

```csharp
using UnityEngine;

public class StoreShot : MonoBehaviour
{
    [SerializeField] private GameObject hudRoot;   // HUD to hide
    [SerializeField] private int superSize = 1;    // 2 = double res

    public async Awaitable CaptureAsync(string tag)
    {
        hudRoot.SetActive(false);
        await Awaitable.NextFrameAsync();          // HUD is gone now
        string file = $"{tag}_{Screen.width}x{Screen.height}.png";
        // Android: Unity prepends Application.persistentDataPath
        ScreenCapture.CaptureScreenshot(file, superSize);
        await Awaitable.WaitForSecondsAsync(0.5f); // file write
        hudRoot.SetActive(true);
        Debug.Log($"Saved {Application.persistentDataPath}/{file}");
    }
}
```

2. Add a trigger that only exists in Development Builds. Attach this to a small UI Button in a corner of the HUD:

```csharp
using UnityEngine;
using UnityEngine.UI;

public class DebugShotButton : MonoBehaviour
{
    [SerializeField] private StoreShot shot;
    [SerializeField] private Button button;
    private int _n;

    private void Awake()
    {
        gameObject.SetActive(Debug.isDebugBuild);
        button.onClick.AddListener(() => _ = shot.CaptureAsync($"shot{_n++:00}"));
    }
}
```

3. Assign `hudRoot` (the HUD canvas or its root panel; the debug button should be under it so it hides too). Leave `superSize` at 1 on a 1080p or higher phone; set 2 only on a 720p phone so the short side clears 1080 px.
4. **File > Build Profiles > Android**, Development Build ticked, **Build And Run**.
5. Play to the moments in your screenshot plan (the loot drop, the near miss, the combo) and tap the button. For Play's promotion formats a game needs at least three 16:9 landscape shots (1920x1080 or more) or three 9:16 portrait shots (1080x1920 or more), so capture at least three such moments in your game's own orientation. Watch **Window > Analysis > Android Logcat** for the `Saved ...` line.
6. Pull the files: `adb pull /sdcard/Android/data/com.yourname.game/files/ docs/store/screenshots/` (replace the package name; the folder is `Application.persistentDataPath`).
7. Open each PNG and check the pixel size. Delete near-duplicates; keep 4 to 6.
8. One edit pass per shot: many phones are 20:9 or taller (for example 1080x2400), and Play refuses a long side more than twice the short one, so crop or pad to **9:16** (1080x1920) in portrait or **16:9** (1920x1080) in landscape; in the same edit, crop out or cover Unity's "Development Build" watermark in the bottom-right corner. If the lab time runs out, finish this pass before Lab B.

*Expected result: 4 to 6 PNGs in `/docs/store/screenshots/` at 9:16 or 16:9, each side 1080 to 3840 px, no HUD and no watermark visible.*

## Part D: Check every file against the spec (~10 min)
1. Create `/docs/store/asset-checklist.md` with a table: file, required spec, actual size and format, tick.
2. Rows: icon (512x512, 32-bit PNG, under 1024 KB), feature graphic (1024x500, JPEG or 24-bit PNG, no alpha, focal point towards the centre), each screenshot (Play's rule: 320 to 3840 px per side, long side at most twice the short, JPEG or 24-bit PNG; this lab's target: 9:16 or 16:9, each side 1080 to 3840 px), count (up to 8 per device type, at least 2 in total; for promotion, apps at least four at 1080 px or more, games at least three 16:9 or three 9:16, see Part C step 5).
3. Add a line: "Specs checked against the Play *Add preview assets* help page on 16 Nov 2026" with the link from the references below. Specs change; the date proves you looked.

*Expected result: every row ticked, or a note saying what still needs re-exporting.*

## Part E: Write the short and long descriptions (~20 min)
1. In `/docs/store/descriptions.md`, write the **short description** first: one line, **80 characters maximum**, a verb, a feeling and the hook. Count it (`wc -m` in Git Bash, or the editor's character count). No "best", no "addictive", no exclamation marks.
2. Write two alternative short descriptions: you will A/B them in Lab B.
3. Write the **long description**, 4000 characters maximum. First three lines carry the pitch (they show before *Read more*). Then a feature list of five to eight lines, one idea per line. Finish with one honest data sentence, for example "No account, no ads, no data leaves your phone; progress is saved locally."
4. Read the long description against your actual build. Every feature named must exist in the CA2 build or the current sprint. Cut anything speculative.

*Expected result: `/docs/store/descriptions.md` with short A, short B, and the long description.*

## Troubleshooting
- **Screenshot file never appears** -> `hudRoot` was null (exception in Logcat before the capture), or you looked in the wrong folder: use the exact path printed by the `Saved ...` log line.
- **Screenshot still shows the HUD** -> the button or another canvas is not under `hudRoot`; capture happens the frame after `SetActive(false)`, so anything outside that root stays visible.
- **Image is 720 px wide** -> older phone; set `superSize` to 2 and capture again.
- **Screenshot rejected as too long, or too big** -> a 20:9 capture breaks Play's 2:1 limit, and a long side over 3840 px breaks its maximum; crop or pad to 9:16 or 16:9, and keep `superSize` at 1 on a 1080p phone.
- **"Development Build" in the corner of every shot** -> Unity's watermark on Development Builds; crop or cover it before the file goes in the checklist.
- **Feature graphic export has transparency** -> flatten the image before exporting, or export as JPEG.
- **Icon looks blurry at 48 px** -> too much detail; remove secondary shapes and increase contrast between subject and background.
- **`adb pull` says permission denied** -> the app-specific folder under `/sdcard/Android/data/` is readable for debuggable builds; make sure you pulled from the Development Build, or copy the file from the Files app on the phone.

## Checkpoints to commit
1. `/docs/store/icon-512.png` and `/docs/store/feature-1024x500.png` (or `.jpg`).
2. `/docs/store/screenshots/` with 4 to 6 PNGs at store resolution.
3. `/docs/store/asset-checklist.md` with every row ticked and dated.
4. `/docs/store/descriptions.md` with short A, short B and the long description.
5. `Assets/Scripts/Store/StoreShot.cs` and `DebugShotButton.cs` (the button is Development Build only; both come out in Week 12).

## Exit ticket
Post your icon, your best screenshot and your 80-character short description in the Moodle forum before you leave, ready for the Lab B A/B critique.

## Reference links
- Play Console help, Add preview assets to showcase your app (icon, feature graphic, screenshot specs): https://support.google.com/googleplay/android-developer/answer/9866151
- Unity 6.6 Script Reference, `ScreenCapture.CaptureScreenshot`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/ScreenCapture.CaptureScreenshot.html
- Unity 6.6 Script Reference, `Application.persistentDataPath`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Application-persistentDataPath.html
- Unity 6.6 Script Reference, `Awaitable.NextFrameAsync`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Awaitable.NextFrameAsync.html
- Unity 6.6 Script Reference, `Awaitable.WaitForSecondsAsync`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Awaitable.WaitForSecondsAsync.html
- Unity 6.6 Script Reference, `Debug.isDebugBuild`: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Debug-isDebugBuild.html
- Android Debug Bridge, `adb pull` and `adb push`: https://developer.android.com/tools/adb
- Android Logcat package: https://docs.unity3d.com/Packages/com.unity.mobile.android-logcat@latest
