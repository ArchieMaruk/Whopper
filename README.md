# Whopper

Option: 2 – Endless Runner Engine: Unity 6.6 (6000.6.0f1) · Platform: Android (sideload)

A portrait endless runner. Swipe to steer a burger through a kitchen: dodge bottles, jump knives, slide under dish racks, collect pickups and complete missions for coins.

## Controls

|Swipe|Action|
|---|---|
|Left / right|Change lane (works in the air too)|
|Up|Jump|
|Down|Slide (in the air: slam down, then slide on landing)|
|Android back|Pause during a run; back to the menu from the Result screen; quit from the menu|

Swipes trigger as soon as the finger has moved far enough; you don't need to lift your finger.

**Goal:** survive 2 minutes ("Order up!"). Hitting a hazard ends the run ("Burnt!"). Each run has 3 random missions that pay coins. Spend coins on the Result screen to make pickups worth more.

## Test device

### Main phone (from 3 Nov 2026)

- Phone: Samsung Galaxy S26+ (SM-S947B)
- Android: 17
- Refresh rate: up to 120 Hz
- Architecture: ARM64

### Earlier phone (Weeks 1–7)

- Phone: Samsung Galaxy A02s (SM-A025F)
- Android: 12 (API 31)
- Graphics API: OpenGL ES 3
- Boot line: `[Boot] samsung SM-A025F | Android OS 12 / API-31 (SP1A.210812.016/A025FXXS8CXH1) | OpenGLES3 | 720x1600 @ 280 dpi`

## Build profiles

- Android Dev: Development Build on, Autoconnect Profiler on. Used for profiling and testing.
- Android Release: Development Build off, App Bundle off. Used for release APKs.

## Build

1. Clone the repo and open the project folder in Unity 6.6 (6000.6.0f1).
2. File > Build Profiles, select Android, then Switch Platform.
3. Edit > Project Settings > Player > Android > Other Settings, and check:
    - Package Name: `com.archil.whopper`
    - Version: `0.7.0`, Bundle Version Code: `9`
    - Scripting Backend: IL2CPP
    - Target Architectures: ARM64
4. Publishing Settings: select the release keystore (see Signing) and enter its passwords.
5. File > Build Profiles > Android Release, then Build.
6. Save to `releases/MyGame-0.7.0-arm64.apk`.
7. Install on a phone with USB debugging on:

```
adb devices
adb install -r releases/MyGame-0.7.0-arm64.apk
adb shell monkey -p com.archil.whopper 1
adb logcat -s Unity
```

Or copy the APK to the phone and open it (allow installing from unknown sources).

In the editor, always press Play from the **Boot** scene: it creates the persistent App object (wallet, scene loader, performance mode).

## Device targets

- Minimum API: 26 (Android 8.0)
- Target API: Automatic (highest installed); see Play's current policy: [https://support.google.com/googleplay/android-developer/answer/11926878](https://support.google.com/googleplay/android-developer/answer/11926878)
- Orientation: portrait
- Tested devices: see `docs/CA1/device-matrix.md` and `docs/CA3/qa/device-matrix.md`

## Internet and data

- The game doesn't need internet access. Unity adds the INTERNET permission by default.
- Coins, upgrade level and settings are saved locally with PlayerPrefs.
- Telemetry is written only to logcat and a local `telemetry.log`; nothing leaves the device (see `docs/CA1/privacy-statement.md`).

## Branching

- `main` holds working, tested builds.
- New features go on `feat/<name>` branches (e.g. `feat/missions`), bug fixes on `fix/<name>` (e.g. `fix/swipe-on-release`), then merge into `main`.
- Weeks 1–4 were committed straight to `main`; from Week 5 onwards all work uses `feat/` and `fix/` branches.

## Versioning

- Package name: `com.archil.whopper`. It was changed once in Week 4 (from `com.archil.Whopper`) to follow the lower-case convention, before any release. It will not change again.
- versionCode is increased for every release build (currently 9, versionName 0.7.0).

## Signing

- Keystore file: `mygame-release.keystore`, stored outside the repo (local Desktop/Keystores folder, backup on OneDrive)
- Keystore type: PKCS12
- Alias: `mygame`
- Valid: 19 Sep 2026 to 06 Sep 2076
- SHA-256 fingerprint: C3:35:79:4F:94:0C:C1:B2:94:10:23:23:EC:F1:CB:6C:42:FC:91:52:8A:E7:68:40:BF:20:5A:66:86:76:2C:A8
- Passwords are kept in a password manager, never in the repo.
- Note: keytool warns the certificate uses SHA1withRSA (weak). Fine for sideloading; a new SHA-256 key would be needed before a real store release.

## Known Issues

Full details in KnownIssues.md.

- Memory over budget on the S26+ (TOTAL PSS 374 MB vs 300 MB budget).
- Some hazard models are smaller than their shared box collider.
- No scenery beside the lane yet.
- Fixed: swipe only triggering on finger lift; impossible three-wide hazard walls.

## Assets and credits

- Kenney – Food Kit (CC0)
- Kay Lousberg – KayKit Restaurant Bits (CC0)
- ambientCG – Tiles074 floor texture (CC0)

## AI assistance

I used Claude (Anthropic) as a step-by-step guide while working through the lab sheets, mainly to explain settings and error logs. I reviewed and edited everything it suggested. All builds, installs and measurements were carried out by me on my own device.

For the vertical slice I also used it for:

- **Code suggestions** for gameplay scripts (swipe input, runner controller, missions, chunk spawner, random models, floor scroll). I made every change myself, tested it on my phone, and found and fixed problems such as overlapping chunks and an impossible zigzag pattern.
- **Two generated assets:** the burger player model (made with a Python script and exported as OBJ) and the main menu background image. I imported, set up and animated them in Unity.
- **Docs:** help structuring and wording the CA2 documents. All numbers come from my own measurements on the device.